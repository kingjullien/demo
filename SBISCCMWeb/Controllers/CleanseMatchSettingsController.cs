using SBISCCMWeb.Models;
using SBISCompanyCleanseMatchFacade.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Data;
using SBISCCMWeb.Utility;
using Microsoft.AspNet.Identity;
using PagedList;
using System.IO;
using OfficeOpenXml;
using SBISCCMWeb.Controllers;
using System.Data.SqlClient;

namespace SBISCCMWeb.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR"), TwoStepVerification]
    public class CleanseMatchSettingsController : BaseController
    {
        #region "Initialization"

        public const string AnyGrade = "#", AnyCode = "##";
        #endregion

        public ActionResult Index(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);

            #region set properties of CleanseMatchExclusions

            CleanseMatchExclusionsFacade CMEfac = new CleanseMatchExclusionsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<CleanseMatchExclusions> list = CMEfac.GetAllCleanseMatchExclusions();

            CleanseMatchExclusionsEntity oCleanseMatchExclusionsEntity = new CleanseMatchExclusionsEntity();
            //set properties of CleanseMatchExclusions
            oCleanseMatchExclusionsEntity.CleanseMatchExclusionId1 = list[0].CleanseMatchExclusionId;
            oCleanseMatchExclusionsEntity.CleanseMatchExclusionId2 = list[1].CleanseMatchExclusionId;
            oCleanseMatchExclusionsEntity.CleanseMatchExclusionId3 = list[2].CleanseMatchExclusionId;
            oCleanseMatchExclusionsEntity.CleanseMatchExclusionId4 = list[3].CleanseMatchExclusionId;
            oCleanseMatchExclusionsEntity.CleanseMatchExclusionId5 = list[4].CleanseMatchExclusionId;
            oCleanseMatchExclusionsEntity.Exclusion1 = list[0].Exclusion;
            oCleanseMatchExclusionsEntity.Exclusion2 = list[1].Exclusion;
            oCleanseMatchExclusionsEntity.Exclusion3 = list[2].Exclusion;
            oCleanseMatchExclusionsEntity.Exclusion4 = list[3].Exclusion;
            oCleanseMatchExclusionsEntity.Exclusion5 = list[4].Exclusion;
            oCleanseMatchExclusionsEntity.Exclusion_DP1 = list[0].Exclusion;
            oCleanseMatchExclusionsEntity.Exclusion_DP2 = list[1].Exclusion;
            oCleanseMatchExclusionsEntity.Exclusion_DP3 = list[2].Exclusion;
            oCleanseMatchExclusionsEntity.Exclusion_DP4 = list[3].Exclusion;
            oCleanseMatchExclusionsEntity.Exclusion_DP5 = list[4].Exclusion;
            oCleanseMatchExclusionsEntity.Active1 = list[0].Active;
            oCleanseMatchExclusionsEntity.Active2 = list[1].Active;
            oCleanseMatchExclusionsEntity.Active3 = list[2].Active;
            oCleanseMatchExclusionsEntity.Active4 = list[3].Active;
            oCleanseMatchExclusionsEntity.Active5 = list[4].Active;
            oCleanseMatchExclusionsEntity.Tags1 = list[0].Tags;
            oCleanseMatchExclusionsEntity.Tags2 = list[1].Tags;
            oCleanseMatchExclusionsEntity.Tags3 = list[2].Tags;
            oCleanseMatchExclusionsEntity.Tags4 = list[3].Tags;
            oCleanseMatchExclusionsEntity.Tags5 = list[4].Tags;

            model.oCleanseMatchExclusionsEntity = new CleanseMatchExclusionsEntity();
            model.oCleanseMatchExclusionsEntity = oCleanseMatchExclusionsEntity;
            #endregion

            #region set properties of AutoAcceptanceDirective
            AutoAcceptanceDirectivesFacade AADfac = new AutoAcceptanceDirectivesFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<AutoAcceptanceDirectives> listdirectives = AADfac.GetAllAutoAcceptanceDirectives();
            AutoAcceptanceDirectivesEntity oAutoAcceptanceDirectivesEntity = new AutoAcceptanceDirectivesEntity();
            //set properties of AutoAcceptanceDirective
            oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId1 = listdirectives[0].AutoAcceptanceDirectiveId;
            oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId2 = listdirectives[1].AutoAcceptanceDirectiveId;
            oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId3 = listdirectives[2].AutoAcceptanceDirectiveId;
            oAutoAcceptanceDirectivesEntity.Directive1 = listdirectives[0].Directive;
            oAutoAcceptanceDirectivesEntity.Directive2 = listdirectives[1].Directive;
            oAutoAcceptanceDirectivesEntity.Directive3 = listdirectives[2].Directive;
            oAutoAcceptanceDirectivesEntity.Active1 = listdirectives[0].Active;
            oAutoAcceptanceDirectivesEntity.Active2 = listdirectives[1].Active;
            oAutoAcceptanceDirectivesEntity.Active3 = listdirectives[2].Active;
            oAutoAcceptanceDirectivesEntity.Tags1 = listdirectives[0].Tags;
            oAutoAcceptanceDirectivesEntity.Tags2 = listdirectives[1].Tags;
            oAutoAcceptanceDirectivesEntity.Tags3 = listdirectives[2].Tags;

            model.oAutoAcceptanceDirectivesEntity = new AutoAcceptanceDirectivesEntity();
            model.oAutoAcceptanceDirectivesEntity = oAutoAcceptanceDirectivesEntity;
            #endregion


            #region Auto-Acceptance 
            //Auto - Acceptance Pagination
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;

            #region Set Viewbag

            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            TempData["pageno"] = currentPageIndex;
            TempData["pagevalue"] = pageSize;
            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);

            //Load AutoAcceptance Criteria.
            model.objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
            #endregion

            if (TempData["ApiLayer"] != null)
                ViewBag.ApiLayer = TempData["ApiLayer"];
            else
                ViewBag.ApiLayer = fac.GetCleanseMatchSettings().Where(x => x.SettingName == "API_LAYER").FirstOrDefault().SettingValue;

            ViewBag.Message = TempData["Message"];

            #endregion
            return View(model);
        }
        // Update Cleans Match setting 
        public ActionResult UpdateCleanseMatchSettings(CleanseMatchSettingsModel model)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.UpdateCleanseMatchSettings(model.Settings);// Update Clean Bach Setting Value
            string message = MessageCollection.SettingUpdate;
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #region "Additional Acceptance Criteria"

        public void Clear(CleanseMatchSettingsModel model)
        {
            //assignee model as empty 
            model.objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
        }

        private void SetDefaultValue(AutoAdditionalAcceptanceCriteriaEntity model)
        {
            //set Default value of AutoAdditionalAcceptanceCriteria
            model.CompanyGrade = AnyGrade;
            model.CompanyCode = AnyCode;
            model.StreetGrade = AnyGrade;
            model.StreetCode = AnyCode;
            model.StreetNameGrade = AnyGrade;
            model.StreetNameCode = AnyCode;
            model.CityGrade = AnyGrade;
            model.CityCode = AnyCode;
            model.StateGrade = AnyGrade;
            model.StateCode = AnyCode;
            model.AddressGrade = AnyGrade;
            model.AddressCode = AnyCode;
            model.PhoneGrade = AnyGrade;
            model.PhoneCode = AnyCode;
            model.ZipGrade = AnyGrade;
            model.Density = AnyGrade;
            model.Uniqueness = AnyGrade;
            model.SIC = AnyGrade;
        }

        public bool Validate(AutoAdditionalAcceptanceCriteriaEntity model)
        {
            return model.IsValidSave;
        }

        public List<CountryGroupEntity> LoadCountryGroupEntity(string ConnectionString)
        {
            // Load Country Group Entity
            SettingFacade fac = new SettingFacade(ConnectionString);
            return fac.GetCountryGroup();
        }

        public List<MatchGradeEntity> LoadMatchGrades(string ConnectionString)
        {
            // Load Match Entity
            SettingFacade fac = new SettingFacade(ConnectionString);
            return fac.GetMatchGrades();
        }

       

        public List<MatchEntity> LoadTopMatchGrades(string ConnectionString)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            DataTable dtResult = fac.GetTopMatchGradeSettings(true);
            List<MatchEntity> lstMatches = new List<MatchEntity>();
            if (dtResult.Rows.Count > 0)
            {
                // Get Match Grade and Confidence Code
                MatchEntity objMatch = new MatchEntity();
                objMatch.DnBConfidenceCode = Convert.ToInt32(0);
                objMatch.DnBMatchGradeText = Convert.ToString("Select Match Grade");
                lstMatches.Add(objMatch);
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    objMatch = new MatchEntity();
                    objMatch.DnBConfidenceCode = Convert.ToInt32(dtResult.Rows[i]["DnBConfidenceCode"]);
                    objMatch.DnBMatchGradeText = Convert.ToString(dtResult.Rows[i]["DnBMatchGradeText"]);
                    lstMatches.Add(objMatch);
                }
            }
            return lstMatches;
        }

        private void GetSettingIDs(CleanseMatchSettingsModel CleanseMatchSettingsModel)
        {
            //Get Cleanse Match Settings
            for (int i = 0; i < CleanseMatchSettingsModel.Settings.Count; i++)
            {
                string settingname = CleanseMatchSettingsModel.Settings[i].SettingName;
                switch (settingname)
                {
                    case "DNB_USERNAME":
                        CleanseMatchSettingsModel.DNB_USERNAME = i; break;
                    case "DNB_PASSWORD":
                        CleanseMatchSettingsModel.DNB_PASSWORD = i; break;
                    case "MIN_CONFIDENCE_CODE":
                        CleanseMatchSettingsModel.MIN_CONFIDENCE_CODE = i; break;
                    case "AUTO_CORRECTION_THRESHOLD":
                        CleanseMatchSettingsModel.AUTO_CORRECTION_THRESHOLD = i; break;
                    case "MAX_CANDIDATE_QTY":
                        CleanseMatchSettingsModel.MAX_CANDIDATE_QTY = i; break;
                    case "MAX_PARALLEL_THREAD":
                        CleanseMatchSettingsModel.MAX_PARALLEL_THREAD = i; break;
                    case "MATCH_GRADE_NAME_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_NAME_THRESHOLD = i; break;
                    case "MATCH_GRADE_STREET_NO_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_STREET_NO_THRESHOLD = i; break;
                    case "MATCH_GRADE_STREET_NAME_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_STREET_NAME_THRESHOLD = i; break;
                    case "MATCH_GRADE_CITY_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_CITY_THRESHOLD = i; break;
                    case "MATCH_GRADE_STATE_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_STATE_THRESHOLD = i; break;
                    case "MATCH_GRADE_TELEPHONE_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_TELEPHONE_THRESHOLD = i; break;
                    case "MATCH_GRADE_ZIPCODE_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_ZIPCODE_THRESHOLD = i; break;
                    case "APPLY_MATCH_GRADE_TO_LCM":
                        CleanseMatchSettingsModel.APPLY_MATCH_GRADE_TO_LCM = i; break;
                    case "PROXY_SERVER":
                        CleanseMatchSettingsModel.PROXY_SERVER = i; break;
                    case "PROXY_USERNAME":
                        CleanseMatchSettingsModel.PROXY_USERNAME = i; break;
                    case "PROXY_PASSWORD":
                        CleanseMatchSettingsModel.PROXY_PASSWORD = i; break;
                    case "PROXY_DOMAIN":
                        CleanseMatchSettingsModel.PROXY_DOMAIN = i; break;
                    case "BATCH_SIZE":
                        CleanseMatchSettingsModel.BATCH_SIZE = i; break;
                    case "WAIT_TIME_BETWEEN_BATCHES_SECS":
                        CleanseMatchSettingsModel.WAIT_TIME_BETWEEN_BATCHES_SECS = i; break;
                    case "DNB_PLUS_KEY":
                        CleanseMatchSettingsModel.DNB_PLUS_KEY = i; break;
                    case "DNB_PLUS_SECRET":
                        CleanseMatchSettingsModel.DNB_PLUS_SECRET = i; break;
                    case "API_LAYER":
                        CleanseMatchSettingsModel.API_LAYER = i; break;
                    default:
                        break;
                }
            }
        }

        public void SetMatchGradeContent(CleanseMatchSettingsModel model)
        {
            string p1 = "#"; string p2 = "#"; string p3 = "#"; string p4 = "#"; string p5 = "#"; string p7 = "#"; string p8 = "#";

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
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_TELEPHONE_THRESHOLD].SettingValue))
                p7 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_ZIPCODE_THRESHOLD].SettingValue))
                p8 = "A";

            string matchGrade = p1 + p2 + p3 + p4 + p5 + "#" + p7 + p8 + "###";
            model.MatchGrade = matchGrade;

        }

        public void LoadAdditionalAcceptanceDeatils(CleanseMatchSettingsModel model)
        {
            //Load AutoAcceptance Criteria.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            model.AutoAcceptanceCriteria = fac.GetAutoAcceptanceDetails();
            ObservableCollection<AutoAdditionalAcceptanceCriteriaEntity> myCollection = new ObservableCollection<AutoAdditionalAcceptanceCriteriaEntity>(model.AutoAcceptanceCriteria);
            model.objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
        }

        public List<MatchCodeEntity> LoadMatchGradesEntities(string ddlMatchGrade, string ConnectionString)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            switch (ddlMatchGrade)
            {
                case "Company":
                    return fac.GetMatchMDPCodes("Company");
                case "StreetName":
                    return fac.GetMatchMDPCodes("StreetName");
                case "StreetNo":
                    return fac.GetMatchMDPCodes("StreetNo");
                case "City":
                    return fac.GetMatchMDPCodes("City");
                case "State":
                    return fac.GetMatchMDPCodes("State"); ;
                case "MailingAddress":
                    return fac.GetMatchMDPCodes("MailingAddress");
                case "Phone":
                    return fac.GetMatchMDPCodes("Phone");


                default:
                    return fac.GetMatchMDPCodes("");
            }
        }
        #endregion

        #region CleanseMatchExclusions
        [RequestFromSameDomain]
        public void CleanseMatchExclusions(string ExcludeNonHeadQuarters, string Tags1, string ExcludeNonMarketable, string Tags2, string ExcludeOutofBusiness, string Tags3, string ExcludeUndeliverable, string Tags4, string ExcludeUnreachable, string Tags5)
        {
            //set properties of Cleanse Match Exclusions
            CleanseMatchExclusionsEntity obj = new CleanseMatchExclusionsEntity();
            obj.CleanseMatchExclusionId1 = 1;
            obj.Active1 = Convert.ToBoolean(ExcludeNonHeadQuarters);
            obj.Tags1 = Tags1;
            obj.CleanseMatchExclusionId2 = 2;
            obj.Active2 = Convert.ToBoolean(ExcludeNonMarketable);
            obj.Tags2 = Tags2;
            obj.CleanseMatchExclusionId3 = 3;
            obj.Active3 = Convert.ToBoolean(ExcludeOutofBusiness);
            obj.Tags3 = Tags3;

            obj.CleanseMatchExclusionId4 = 4;
            obj.Active4 = Convert.ToBoolean(ExcludeUndeliverable);
            obj.Tags4 = Tags4;

            obj.CleanseMatchExclusionId5 = 5;
            obj.Active5 = Convert.ToBoolean(ExcludeUnreachable);
            obj.Tags5 = Tags5;
            //update CleanseMatchExclusions Setting
            CleanseMatchExclusionsFacade CMEfac = new CleanseMatchExclusionsFacade(this.CurrentClient.ApplicationDBConnectionString);
            CMEfac.UpdateCleanseMatchExclusions(obj);
        }
        #endregion

        #region AutoAcceptanceDirectives
        [RequestFromSameDomain]
        public void AutoAcceptanceDirectives(string AcceptActiveRecordsOnly, string Tags1, string PreferHeadquartersRecord, string Tags2, string AcceptHeadquartersRecordOnly, string Tags3)
        {
            //set properties of Auto-Acceptance Directives
            AutoAcceptanceDirectivesEntity obj = new AutoAcceptanceDirectivesEntity();
            obj.AutoAcceptanceDirectiveId1 = 1;
            obj.Active1 = Convert.ToBoolean(AcceptActiveRecordsOnly);
            obj.Tags1 = Tags1;
            obj.AutoAcceptanceDirectiveId2 = 2;
            obj.Active2 = Convert.ToBoolean(PreferHeadquartersRecord);
            obj.Tags2 = Tags2;
            obj.AutoAcceptanceDirectiveId3 = 3;
            obj.Active3 = Convert.ToBoolean(AcceptHeadquartersRecordOnly);
            obj.Tags3 = Tags3;
            //update AutoAcceptanceDirectives Setting
            AutoAcceptanceDirectivesFacade AADfac = new AutoAcceptanceDirectivesFacade(this.CurrentClient.ApplicationDBConnectionString);
            AADfac.UpdateAutoAcceptanceDirectives(obj);
        }
        #endregion

        #region D&B Direct 2.0 License 
        [HttpGet]
        public ActionResult DnBDirectLicense()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            // Fill all dropdown and value for setting value.
            model.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(model);
            SetMatchGradeContent(model);
            Clear(model);
            ViewBag.ApiLayer = model.Settings.Where(x => x.SettingName == "API_LAYER").FirstOrDefault().SettingValue;
            return PartialView(model);
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult DnBDirectLicense(CleanseMatchSettingsModel model, string btnSubmit = null)
        {

            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            CleanseMatchSettingsModel oldmodel = new CleanseMatchSettingsModel();
            oldmodel.Settings = fac.GetCleanseMatchSettings();

            GetSettingIDs(oldmodel);
            try
            {
                model.Settings[oldmodel.DNB_PASSWORD].SettingValue = SBISCCMWeb.Utility.Utility.GetDecryptedString(model.Settings[model.DNB_PASSWORD].SettingValue);
            }
            catch
            {
                model.Settings[oldmodel.DNB_PASSWORD].SettingValue = model.Settings[oldmodel.DNB_PASSWORD].SettingValue;
            }
            oldmodel.Settings[oldmodel.DNB_USERNAME].SettingValue = model.Settings[oldmodel.DNB_USERNAME].SettingValue;
            oldmodel.Settings[oldmodel.DNB_PASSWORD].SettingValue = model.Settings[oldmodel.DNB_PASSWORD].SettingValue;
            oldmodel.Settings[oldmodel.API_LAYER].SettingValue = ApiLayerType.Direct20.ToString();
            //check DnB Credentials is validate or not.
            string Message = fac.ValidateDnBLogin(oldmodel.Settings[oldmodel.DNB_USERNAME].SettingValue, oldmodel.Settings[oldmodel.DNB_PASSWORD].SettingValue);
            if (Message.ToLower() == MessageCollection.ValidCredential)
            {
                RestfulHttpClient<AuthResponse, string> authClient = new RestfulHttpClient<AuthResponse, string>(Helper.AuthURL, string.Empty, oldmodel.Settings[oldmodel.DNB_USERNAME].SettingValue, oldmodel.Settings[oldmodel.DNB_PASSWORD].SettingValue, Helper.JsonMediaType);
                var responseData = authClient.GetAuthResponse();
                if (responseData != null)
                {
                    if (responseData.TransactionResult.ResultID != "CM000")
                    {
                        Helper.ApiToken = null;
                    }
                    else
                    {
                        Helper.ApiToken = responseData.AuthenticationDetail.Token;
                    }
                }
                else
                {
                    Helper.ApiToken = null;
                }
                //update DnBDirectLicense Credentials
                fac.UpdateCleanseMatchSettings(oldmodel.Settings);
                fac.UpdateCleanseMatchSettingsAPIFamily(ApiLayerType.Direct20.ToString());
                Message = MessageCollection.SettingUpdate;
            }


            model.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(model);
            SetMatchGradeContent(model);
            Clear(model);
            TempData["ApiLayer"] = ApiLayerType.Direct20.ToString();
            Helper.APILAYER = ApiLayerType.Direct20.ToString();
            string Data = Message;
            return Json(Data);
        }
        #endregion

        #region Global Match Auto-Acceptance Settings 
        [HttpGet]
        public ActionResult GlobalMatchAutoAcceptanceSettings()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            // Fill all dropdown and value for setting value.
            model.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(model);
            SetMatchGradeContent(model);
            Clear(model);
            model.boolApplyMatchGradeLCM = Convert.ToBoolean(model.Settings[model.APPLY_MATCH_GRADE_TO_LCM].SettingValue);
            model.boolBusinessName = Convert.ToBoolean(model.Settings[model.MATCH_GRADE_NAME_THRESHOLD].SettingValue);
            model.boolCity = Convert.ToBoolean(model.Settings[model.MATCH_GRADE_CITY_THRESHOLD].SettingValue);
            model.boolState = Convert.ToBoolean(model.Settings[model.MATCH_GRADE_STATE_THRESHOLD].SettingValue);
            model.boolStreet = Convert.ToBoolean(model.Settings[model.MATCH_GRADE_STREET_NO_THRESHOLD].SettingValue);
            model.boolStreetName = Convert.ToBoolean(model.Settings[model.MATCH_GRADE_STREET_NAME_THRESHOLD].SettingValue);
            model.boolTelephone = Convert.ToBoolean(model.Settings[model.MATCH_GRADE_TELEPHONE_THRESHOLD].SettingValue);
            model.boolZipCode = Convert.ToBoolean(model.Settings[model.MATCH_GRADE_ZIPCODE_THRESHOLD].SettingValue);
            model.AUTO_CORRECTION_THRESHOLD = Convert.ToInt32(model.Settings[model.AUTO_CORRECTION_THRESHOLD].SettingValue);

            ViewBag.Message = TempData["Message"];
            return PartialView(model);
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult GlobalMatchAutoAcceptanceSettings(CleanseMatchSettingsModel model)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            CleanseMatchSettingsModel oldmodel = new CleanseMatchSettingsModel();
            oldmodel.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(oldmodel);
            //set Properties of Global Match Auto-Acceptance Settings
            oldmodel.Settings[oldmodel.AUTO_CORRECTION_THRESHOLD].SettingValue = model.AUTO_CORRECTION_THRESHOLD.ToString();
            oldmodel.Settings[oldmodel.APPLY_MATCH_GRADE_TO_LCM].SettingValue = model.boolApplyMatchGradeLCM.ToString();
            oldmodel.Settings[oldmodel.MATCH_GRADE_NAME_THRESHOLD].SettingValue = model.boolBusinessName.ToString();
            oldmodel.Settings[oldmodel.MATCH_GRADE_CITY_THRESHOLD].SettingValue = model.boolCity.ToString();
            oldmodel.Settings[oldmodel.MATCH_GRADE_STATE_THRESHOLD].SettingValue = model.boolState.ToString();
            oldmodel.Settings[oldmodel.MATCH_GRADE_STREET_NO_THRESHOLD].SettingValue = model.boolStreet.ToString();
            oldmodel.Settings[oldmodel.MATCH_GRADE_STREET_NAME_THRESHOLD].SettingValue = model.boolStreetName.ToString();
            oldmodel.Settings[oldmodel.MATCH_GRADE_TELEPHONE_THRESHOLD].SettingValue = model.boolTelephone.ToString();
            oldmodel.Settings[oldmodel.MATCH_GRADE_ZIPCODE_THRESHOLD].SettingValue = model.boolZipCode.ToString();
            //update Global Match Auto-Acceptance Settings
            fac.UpdateCleanseMatchSettings(oldmodel.Settings);
            model.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(model);
            SetMatchGradeContent(model);
            Clear(model);
            return Json(MessageCollection.SettingUpdate);
        }
        #endregion

        #region Background Process Settings 
        [HttpGet]
        public ActionResult BackgroundProcessSettings()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            // Fill all dropdown and value for setting value.
            model.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(model);
            SetMatchGradeContent(model);
            Clear(model);
            return PartialView(model);
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult BackgroundProcessSettings(CleanseMatchSettingsModel model)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            CleanseMatchSettingsModel oldmodel = new CleanseMatchSettingsModel();
            oldmodel.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(oldmodel);
            //set Properties of Background Process Settings
            oldmodel.Settings[oldmodel.MIN_CONFIDENCE_CODE].SettingValue = model.Settings[oldmodel.MIN_CONFIDENCE_CODE].SettingValue;
            oldmodel.Settings[oldmodel.MAX_CANDIDATE_QTY].SettingValue = model.Settings[oldmodel.MAX_CANDIDATE_QTY].SettingValue;
            oldmodel.Settings[oldmodel.MAX_PARALLEL_THREAD].SettingValue = model.Settings[oldmodel.MAX_PARALLEL_THREAD].SettingValue;
            oldmodel.Settings[oldmodel.BATCH_SIZE].SettingValue = model.Settings[oldmodel.BATCH_SIZE].SettingValue;
            oldmodel.Settings[oldmodel.WAIT_TIME_BETWEEN_BATCHES_SECS].SettingValue = model.Settings[oldmodel.WAIT_TIME_BETWEEN_BATCHES_SECS].SettingValue;
            //update Cleanse Match Settings
            fac.UpdateCleanseMatchSettings(oldmodel.Settings);
            model.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(model);
            SetMatchGradeContent(model);
            Clear(model);
            return Json(MessageCollection.SettingUpdate);
        }
        #endregion

        #region Direct Plus APICredentials
        [HttpGet]
        public ActionResult DirectPlusAPICredentials()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            // Fill all dropdown and value for setting value.
            model.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(model);
            SetMatchGradeContent(model);
            Clear(model);
            ViewBag.ApiLayer = model.Settings.Where(x => x.SettingName == "API_LAYER").FirstOrDefault().SettingValue;
            return View(model);
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult DirectPlusAPICredentials(FormCollection model)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            CleanseMatchSettingsModel oldmodel = new CleanseMatchSettingsModel();
            oldmodel.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(oldmodel);
            // Direct+ API Credentials username & password
            string consumerKey = model["Settings[" + oldmodel.DNB_PLUS_KEY + "].SettingValue"];
            string consumerSecret = model["Settings[" + oldmodel.DNB_PLUS_SECRET + "].SettingValue"];

            try
            {
                consumerSecret = SBISCCMWeb.Utility.Utility.GetDecryptedString(consumerSecret);
            }
            catch
            {
                consumerSecret = consumerSecret.ToString();
            }
            oldmodel.Settings[oldmodel.DNB_PLUS_KEY].SettingValue = consumerKey;
            oldmodel.Settings[oldmodel.DNB_PLUS_SECRET].SettingValue = consumerSecret;
            oldmodel.Settings[oldmodel.API_LAYER].SettingValue = ApiLayerType.Directplus.ToString();

            ////check DirectPlusAPI Credentials is validate or not.
            string Result = "", ErrorMessage = "";
            Utility.Utility api = new Utility.Utility();
            dnb_Authentication dnb_authentication = new dnb_Authentication();
            dnb_authentication = api.DirectPlusAuth(consumerKey, consumerSecret, out Result, out ErrorMessage);

            if (dnb_authentication != null && string.IsNullOrEmpty(dnb_authentication.errorMessage))
            {
                Helper.DirectPlusApiToken = "Bearer " + dnb_authentication.access_token;
                //update DirectPlusAPI Credentials
                fac.UpdateCleanseMatchSettings(oldmodel.Settings);
                fac.UpdateCleanseMatchSettingsAPIFamily(ApiLayerType.Directplus.ToString());
                TempData["ApiLayer"] = ApiLayerType.Directplus.ToString();
                Helper.APILAYER = ApiLayerType.Directplus.ToString();
                return Json(MessageCollection.SettingUpdate);
            }
            return Json(MessageCollection.InvalidCredential);

        }
        #endregion

        #region Mix Mode API Credentials
        [HttpGet]
        public ActionResult MixModeAPICredentials()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            // Fill all dropdown and value for setting value.
            model.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(model);
            SetMatchGradeContent(model);
            Clear(model);
            ViewBag.ApiLayer = model.Settings.Where(x => x.SettingName == "API_LAYER").FirstOrDefault().SettingValue;
            CompanyFacade fcd = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            string APIFamily = fcd.GetAPILayer("Match and Cleanse");
            ViewBag.APIFamily = APIFamily;
            return PartialView(model);
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult MixModeAPICredentials(FormCollection model)
        {
            try
            {
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                CleanseMatchSettingsModel oldmodel = new CleanseMatchSettingsModel();
                oldmodel.Settings = fac.GetCleanseMatchSettings();
                GetSettingIDs(oldmodel);
                string APIFamily = model["APIFamily"];
                //DnB Direct License username & password
                string DnBDirectLicenseuserName = model["Settings[" + oldmodel.DNB_USERNAME + "].SettingValue"];
                string DnBDirectLicensePassword = model["Settings[" + oldmodel.DNB_PASSWORD + "].SettingValue"];
                try
                {
                    DnBDirectLicensePassword = SBISCCMWeb.Utility.Utility.GetDecryptedString(DnBDirectLicensePassword);
                }
                catch
                {
                    DnBDirectLicensePassword = DnBDirectLicensePassword.ToString();
                }

                // Direct+ API Credentials username & password
                string DirectPlusAPIuserName = model["Settings[" + oldmodel.DNB_PLUS_KEY + "].SettingValue"];
                string DirectPlusAPIPassword = model["Settings[" + oldmodel.DNB_PLUS_SECRET + "].SettingValue"];
                try
                {
                    DirectPlusAPIPassword = SBISCCMWeb.Utility.Utility.GetDecryptedString(DirectPlusAPIPassword);
                }
                catch
                {
                    DirectPlusAPIPassword = DirectPlusAPIPassword.ToString();
                }
                oldmodel.Settings[oldmodel.DNB_PLUS_KEY].SettingValue = DirectPlusAPIuserName;
                oldmodel.Settings[oldmodel.DNB_PLUS_SECRET].SettingValue = DirectPlusAPIPassword;
                oldmodel.Settings[oldmodel.DNB_USERNAME].SettingValue = DnBDirectLicenseuserName;
                oldmodel.Settings[oldmodel.DNB_PASSWORD].SettingValue = DnBDirectLicensePassword;
                oldmodel.Settings[oldmodel.API_LAYER].SettingValue = ApiLayerType.MixMode.ToString();

                //update MixModeAPI Credentials
                fac.UpdateCleanseMatchSettings(oldmodel.Settings);
                fac.UpdateCleanseMatchSettingsAPIFamily(APIFamily);
                TempData["ApiLayer"] = ApiLayerType.MixMode.ToString();
                CompanyFacade fcd = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                APIFamily = fcd.GetAPILayer("Match and Cleanse");
                Helper.APILAYER = ApiLayerType.MixMode.ToString();
                var Result = new { Message= MessageCollection.SettingUpdate, APIFamily = APIFamily };
                return Json(Result);
            }
            catch (Exception)
            {
                return Json(MessageCollection.SettingNotUpdate);
            }

        }
        #endregion


    }
}
