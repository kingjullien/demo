using Newtonsoft.Json;
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
    [Authorize,TwoStepVerification, AllowLicense, ValidateInput(true), DandBLicenseEnabled]
    public class DNBLicenceController : BaseController
    {
        // GET: DNBLicence
        public ActionResult Index()
        {
            return View();
        }

        #region "D&B License Keys"
        [HttpGet]
        [Route("DNB/License")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult DnBLicenseCredential()
        {
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstThirdPartyAPICredentials = fac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            SessionHelper.ThirdPartyAPI = JsonConvert.SerializeObject(lstThirdPartyAPICredentials);

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return View(lstThirdPartyAPICredentials);
            else
            {
                ViewBag.SelectedTab = "License";
                return View("~/Views/DandB/Index.cshtml", lstThirdPartyAPICredentials);
            }
        }
        public ActionResult GetDandBCredentials()
        {
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstThirdPartyAPICredentials = fac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            return View("~/Views/DNBLicence/DnBLicenseCredential.cshtml", lstThirdPartyAPICredentials);
        }

        public ActionResult AddUpdateThirdPartyDandBCredential(string Parameters)
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
            return View(model);
        }

        [HttpPost, RequestFromSameDomain, ValidateInput(true), ValidateAntiForgeryToken]
        public ActionResult AddUpdateThirdPartyDandBCredential(ThirdPartyAPICredentialsEntity model)
        {
            ModelState.Remove("Tag");
            try
            {
                if (ModelState.IsValid)
                {
                    ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                    model.ThirdPartyProvider = ThirdPartyProvider.DNB.ToString();
                    string Message = fac.InsertUpdateThirdPartyAPICredentials(model, Helper.oUser.UserId);
                    if (string.IsNullOrEmpty(Message))
                    {
                        fac.RefreshThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
                        CommonMethod.GetThirdPartyAPICredentials(this.CurrentClient.ApplicationDBConnectionString);
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
                else
                {
                    return Json(new { result = false, Message = CommonMessagesLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
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
                CredentialId = Convert.ToInt32(Parameters);

                // Delete specific Third Party API Credentials
                string Message = string.Empty;
                ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                Message = fac.DeleteThirdPartyAPICredentials(CredentialId, Helper.oUser.UserId);
                if (string.IsNullOrEmpty(Message))
                {
                    return Json(new { result = true, Message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, Message = Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, Message = CommonMessagesLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult SetEntitleMentsForCreds(string Parameters)
        {
            EntitlementsViewModel viewModel = new EntitlementsViewModel();
            int CredentialId = 0;
            string apiType = string.Empty;
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                CredentialId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                apiType = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
            }
            List<DnbAPIEntity> lstAPIs = fac.GetDnBAPIList(apiType, CredentialId);
            viewModel.CredentialId = CredentialId;
            viewModel.APIType = apiType;
            viewModel.lstAPIIds = string.Join(",", lstAPIs.Select(x => x.DnBAPIId).ToList());
            return View(viewModel);
        }

        [HttpPost, RequestFromSameDomain]
        public ActionResult SetEntitleMentsForCreds(EntitlementsViewModel entitlements)
        {
            try
            {
                ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.UpsertDnBAPIEntitlements(entitlements.CredentialId, entitlements.DnBAPIId);
                return Json(new { result = true, Message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, Message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region "Ux Default Credentials"
        [HttpGet]
        public ActionResult DefaultInteractiveKeys()
        {
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            ThirdPartyAPIEntity model = new ThirdPartyAPIEntity();
            List<UXDefaultCredentialsEntity> uxCreds = fac.GetUXDefaultCredentials();
            List<ThirdPartyAPIForEnrichmentEntity> uxEnrichCreds = new List<ThirdPartyAPIForEnrichmentEntity>();
            uxEnrichCreds = fac.GetUXDefaultUXEnrichments();
            ThirdPartyAPIForEnrichmentEntity enrichModel = uxEnrichCreds.FirstOrDefault(x => x.EnrichmentType == "OWNERSHIP");

            model.DNB_BUILD_A_LIST = Convert.ToString(uxCreds.FirstOrDefault(x => x.Code == "DNB_BUILD_A_LIST")?.CredentialId);
            model.DNB_INVESTIGATIONS = Convert.ToString(uxCreds.FirstOrDefault(x => x.Code == "DNB_INVESTIGATIONS")?.CredentialId);
            model.DNB_SINGLE_ENTITY_SEARCH = Convert.ToString(uxCreds.FirstOrDefault(x => x.Code == "DNB_SINGLE_ENTITY_SEARCH")?.CredentialId);
            model.DNB_TYPEAHEAD_SEARCH = Convert.ToString(uxCreds.FirstOrDefault(x => x.Code == "DNB_TYPEAHEAD_SEARCH")?.CredentialId);
            model.GOOGLE = Convert.ToString(uxCreds.FirstOrDefault(x => x.Code == "GOOGLE")?.CredentialId);
            model.DESCARTES = Convert.ToString(uxCreds.FirstOrDefault(x => x.Code == "DESCARTES")?.CredentialId);
            model.OwnershipCredId = uxEnrichCreds.FirstOrDefault(x => x.EnrichmentType == "OWNERSHIP").CredentialId;
            model.OwnershipDnBAPIId = uxEnrichCreds.FirstOrDefault(x => x.EnrichmentType == "OWNERSHIP").DnBAPIId;
            model.TypeOWNERSHIP= uxEnrichCreds.FirstOrDefault(x => x.EnrichmentType == "OWNERSHIP").EnrichmentType;
            return View(model);
        }
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult SetDefaultInteractiveKeys(ThirdPartyAPIEntity model)
        {
            try
            {
                List<string> lstCodes = new List<string>();
                List<string> credIds = new List<string>();
                ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                if (Helper.LicenseBuildAList)
                {
                    lstCodes.Add("DNB_BUILD_A_LIST");
                    credIds.Add(model.DNB_BUILD_A_LIST);
                }

                if (Helper.LicenseEnableInvestigations)
                {
                    lstCodes.Add("DNB_INVESTIGATIONS");
                    credIds.Add(model.DNB_INVESTIGATIONS);
                }

                lstCodes.Add("DNB_SINGLE_ENTITY_SEARCH");
                credIds.Add(model.DNB_SINGLE_ENTITY_SEARCH);

                lstCodes.Add("DNB_TYPEAHEAD_SEARCH");
                credIds.Add(model.DNB_TYPEAHEAD_SEARCH);

                lstCodes.Add("GOOGLE");
                credIds.Add(model.GOOGLE);

                lstCodes.Add("DESCARTES");
                credIds.Add(model.DESCARTES);

                fac.UpdateUXDefaultCredentials(string.Join(",", lstCodes), string.Join(",", credIds), Helper.oUser.UserId);
                if (Helper.LicenseEnableCompliance)
                {
                    //Update Ownership creds
                    fac.UpdateUXDefaultCredentialsForEnrichment(model.TypeOWNERSHIP, model.OwnershipDnBAPIId, model.OwnershipCredId);
                    CommonMethod.GetUXDefaultUXEnrichments(this.CurrentClient.ApplicationDBConnectionString);
                }

                //update session for UX default Credentials(for Auth tokens)
                CommonMethod.GetThirdPartyAPICredentials(this.CurrentClient.ApplicationDBConnectionString);
                return Json(new { result = true, Message = DandBSettingLang.msgSettingUpdate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = CommonMessagesLang.msgSomethingWrong }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Ux Default Credentials ForEnrichment"
        [HttpGet]
        public ActionResult DefaultKeysForEnrichment()
        {
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            ThirdPartyAPIForEnrichmentEntity model = new ThirdPartyAPIForEnrichmentEntity();

            List<ThirdPartyAPIForEnrichmentEntity> uxCreds = new List<ThirdPartyAPIForEnrichmentEntity>();
            uxCreds = fac.GetUXDefaultUXEnrichments();
            model.EnrichmentType = uxCreds.FirstOrDefault(x => x.EnrichmentType == "FIRMOGRAPHICS").EnrichmentType;
            model.CredentialId = uxCreds.FirstOrDefault(x => x.EnrichmentType == "FIRMOGRAPHICS").CredentialId;
            model.DnBAPIId = uxCreds.FirstOrDefault(x => x.EnrichmentType == "FIRMOGRAPHICS").DnBAPIId;

            List<ThirdPartyAPICredentialsEntity> lst = JsonConvert.DeserializeObject<List<ThirdPartyAPICredentialsEntity>>(SessionHelper.ThirdPartyAPI);
            model.APIType = lst.FirstOrDefault(x => x.CredentialId == model.CredentialId).APIType;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult SetDefaultKeysForEnrichment(ThirdPartyAPIForEnrichmentEntity model)
        {
            try
            {
                ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.UpdateUXDefaultCredentialsForEnrichment(model.EnrichmentType, model.DnBAPIId, model.CredentialId);
                return Json(new { result = true, Message = DandBSettingLang.msgSettingUpdate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, Message = CommonMessagesLang.msgSomethingWrong }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetThirdPartyAPIType(string newAPIValue)
        {
            SelectList lst = CommonDropdown.GetAPITypeForThirdPartyAPICredentialsForEnrichment(this.CurrentClient.ApplicationDBConnectionString, newAPIValue);
            return new JsonResult { Data = lst.Items };
        }
        #endregion

        #region "Background Process Settings"
        [HttpGet]
        public ActionResult BackgroundProcessSettings()
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            // Fill all dropdown and value for setting value.
            List<SettingEntity> Settings = fac.GetCleanseMatchSettings();
            BackgroundProcessSettingsViewModal viewModal = new BackgroundProcessSettingsViewModal();
            if (Settings != null && Settings.Any())
            {
                CommonMethod objCommon = new CommonMethod();
                var objResult = objCommon.LoadCleanseMatchSettings(this.CurrentClient.ApplicationDBConnectionString);
                viewModal.MAX_PARALLEL_THREAD = objCommon.GetSettingIDs(objResult, "MAX_PARALLEL_THREAD");
                viewModal.BATCH_SIZE = objCommon.GetSettingIDs(objResult, "BATCH_SIZE");
                viewModal.WAIT_TIME_BETWEEN_BATCHES_SECS = objCommon.GetSettingIDs(objResult, "WAIT_TIME_BETWEEN_BATCHES_SECS");
            }
            return PartialView(viewModal);
        }
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain, ValidateInput(true), AllowLicenseType]
        public JsonResult BackgroundProcessSettings(BackgroundProcessSettingsViewModal model)
        {
            try
            {
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                CleanseMatchSettingsModel oldmodel = new CleanseMatchSettingsModel();
                oldmodel.Settings = fac.GetCleanseMatchSettings();
                CommonMethod.GetSettingIDs(oldmodel);
                if (CommonMethod.IsDigitsOnly(model.MAX_PARALLEL_THREAD) && CommonMethod.IsDigitsOnly(model.BATCH_SIZE) && CommonMethod.IsDigitsOnly(model.WAIT_TIME_BETWEEN_BATCHES_SECS))
                {
                    oldmodel.Settings[oldmodel.MAX_PARALLEL_THREAD].SettingValue = model.MAX_PARALLEL_THREAD;
                    oldmodel.Settings[oldmodel.BATCH_SIZE].SettingValue = model.BATCH_SIZE;
                    oldmodel.Settings[oldmodel.WAIT_TIME_BETWEEN_BATCHES_SECS].SettingValue = model.WAIT_TIME_BETWEEN_BATCHES_SECS;
                    //update Cleanse Match Settings
                    fac.UpdateCleanseMatchSettings(oldmodel.Settings);
                    return Json(new { result = true, Message = DandBSettingLang.msgSettingUpdate }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, Message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, Message = CommonMessagesLang.msgSomethingWrong }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}