using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR"), TwoStepVerification, OrbLicenseEnabled, AllowLicense, ValidateInput(true)]
    public class OISettingController : BaseController
    {

        #region "License"
        // GET: OISetting
        [Route("OI/Setting/License")]
        public ActionResult Index()
        {
            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            ViewBag.SelectedTab = "OI License";
            if (Request.Headers["X-PJAX"] == "true")
            {
                List<ThirdPartyAPICredentialsEntity> lstThirdPartyAPICredentials = new List<ThirdPartyAPICredentialsEntity>();
                ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                lstThirdPartyAPICredentials = fac.GetThirdPartyAPICredentials(ThirdPartyProvider.ORB.ToString());
                OISettingEntity Orbmodel = new OISettingEntity();
                if (lstThirdPartyAPICredentials != null && lstThirdPartyAPICredentials.Any())
                {
                    Orbmodel.ORB_API_KEY = lstThirdPartyAPICredentials.FirstOrDefault().APICredential;
                }
                return PartialView("~/Views/OI/OISetting/IndexOILicense.cshtml", Orbmodel);

            }
            return View("~/Views/OI/OISetting/Index.cshtml");
        }


        public ActionResult IndexOICredentials()
        {
            return PartialView("~/Views/OI/OISetting/IndexOICredentials.cshtml");
        }

        [HttpGet]
        public ActionResult IndexDataImportHandling()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            model.Settings = fac.GetCleanseMatchSettings();
            CommonMethod.GetSettingIDs(model);
            OISettingEntity Orbmodel = new OISettingEntity();
            Orbmodel.ORB_DATA_IMPORT_DUPLICATE_RESOLUTION_TAGS = model.Settings[model.ORB_DATA_IMPORT_DUPLICATE_RESOLUTION_TAGS].SettingValue;
            Orbmodel.ORB_DATA_IMPORT_DUPLICATE_RESOLUTION = model.Settings[model.ORB_DATA_IMPORT_DUPLICATE_RESOLUTION].SettingValue;
            return PartialView("~/Views/OI/OISetting/IndexDataImportHandling.cshtml", Orbmodel);
        }

        [HttpPost, ValidateAntiForgeryToken, RequestFromAjax, RequestFromSameDomain, ValidateInput(true)]
        public ActionResult IndexDataImportHandling(OISettingEntity model)
        {
            if (!string.IsNullOrEmpty(model.ORB_DATA_IMPORT_DUPLICATE_RESOLUTION))
            {
                OISettingFacade fac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.UpdateOrbDataImportHandling(model, "OrbDataImportHandling");
                return Json(new { result = true, message = DandBSettingLang.msgSettingUpdate }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = true, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
            }
        }

        #region "OI Background Process Settings"
        [HttpGet]
        public ActionResult OIBackgroundProcessSettings()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            model.Settings = fac.GetCleanseMatchSettings();
            CommonMethod.GetSettingIDs(model);
            OISettingEntity Orbmodel = new OISettingEntity();
            Orbmodel.ORB_BATCH_SIZE = model.Settings[model.ORB_BATCH_SIZE].SettingValue;
            Orbmodel.ORB_BATCH_WAITTIME_SECS = model.Settings[model.ORB_BATCH_WAITTIME_SECS].SettingValue;
            Orbmodel.ORB_MAX_PARALLEL_THREADS = model.Settings[model.ORB_MAX_PARALLEL_THREADS].SettingValue;
            Orbmodel.PAUSE_ORB_BATCHMATCH_ETL = Convert.ToBoolean(model.Settings[model.PAUSE_ORB_BATCHMATCH_ETL].SettingValue);
            Orbmodel.ORB_ENABLE_CORPORATE_TREE_ENRICHMENT = Convert.ToBoolean(model.Settings[model.ORB_ENABLE_CORPORATE_TREE_ENRICHMENT].SettingValue);
            return PartialView("~/Views/OI/OISetting/OIBackgroundProcessSettings.cshtml", Orbmodel);
        }
        [HttpPost, ValidateAntiForgeryToken, RequestFromAjax, RequestFromSameDomain, ValidateInput(true)]
        public ActionResult OIBackgroundProcessSettings(OISettingEntity model)
        {
            if (!string.IsNullOrEmpty(model.ORB_BATCH_SIZE) && !string.IsNullOrEmpty(model.ORB_BATCH_WAITTIME_SECS) && !string.IsNullOrEmpty(model.ORB_MAX_PARALLEL_THREADS))
            {
                OISettingFacade fac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.UpdateOrbBackgroundSetting(model, "OrbBackgroundSetting");
                Helper.IsEnableCorporateTreeEnrichment = model.ORB_ENABLE_CORPORATE_TREE_ENRICHMENT;
                return Json(new { result = true, message = DandBSettingLang.msgSettingUpdate }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = true, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region D&B Direct 2.0 License 
        [HttpGet]
        public ActionResult IndexOILicense()
        {
            List<ThirdPartyAPICredentialsEntity> lstThirdPartyAPICredentials = new List<ThirdPartyAPICredentialsEntity>();
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            lstThirdPartyAPICredentials = fac.GetThirdPartyAPICredentials(ThirdPartyProvider.ORB.ToString());
            OISettingEntity Orbmodel = new OISettingEntity();
            if (lstThirdPartyAPICredentials != null && lstThirdPartyAPICredentials.Any())
            {
                Orbmodel.ORB_API_KEY = lstThirdPartyAPICredentials.FirstOrDefault().APICredential;
            }

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            ViewBag.SelectedTab = "OI Auto Acceptance";
            return PartialView("~/Views/OI/OISetting/IndexOILicense.cshtml", Orbmodel);
        }
        [HttpPost, ValidateAntiForgeryToken, RequestFromAjax, RequestFromSameDomain, ValidateInput(true)]
        public ActionResult IndexOILicense(OISettingEntity model)
        {

            if (!string.IsNullOrEmpty(model.ORB_API_KEY))
            {
                //Update API License in Main Portal DB
                OISettingFacade fac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.UpdateOrbCredentials(model, "OrbCredentialsSetting");

                //OI - License management for OI API. (MP-590)
                //Update API License in ClientMasert DB
                fac = new OISettingFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase));
                //Get SubDomain  
                string[] hostParts = new System.Uri(Request.Url.AbsoluteUri).Host.Split('.');
                string SubDomain = hostParts[0];
                fac.UpdateOIAPILicenseForClients(SubDomain, model.ORB_API_KEY);   // MP-846 Admin database cleanup and code cleanup.-CLIENT
                return Json(new { result = true, message = DandBSettingLang.msgSettingUpdate }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = true, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        #endregion

        #region "Identity Resolution"
        #region "Auto-Acceptance"
        // Auto-Acceptance
        [Route("OI/Setting/IdentityResolution")]
        public ActionResult IndexOIAutoAcceptance(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            #region Pagination code
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 22;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 30;
            #endregion
            List<OIAutoAcceptanceEntity> model = new List<OIAutoAcceptanceEntity>();
            OIAutoAcceptanceFacade fac = new OIAutoAcceptanceFacade(this.CurrentClient.ApplicationDBConnectionString);
            model = fac.GetAutoAcceptanceRulesPaging(pageSize, currentPageIndex, out totalCount);

            #region "set Viewbag"
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            #endregion
            IPagedList<OIAutoAcceptanceEntity> pagedAcceptance = null;
            if(model != null && model.Count > 0)
            {
               pagedAcceptance = new StaticPagedList<OIAutoAcceptanceEntity>(model.ToList(), currentPageIndex, pageSize, totalCount);
            }

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("~/Views/OI/OISetting/IndexOIAutoAcceptance.cshtml", pagedAcceptance);
            else
            {
                ViewBag.SelectedTab = "OI Auto Acceptance";
                return View("~/Views/OI/OISetting/Index.cshtml", pagedAcceptance);
            }
        }
        // On Adding/Editing Auto-Acceptance Rules
        [HttpGet]
        public ActionResult InsertUpdateOIAutoAcceptance(string Parameters)
        {
            int RuleId = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                RuleId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
            }

            OIAutoAcceptanceFacade fac = new OIAutoAcceptanceFacade(this.CurrentClient.ApplicationDBConnectionString);
            OIAutoAcceptanceEntity model = new OIAutoAcceptanceEntity();
            if (RuleId > 0)
            {
                model = fac.GetAutoAcceptanceRuleById(RuleId);
            }
            return View("~/Views/OI/OISetting/InsertUpdateOIAutoAcceptance.cshtml", model);
        }
        // On Adding/Updating Auto-Acceptance Rules
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult InsertUpdateOIAutoAcceptance(OIAutoAcceptanceEntity obj)
        {
            string MatchGradeValue = "A,B,F,X,Z";

            obj.MG_Company = obj.MG_Company.Replace(" ", "") == MatchGradeValue ? "#" : (obj.MG_Company.Contains("#") ? "#" : obj.MG_Company);
            obj.MG_StreetNo = obj.MG_StreetNo.Replace(" ", "") == MatchGradeValue ? "#" : (obj.MG_StreetNo.Contains("#") ? "#" : obj.MG_StreetNo);
            obj.MG_StreetName = obj.MG_StreetName.Replace(" ", "") == MatchGradeValue ? "#" : (obj.MG_StreetName.Contains("#") ? "#" : obj.MG_StreetName);
            obj.MG_City = obj.MG_City.Replace(" ", "") == MatchGradeValue ? "#" : (obj.MG_City.Contains("#") ? "#" : obj.MG_City);
            obj.MG_State = obj.MG_State.Replace(" ", "") == MatchGradeValue ? "#" : (obj.MG_State.Contains("#") ? "#" : obj.MG_State);
            obj.MG_PostalCode = obj.MG_PostalCode.Replace(" ", "") == MatchGradeValue ? "#" : (obj.MG_PostalCode.Contains("#") ? "#" : obj.MG_PostalCode);
            obj.MG_Phone = obj.MG_Phone.Replace(" ", "") == MatchGradeValue ? "#" : (obj.MG_Phone.Contains("#") ? "#" : obj.MG_Phone);
            obj.MG_Webdomain = obj.MG_Webdomain.Replace(" ", "") == MatchGradeValue ? "#" : (obj.MG_Webdomain.Contains("#") ? "#" : obj.MG_Webdomain);
            obj.MG_Country = obj.MG_Country.Replace(" ", "") == MatchGradeValue ? "#" : (obj.MG_Country.Contains("#") ? "#" : obj.MG_Country);
            obj.MG_EIN = obj.MG_EIN.Replace(" ", "") == MatchGradeValue ? "#" : (obj.MG_EIN.Contains("#") ? "#" : obj.MG_EIN);
            OIAutoAcceptanceFacade fac = new OIAutoAcceptanceFacade(this.CurrentClient.ApplicationDBConnectionString);
            try
            {
                int result = fac.InsertUpdateAutoAcceptanceRules(obj);
                if (obj.RuleId > 0)
                {
                    return Json(new { result = true, message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = true, message = CommonMessagesLang.msgCommanInsertMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // On Deleting Auto-Acceptance Rules
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteAutoAcceptance(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            OIAutoAcceptanceFacade fac = new OIAutoAcceptanceFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.DeleteAutoAcceptance(Parameters);
            return Json(CommonMessagesLang.msgCommanDeleteMessage);
        }
        #endregion
        #endregion
        #region "Reset System Data"
        // Resets OI system data
        [HttpGet]
        public ActionResult ResetOISystemData()
        {
            return View("~/Views/OI/OISetting/ResetOISystemData.cshtml");
        }
        // Resets OI system data is reset button is pressed
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult ResetOISystemsData()
        {
            try
            {
                OISettingFacade fac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                // Reset System Data
                fac.ResetOISystemData();
                return new JsonResult { Data = DandBSettingLang.msgResetDataSuccessfully };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = DandBSettingLang.msgResetDataUnsuccessfully };
            }

        }
        #endregion

        #region "Third Party API Credentials"
        public ActionResult IndexThirdPartyAPICredentials()
        {
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstThirdPartyAPICredentials = fac.GetThirdPartyAPICredentials(ThirdPartyProvider.ORB.ToString());
            return PartialView("~/Views/OI/OISetting/_indexThirdPartyAPICredentials.cshtml", lstThirdPartyAPICredentials);
        }
        public ActionResult AddUpdateThirdPartyOrbCredential(string Parameters)
        {
            int CredentialId = 0;
            ThirdPartyAPICredentialsEntity model = new ThirdPartyAPICredentialsEntity();
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                CredentialId = Convert.ToInt32(Parameters);
                if (CredentialId > 0)
                    model = fac.GetThirdPartyAPICredentialsById(CredentialId);
            }
            return View("~/Views/OI/OISetting/AddUpdateThirdPartyOrbCredential.cshtml", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddUpdateThirdPartyOrbCredential(ThirdPartyAPICredentialsEntity model)
        {
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            try
            {
                model.ThirdPartyProvider = ThirdPartyProvider.ORB.ToString();
                string Message = fac.InsertUpdateThirdPartyAPICredentials(model, Helper.oUser.UserId);
                if (string.IsNullOrEmpty(Message))
                {
                    //fac.RefreshThirdPartyAPICredentials(ThirdPartyProvider.ORB.ToString());
                    if (model.CredentialId > 0)
                    {
                        return Json(new { result = true, Message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = true, Message = CommonMessagesLang.msgCommanInsertMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false, Message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteThirdPartyAPICredentials(string Parameters)
        {
            int CredentialId = 0;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            CredentialId = Convert.ToInt32(Parameters);
            // Delete specific user
            string Message = string.Empty;
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            Message = fac.DeleteThirdPartyAPICredentials(CredentialId, Helper.oUser.UserId);
            if (Message == "")
            {
                Message = CommonMessagesLang.msgCommanDeleteMessage;
            }
            return Json(Message);
        }
        #endregion
    }
}