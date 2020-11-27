using Newtonsoft.Json;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, AllowLicense, ValidateInput(true), DandBLicenseEnabled]
    public class MultiPassController : BaseController
    {
        // GET: MultiPass
        public ActionResult Index()
        {
            return View();
        }

        [Route("DNB/MultiPass")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexMultiPass()
        {
            List<MPMSummary> lstConfigs = new List<MPMSummary>();
            MultiPassFacade mFac = new MultiPassFacade(this.CurrentClient.ApplicationDBConnectionString);
            lstConfigs = mFac.GetMPMSummaryByTag(106001, string.Empty);
            if (Request.Headers["X-PJAX"] == "true")
            {
                return View(lstConfigs);
            }
            else
            {
                ViewBag.SelectedTab = "Identity Resolution";
                ViewBag.SelectedIndividualTab = "Multi-Pass Configuration";
                return View("~/Views/DandB/Index.cshtml", lstConfigs);
            }
        }
        [HttpGet, RequestFromSameDomain]
        public ActionResult AddMultiPassConfig()
        {
            return PartialView();
        }

        [HttpGet, RequestFromSameDomain]
        public ActionResult CreateMultiPassGroup(string Parameters)
        {
            string tag = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                tag = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
            }
            ViewBag.Tag = tag;
            ViewBag.IsFromUpdate = false;
            List<MultiPassGroupConfiguration> lstGroups = new List<MultiPassGroupConfiguration>();
            return PartialView(lstGroups);
        }

        [HttpGet, RequestFromSameDomain]
        public ActionResult UpdateMultiPassGroup(string Parameters)
        {
            string tag = string.Empty;
            int provider = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                tag = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                provider = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
            }
            MultiPassFacade mFac = new MultiPassFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<MultiPassGroupConfiguration> lstGroups = new List<MultiPassGroupConfiguration>();
            lstGroups = mFac.GetVerificationGroupLookupList(provider, tag);
            ViewBag.Tag = tag;
            ViewBag.IsFromUpdate = true;
            return PartialView("CreateMultiPassGroup", lstGroups);
        }

        [HttpPost, RequestFromSameDomain]
        public ActionResult SaveMultiPassGroups(List<MultiPassGroupConfiguration> multiPassGroups)
        {
            string category = multiPassGroups[0].Category;
            string tag = multiPassGroups[0].Tag;
            ViewBag.Tag = tag;
            int provider = multiPassGroups[0].ProviderCode;
            MultiPassFacade mFac = new MultiPassFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = mFac.GetProviderLookups(provider);
            MPMPrecedenceSelection lstGroups = new MPMPrecedenceSelection();
            lstGroups.AvailablePrecedence = new List<MPMPrecedenceConfig>();
            lstGroups.SelectedPrecedence = new List<string>();
            if(dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstGroups.AvailablePrecedence.Add(new MPMPrecedenceConfig() { IsVerificationGroup = false, Name = dt.Rows[i]["LookupName"].ToString() });
                }
            }
            if (category == "no")
            {
                ViewBag.IsPrecedenceComplusory = true;
                SessionHelper.ListMultiPassGroupConfigurationForCreation = JsonConvert.SerializeObject(multiPassGroups);
            }
            else
            {
                SessionHelper.ListMultiPassGroupConfigurationForCreation = JsonConvert.SerializeObject(multiPassGroups);
                if (multiPassGroups.Count > 1)
                {
                    ViewBag.IsPrecedenceComplusory = true;
                }
                else
                {
                    ViewBag.IsPrecedenceComplusory = false;
                }
                foreach (var item in multiPassGroups)
                {
                    if (!string.IsNullOrEmpty(item.VerificationGroupName))
                        lstGroups.AvailablePrecedence.Add(new MPMPrecedenceConfig() { IsVerificationGroup = true, Name = item.VerificationGroupName });
                }
            }
            string selected = mFac.GetPrecedenceSteps(provider, tag);
            lstGroups.SelectedPrecedence = !string.IsNullOrEmpty(selected) ? selected.Split(',').ToList() : new List<string>();
            lstGroups.SelectedPrecedence = lstGroups.SelectedPrecedence.Intersect(lstGroups.AvailablePrecedence.Select(x => x.Name)).ToList();
            return PartialView("MultiPassPrecedence", lstGroups);
        }

        [HttpPost, RequestFromSameDomain]
        public ActionResult SaveMultiPassPrecedence(List<MultiPassPrecedence> multiPassPrecedences)
        {
            string message = string.Empty;
            string tag = multiPassPrecedences[0].Tag;
            ViewBag.Tag = tag;
            int provider = multiPassPrecedences[0].ProviderCode;
            string Steps = string.Join(",", multiPassPrecedences.Select(x => x.Steps));
            List<MultiPassGroupConfiguration> lstGroups = new List<MultiPassGroupConfiguration>();
            lstGroups = JsonConvert.DeserializeObject<List<MultiPassGroupConfiguration>>(SessionHelper.ListMultiPassGroupConfigurationForCreation);
            string VGNamesAndLookupIds = string.Empty;
            foreach (var item in lstGroups)
            {
                if (string.IsNullOrEmpty(VGNamesAndLookupIds))
                    VGNamesAndLookupIds = item.VerificationGroupName + ":" + item.VerifiationLookup;
                else
                    VGNamesAndLookupIds = VGNamesAndLookupIds + "|" + item.VerificationGroupName + ":" + item.VerifiationLookup;
            }
            MultiPassFacade mFac = new MultiPassFacade(this.CurrentClient.ApplicationDBConnectionString);
            try
            {
                if (lstGroups[0].Category == "no")
                {
                    message = mFac.ModifyRule(provider, tag, null, Steps, false);
                }
                else
                {
                    message = mFac.ModifyRule(provider, tag, VGNamesAndLookupIds, Steps, false);
                }
                if (string.IsNullOrEmpty(message))
                    return Json(new { result = true, message = CommonMessagesLang.msgCommanInsertMessage });
                else
                    return Json(new { result = false, message = message });

            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage });
            }
        }

        [HttpGet, RequestFromSameDomain]
        public ActionResult DeleteMultiPassConfiguration(string Parameters)
        {
            string tag = string.Empty;
            int provider = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                tag = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                provider = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
            }
            MultiPassFacade mFac = new MultiPassFacade(this.CurrentClient.ApplicationDBConnectionString);
            string message = mFac.ModifyRule(provider, tag, null, null, true);
            if (string.IsNullOrEmpty(message))
                return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
        }

    }
}