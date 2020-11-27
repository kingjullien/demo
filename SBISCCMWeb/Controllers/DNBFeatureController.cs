using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize,TwoStepVerification, AllowLicense, ValidateInput(true), DandBLicenseEnabled]
    public class DNBFeatureController : BaseController
    {
        // GET: DNBFeature
        [Route("DNB/Feature")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            CommonMethod objCommon = new CommonMethod();
            var objResult = objCommon.LoadCleanseMatchSettings(this.CurrentClient.ApplicationDBConnectionString);
            string IsPauseCleanseMatchEtl = objCommon.GetSettingIDs(objResult, "PAUSE_CLEANSE_MATCH_ETL");
            string IsPauseEnrichmentEtl = objCommon.GetSettingIDs(objResult, "PAUSE_ENRICHMENT_ETL");
            string DATA_IMPORT_DUPLICATE_RESOLUTION = objCommon.GetSettingIDs(objResult, "DATA_IMPORT_DUPLICATE_RESOLUTION");//Ability to remove duplicates from the Active queue MP-466
            string DATA_IMPORT_ERROR_RESOLUTION = objCommon.GetSettingIDs(objResult, "DATA_IMPORT_ERROR_RESOLUTION");// Tune import process to handle bad data import(MP-681)

            //New Process settings for transfer duns enrichment MP-507
            string TRANSFER_DUNS_AUTO_ENRICH = objCommon.GetSettingIDs(objResult, "TRANSFER_DUNS_AUTO_ENRICH");
            string TRANSFER_DUNS_AUTO_ENRICH_TAG = objCommon.GetSettingIDs(objResult, "TRANSFER_DUNS_AUTO_ENRICH_TAG");

            // MP-920 UI changes
            string EnrichmentNbrOfDays = objCommon.GetSettingIDs(objResult, "ENRICHMENT_STALE_NBR_DAYS");
            bool EnrichmentAlwaysRefresh = false;
            if (EnrichmentNbrOfDays == "-1")
            {
                EnrichmentAlwaysRefresh = true;
            }

            DandBFeatureViewModel viewModel = new DandBFeatureViewModel();
            viewModel.PAUSE_CLEANSE_MATCH_ETL = string.IsNullOrEmpty(IsPauseCleanseMatchEtl) ? false : Convert.ToBoolean(IsPauseCleanseMatchEtl);
            viewModel.PAUSE_ENRICHMENT_ETL = string.IsNullOrEmpty(IsPauseEnrichmentEtl) ? false : Convert.ToBoolean(IsPauseEnrichmentEtl);
            viewModel.DATA_IMPORT_DUPLICATE_RESOLUTION = DATA_IMPORT_DUPLICATE_RESOLUTION;
            viewModel.DATA_IMPORT_ERROR_RESOLUTION = DATA_IMPORT_ERROR_RESOLUTION;
            viewModel.TRANSFER_DUNS_AUTO_ENRICH = string.IsNullOrEmpty(TRANSFER_DUNS_AUTO_ENRICH) ? false : Convert.ToBoolean(TRANSFER_DUNS_AUTO_ENRICH);
            viewModel.TRANSFER_DUNS_AUTO_ENRICH_TAG = TRANSFER_DUNS_AUTO_ENRICH_TAG;
            viewModel.ENRICHMENT_STALE_NBR_DAYS = EnrichmentNbrOfDays;
            viewModel.EnrichmentAlwaysRefresh = EnrichmentAlwaysRefresh;

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            ViewBag.SelectedTab = "Feature";
            if (Request.Headers["X-PJAX"] == "true")
            {
                return View("/Views/DNBFeature/IndexFeature.cshtml", viewModel);
            }
            else
            {
                return View("/Views/DandB/Index.cshtml", viewModel);
            }
        }
        public ActionResult IndexFeature()
        {
            CommonMethod objCommon = new CommonMethod();
            var objResult = objCommon.LoadCleanseMatchSettings(this.CurrentClient.ApplicationDBConnectionString);
            string IsPauseCleanseMatchEtl = objCommon.GetSettingIDs(objResult, "PAUSE_CLEANSE_MATCH_ETL");
            string IsPauseEnrichmentEtl = objCommon.GetSettingIDs(objResult, "PAUSE_ENRICHMENT_ETL");
            string DATA_IMPORT_DUPLICATE_RESOLUTION = objCommon.GetSettingIDs(objResult, "DATA_IMPORT_DUPLICATE_RESOLUTION");//Ability to remove duplicates from the Active queue MP-466
            string DATA_IMPORT_ERROR_RESOLUTION = objCommon.GetSettingIDs(objResult, "DATA_IMPORT_ERROR_RESOLUTION");// Tune import process to handle bad data import(MP-681)

            //New Process settings for transfer duns enrichment MP-507
            string TRANSFER_DUNS_AUTO_ENRICH = objCommon.GetSettingIDs(objResult, "TRANSFER_DUNS_AUTO_ENRICH");
            string TRANSFER_DUNS_AUTO_ENRICH_TAG = objCommon.GetSettingIDs(objResult, "TRANSFER_DUNS_AUTO_ENRICH_TAG");

            // MP-920 UI changes
            string EnrichmentNbrOfDays = objCommon.GetSettingIDs(objResult, "ENRICHMENT_STALE_NBR_DAYS");
            bool EnrichmentAlwaysRefresh = false;
            if (EnrichmentNbrOfDays == "-1")
            {
                EnrichmentAlwaysRefresh = true;
            }

            DandBFeatureViewModel viewModel = new DandBFeatureViewModel();
            viewModel.PAUSE_CLEANSE_MATCH_ETL = string.IsNullOrEmpty(IsPauseCleanseMatchEtl) ? false : Convert.ToBoolean(IsPauseCleanseMatchEtl);
            viewModel.PAUSE_ENRICHMENT_ETL = string.IsNullOrEmpty(IsPauseEnrichmentEtl) ? false : Convert.ToBoolean(IsPauseEnrichmentEtl);
            viewModel.DATA_IMPORT_DUPLICATE_RESOLUTION = DATA_IMPORT_DUPLICATE_RESOLUTION;
            viewModel.DATA_IMPORT_ERROR_RESOLUTION = DATA_IMPORT_ERROR_RESOLUTION;
            viewModel.TRANSFER_DUNS_AUTO_ENRICH = string.IsNullOrEmpty(TRANSFER_DUNS_AUTO_ENRICH) ? false : Convert.ToBoolean(TRANSFER_DUNS_AUTO_ENRICH);
            viewModel.TRANSFER_DUNS_AUTO_ENRICH_TAG = TRANSFER_DUNS_AUTO_ENRICH_TAG;
            viewModel.ENRICHMENT_STALE_NBR_DAYS = EnrichmentNbrOfDays;
            viewModel.EnrichmentAlwaysRefresh = EnrichmentAlwaysRefresh;
            return View(viewModel);
        }

        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult IndexDNBFeature(DandBFeatureViewModel viewModel)
        {
            try
            {
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.UpdateProcessSettings("DandBFeature", null, null, null, null, null
                    , Convert.ToString(viewModel.PAUSE_CLEANSE_MATCH_ETL)
                    , Convert.ToString(viewModel.PAUSE_ENRICHMENT_ETL)
                    , viewModel.DATA_IMPORT_DUPLICATE_RESOLUTION
                    , Convert.ToString(viewModel.TRANSFER_DUNS_AUTO_ENRICH)
                    , viewModel.TRANSFER_DUNS_AUTO_ENRICH_TAG
                    , viewModel.DATA_IMPORT_ERROR_RESOLUTION
                    , viewModel.ENRICHMENT_STALE_NBR_DAYS);
                return Json(new { result = true, message = DandBSettingLang.msgProcessSettingUpdate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }



        #region De-Duplicate Data
        // MP-523 Provide ability to de-duplicate data in Match/No-Match and Export queues
        [HttpGet]
        public ActionResult DuplicateData()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryTokenOnAllPosts, RequestFromAjax, RequestFromSameDomain]
        public JsonResult DuplicateData(string Tag = null, string LOBTag = null, string CountryCode = null, int CountryGroupId = 0)
        {
            try
            {
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                string Message = sfac.RemoveDuplicateRecords(Helper.oUser.UserId, Tag, LOBTag, CountryCode, CountryGroupId);
                // D&B - Provide Confirmation Button on De-Duplicate Data Filter (MP-721)
                return Json(new { result = true, message = Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = DandBSettingLang.msgSettingNotUpdate }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Cached Data Settings"
        [HttpGet, RequestFromSameDomain]
        public ActionResult CachedDataSettings()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            // Fill all dropdown and value for setting value.
            model.Settings = fac.GetCleanseMatchSettings();
            CommonMethod.GetSettingIDs(model);

            // GET API call for getting the udated value of MissingDataProvider field
            Clients api = new Clients();
            string ClientCode = model.Settings[model.DATA_STUB_CLIENT_CODE].SettingValue;
            string missingdataprovider = api.GetClientCode(ClientCode);
            if (missingdataprovider.Contains("False"))
            {
                missingdataprovider = "false";
            }
            else if (missingdataprovider.Contains("True"))
            {
                missingdataprovider = "true";
            }

            model.USE_DATA_STUB_VALUE = string.IsNullOrEmpty(model.Settings[model.USE_DATA_STUB].SettingValue) ? false : Convert.ToBoolean(model.Settings[model.USE_DATA_STUB].SettingValue);
            model.USE_DATA_STUB_FOR_ENRICHMENT_VALUE = string.IsNullOrEmpty(model.Settings[model.USE_DATA_STUB_FOR_ENRICHMENT].SettingValue) ? false : Convert.ToBoolean(model.Settings[model.USE_DATA_STUB_FOR_ENRICHMENT].SettingValue);
            if (missingdataprovider == "true" || missingdataprovider == "false")
            {
                model.MISSING_DATA_FROM_PROVIDER = Convert.ToBoolean(missingdataprovider);
            }
            SetMatchGradeContent(model);
            Clear(model);
            return PartialView(model);
        }
        public void SetMatchGradeContent(CleanseMatchSettingsModel model)
        {
            string p1 = "#"; string p2 = "#"; string p3 = "#"; string p4 = "#"; string p5 = "#"; string p6 = "#"; string p7 = "#";

            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_NAME_THRESHOLD].SettingValue))
                p1 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_STREET_NO_THRESHOLD].SettingValue))
                p2 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_STREET_NAME_THRESHOLD].SettingValue))
                p3 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_CITY_THRESHOLD].SettingValue))
                p4 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_STATE_THRESHOLD].SettingValue))
                p5 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_POBOX_THRESHOLD].SettingValue))
                p6 = "A";    //changing the SettingName in the ProcessSettings table to MATCH_GRADE_POBOX_THRESHOLD from MATCH_GRADE_ZIPCODE_THRESHOLD(MP-338)
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_TELEPHONE_THRESHOLD].SettingValue))
                p7 = "A";

            string matchGrade = p1 + p2 + p3 + p4 + p5 + p6 + p7 + "####";
            model.MatchGrade = matchGrade;
        }
        public void Clear(CleanseMatchSettingsModel model)
        {
            //assignee model as empty 
            model.objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
        }

        #region "Delete Cached Cleanse Match"
        public ActionResult DeleteCleanseMatchData(string Parameters)
        {
            string ClientCode = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ClientCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 0);
            }
            ViewBag.ClientCode = ClientCode;
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult DeleteCleanseMatchData(string ClientCode, string APIFamily, DateTime? BeginDateTime, DateTime? EndDateTime)
        {
            try
            {
                Clients api = new Clients();
                string result = api.DeleteCachedCleanseMatch(ClientCode, APIFamily, BeginDateTime, EndDateTime);
                if (string.IsNullOrEmpty(result))
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = true, message = DandBSettingLang.msgCommonDeleteMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region "Delete Cached Enrichment Data"
        public ActionResult DeleteCachedEnrichmentData(string Parameters)
        {
            string ClientCode = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ClientCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 0);
            }
            ViewBag.ClientCode = ClientCode;
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult DeleteCachedEnrichmentData(string ClientCode, string APIType, string DUNSNumber, DateTime? BeginDateTime, DateTime? EndDateTime)
        {
            try
            {
                Clients api = new Clients();
                string result = api.DeleteCachedEnrichment(ClientCode, APIType, DUNSNumber, BeginDateTime, EndDateTime);
                if (string.IsNullOrEmpty(result))
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = true, message = DandBSettingLang.msgCommonDeleteMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Edit GetMissingDataFromProvider
        // Updates client
        [HttpPost]
        // Updates missing data from provider with the help of an API
        public JsonResult UpdateMissingDataProvider(string Parameters)
        {
            string ClientCode = "";
            bool GetMissingDataFromProvider = false;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ClientCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                GetMissingDataFromProvider = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Clients api = new Clients();
                    string Message = api.EditClientsListByClientCode(ClientCode, GetMissingDataFromProvider);
                    if (Message.Contains("Client code not found"))
                    {
                        return Json(new { result = false, message = DandBSettingLang.msgClientCodeNotFound }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = true, message = DandBSettingLang.msgSettingUpdate }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Download Cache Data
        // Downloading the cache data
        public ActionResult CacheDataPopup(string Parameters)
        {
            string ClientCode = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ClientCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 0);
            }
            ViewBag.ClientCode = ClientCode;
            return View();
        }
        // When the cache data values in the popup gets updated
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult CacheDataPopup(string ClientCode, string CountryISOAlpha2Code)
        {
            List<DownloadCacheDataModel> model = new List<DownloadCacheDataModel>();
            if (ModelState.IsValid)
            {
                try
                {
                    string APIFamily = string.Empty;

                    Clients api = new Clients();
                    // API Call for Download Cache Data
                    model = api.DownloadCacheData(ClientCode, CountryISOAlpha2Code, APIFamily);
                    if (model != null)
                    {
                        List<DownloadCacheDataModel> lstResult = new List<DownloadCacheDataModel>();
                        lstResult = model;
                        DataTable data = ToDataTable(lstResult);
                        if (lstResult == null)
                        {
                            data = new DataTable();
                        }
                        string fileName = "CacheData" + DateTime.Now.Ticks.ToString() + ".xlsx";
                        string SheetName = "CacheData";
                        // Exporting the cache data in an excel format
                        byte[] response = CommonExportMethods.ExportExcelFile(data, fileName, SheetName);
                        return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                    else
                    {
                        Session["Message"] = ExportLang.msgNoRecordFound;
                        return RedirectToAction("Index", "DNBFeature");
                    }
                }
                catch (Exception)
                {
                    Session["Message"] = CommonMessagesLang.msgCommanErrorMessage;
                    return RedirectToAction("Index", "DNBFeature");
                }
            }
            Session["Message"] = CommonMessagesLang.msgCommanInsertMessage;
            return RedirectToAction("Index", "DNBFeature");
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        #endregion

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateInput(true)]
        public JsonResult CachedDataSettings(CleanseMatchSettingsModel model)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            CleanseMatchSettingsModel oldmodel = new CleanseMatchSettingsModel();
            oldmodel.Settings = fac.GetCleanseMatchSettings();
            CommonMethod.GetSettingIDs(oldmodel);
            //set Properties of Background Process Settings
            string USE_DATA_STUB = Convert.ToString(model.USE_DATA_STUB_VALUE);
            string USE_DATA_STUB_FOR_ENRICHMENT = Convert.ToString(model.USE_DATA_STUB_FOR_ENRICHMENT_VALUE);
            oldmodel.Settings[oldmodel.USE_DATA_STUB].SettingValue = USE_DATA_STUB;
            oldmodel.Settings[oldmodel.USE_DATA_STUB_FOR_ENRICHMENT].SettingValue = USE_DATA_STUB_FOR_ENRICHMENT;
            //update Cleanse Match Settings
            fac.UpdateCleanseMatchSettings(oldmodel.Settings);
            return Json(DandBSettingLang.msgSettingUpdate);
        }

        #endregion
    }
}