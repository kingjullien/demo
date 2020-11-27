using Newtonsoft.Json;
using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, AllowLicense, ValidateInput(true), DandBLicenseEnabled]
    public class DNBIdentityResolutionController : BaseController
    {
        public const string AnyGrade = "#", AnyCode = "##";
        // GET: DNBIdentityResolution
        public ActionResult Index()
        {
            return View();
        }
        #region "Minimum Match Criteria Override"
        [HttpGet]
        public ActionResult IndexMinimumConfidenceCodeOverride()
        {
            bool IsGlobal = false;
            string LOBTag = null;
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstThirdPartyAPICredentials = fac.GetMinConfidenceSettingsListPage(IsGlobal, LOBTag);
            ViewBag.ApplicationDBConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            return View(lstThirdPartyAPICredentials);
        }
        public ActionResult UpsertMinimumConfidenceCodeOverride(string Parameters)
        {
            int MinCCId = 0;
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            ThirdPartyAPICredentialsEntity objminCC = new ThirdPartyAPICredentialsEntity();
            ViewBag.ApplicationDBConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            if (Parameters != null)
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                MinCCId = Convert.ToInt32(Parameters);
                if (MinCCId > 0)
                {
                    objminCC = fac.GetMinConfidenceSettingsById(MinCCId);
                }
            }
            return View(objminCC);
        }
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult UpsertMinimumConfidenceCodeOverride(ThirdPartyAPICredentialsEntity objminCCModal)
        {
            try
            {
                ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.InsertUpdateMinConfidenceSettings(objminCCModal.Id, objminCCModal.MinConfidenceCode, objminCCModal.MaxCandidateQty, objminCCModal.Tag, objminCCModal.CredentialId, false);
                return Json(new { result = true, message = objminCCModal.Id == 0 ? CommonMessagesLang.msgCommanInsertMessage : CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgSomethingWrong }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteMinCCOverride(string Parameters)
        {
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

                    ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                    fac.DeleteMinConfidenceSettings(Convert.ToInt32(Parameters));
                    return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Global Minimum Match Criteria"
        [HttpGet]
        [Route("DNB/MinimumMatchCriteria")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexMinimumMatchCriteria()
        {
            bool IsGlobal = true;
            string LOBTag = null;
            DataTable lstThirdPartyAPICredentials;
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            lstThirdPartyAPICredentials = fac.GetMinConfidenceSettingsListPaging(IsGlobal, LOBTag);
            ThirdPartyAPICredentialsEntity model = new ThirdPartyAPICredentialsEntity();
            foreach (DataRow row in lstThirdPartyAPICredentials.Rows)
            {
                model.MinConfidenceCode = Convert.ToInt32(row["MinConfidenceCode"]);
                model.MaxCandidateQty = Convert.ToInt32(row["MaxCandidateQty"]);
                model.CredentialName = Convert.ToString(row["CredentialName"]);
                model.Id = Convert.ToInt32(row["Id"]);
                model.CredentialId = Convert.ToInt32(row["CredentialId"]);
            }

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return View(model);
            else
            {
                ViewBag.SelectedTab = "Identity Resolution";
                ViewBag.SelectedIndividualTab = "Minimum Match Criteria";
                return View("/Views/DandB/Index.cshtml", model);
            }
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult IndexMinimumMatchCriteria(string Parameters)
        {
            int Id = 0;
            int MinConfidenceCode = 0;
            int MaxCandidateQty = 0;
            string Tag = string.Empty;
            int CredentialId = 0;
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    MinConfidenceCode = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                    MaxCandidateQty = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                    Id = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));
                    CredentialId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));
                }

                ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.InsertUpdateMinConfidenceSettings(Id, MinConfidenceCode, MaxCandidateQty, Tag, CredentialId, true);
                return Json(new { result = true, message = DandBSettingLang.msgSettingUpdate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgSomethingWrong }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Exclusions for Cleanse Match API Calls"
        [Route("DNB/ExclusionsCleanseMatch")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexExclusionsCleanseMatch()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            CleanseMatchExclusionsFacade CMEfac = new CleanseMatchExclusionsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<CleanseMatchExclusions> list = CMEfac.GetAllCleanseMatchExclusions();
            CleanseMatchExclusionsEntity oCleanseMatchExclusionsEntity = new CleanseMatchExclusionsEntity();
            if(list != null && list.Count > 0)
            {
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

            }
            model.oCleanseMatchExclusionsEntity = new CleanseMatchExclusionsEntity();
            model.oCleanseMatchExclusionsEntity = oCleanseMatchExclusionsEntity;

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            ViewBag.SelectedTab = "ExclusionsCleanseMatch";
            if (Request.Headers["X-PJAX"] == "true")
            {
                return PartialView("IndexExclusionsCleanseMatch", model);
            }
            else
            {
                ViewBag.SelectedTab = "Identity Resolution";
                ViewBag.SelectedIndividualTab = "Exclusions Cleanse Match";
                return View("~/Views/DandB/Index.cshtml", model);
            }
        }
        [RequestFromSameDomain]
        public void CleanseMatchExclusions(string ExcludeNonHeadQuarters, string Tags1, string ExcludeNonMarketable, string Tags2, string ExcludeOutofBusiness, string Tags3, string ExcludeUndeliverable, string Tags4, string ExcludeUnreachable, string Tags5)
        {
            Tags1 = Tags1.TrimStart(',').TrimEnd(',');
            Tags2 = Tags2.TrimStart(',').TrimEnd(',');
            Tags3 = Tags3.TrimStart(',').TrimEnd(',');
            Tags4 = Tags4.TrimStart(',').TrimEnd(',');
            Tags5 = Tags5.TrimStart(',').TrimEnd(',');
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

        #region "Auto Acceptance Rules"
        // Listing the Auto Acceptance Rules
        [Route("DNB/AutoAcceptance")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexAutoAcceptance(string Parameters)
        {
            int? page = null, sortby = null, sortorder = null, pagevalue = null, ConfidenceCode = null, CountyGroupId = null;
            string Tags = null;
            bool Active = true;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                pagevalue = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                sortby = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                sortorder = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                if (Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1) == "")
                {
                    ConfidenceCode = 0;
                }
                else
                {
                    ConfidenceCode = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));
                }
                if (Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1) == "")
                {
                    CountyGroupId = 0;
                }
                else
                {
                    CountyGroupId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));
                }
                Tags = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1);
                Active = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 6, 1));
            }
            #region Pagination code
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 22;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 30;
            #endregion
            List<AutoAdditionalAcceptanceCriteriaEntity> model;
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            Tags = Tags == "undefined" ? null : Tags;
        Start:
            //Get List of Auto Acceptance Details
            model = fac.GetAutoAcceptanceDetailsPagedSorted(sortParam, currentPageIndex, pageSize, out totalCount, ConfidenceCode == null ? 0 : Convert.ToInt32(ConfidenceCode), CountyGroupId == null ? 0 : Convert.ToInt32(CountyGroupId), Tags, Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", Active);
            if (model != null && model.Count == 0 && Convert.ToInt32(totalCount) > 0)
            {
                currentPageIndex--;
                goto Start;
            }
            #region "set Viewbag"
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            ViewBag.ConfidenceCode = ConfidenceCode;
            ViewBag.TagList = Tags;
            ViewBag.CountyGroupId = CountyGroupId;
            ViewBag.Active = Active;
            #endregion
            IPagedList<AutoAdditionalAcceptanceCriteriaEntity> pagedAcceptance = null;
            if(model != null && model.Count > 0)
            {
                pagedAcceptance = new StaticPagedList<AutoAdditionalAcceptanceCriteriaEntity>(model.ToList(), currentPageIndex, pageSize, totalCount);
            }

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
            {
                return View(pagedAcceptance);
            }

            else
            {
                ViewBag.SelectedTab = "Identity Resolution";
                ViewBag.SelectedIndividualTab = "Auto Acceptance";
                return View("~/Views/DandB/Index.cshtml", pagedAcceptance);
            }
        }

        public ActionResult InsertUpdateAutoAcceptance(string Parameters, bool isAutoAcceptance = true)
        {
            int? CriteriaGroupId = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                CriteriaGroupId = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
            }
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
            if (CriteriaGroupId.HasValue && CriteriaGroupId > 0)
            {
                objAutoSetting = fac.GetAutoAcceptanceDetailByID(CriteriaGroupId.Value);
            }
            else
            {
                SetDefaultValue(objAutoSetting);
            }
            ViewBag.IsReview = false;
            ViewBag.isAutoAcceptance = isAutoAcceptance;
            return PartialView(objAutoSetting);
        }

        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult InsertUpdateAutoAcceptance(AutoAdditionalAcceptanceCriteriaEntity objAutoSetting, bool IsReview)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            //set properties of AutoAdditionalAcceptanceCriteria
            string MatchGradeValue = "A,B,F,Z";
            objAutoSetting.CompanyGrade = objAutoSetting.CompanyGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.CompanyGrade.Contains("#") ? "#" : objAutoSetting.CompanyGrade);
            objAutoSetting.CompanyCode = objAutoSetting.CompanyCode.Contains("##") ? "##" : objAutoSetting.CompanyCode;
            objAutoSetting.StreetGrade = objAutoSetting.StreetGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.StreetGrade.Contains("#") ? "#" : objAutoSetting.StreetGrade);
            objAutoSetting.StreetCode = objAutoSetting.StreetCode.Contains("##") ? "##" : objAutoSetting.StreetCode;
            objAutoSetting.StreetNameGrade = objAutoSetting.StreetNameGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.StreetNameGrade.Contains("#") ? "#" : objAutoSetting.StreetNameGrade);
            objAutoSetting.StreetNameCode = objAutoSetting.StreetNameCode.Contains("##") ? "##" : objAutoSetting.StreetNameCode;
            objAutoSetting.CityGrade = objAutoSetting.CityGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.CityGrade.Contains("#") ? "#" : objAutoSetting.CityGrade);
            objAutoSetting.CityCode = objAutoSetting.CityCode.Contains("##") ? "##" : objAutoSetting.CityCode;
            objAutoSetting.StateGrade = objAutoSetting.StateGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.StateGrade.Contains("#") ? "#" : objAutoSetting.StateGrade);
            objAutoSetting.StateCode = objAutoSetting.StateCode.Contains("##") ? "##" : objAutoSetting.StateCode;
            objAutoSetting.AddressGrade = objAutoSetting.AddressGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.AddressGrade.Contains("#") ? "#" : objAutoSetting.AddressGrade);
            objAutoSetting.AddressCode = objAutoSetting.AddressCode.Contains("##") ? "##" : objAutoSetting.AddressCode;
            objAutoSetting.PhoneGrade = objAutoSetting.PhoneGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.PhoneGrade.Contains("#") ? "#" : objAutoSetting.PhoneGrade);
            objAutoSetting.PhoneCode = objAutoSetting.PhoneCode.Contains("##") ? "##" : objAutoSetting.PhoneCode;
            objAutoSetting.ZipGrade = objAutoSetting.ZipGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.ZipGrade.Contains("#") ? "#" : objAutoSetting.ZipGrade);
            objAutoSetting.ZipCode = objAutoSetting.ZipCode.Contains("##") ? "##" : objAutoSetting.ZipCode;
            objAutoSetting.Density = objAutoSetting.Density.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.Density.Contains("#") ? "#" : objAutoSetting.Density);
            objAutoSetting.DensityCode = objAutoSetting.DensityCode.Contains("##") ? "##" : objAutoSetting.DensityCode;
            objAutoSetting.Uniqueness = objAutoSetting.Uniqueness.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.Uniqueness.Contains("#") ? "#" : objAutoSetting.Uniqueness);
            objAutoSetting.UniquenessCode = objAutoSetting.UniquenessCode.Contains("##") ? "##" : objAutoSetting.UniquenessCode;
            objAutoSetting.SIC = objAutoSetting.SIC.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.SIC.Contains("#") ? "#" : objAutoSetting.SIC);
            objAutoSetting.SICCode = objAutoSetting.SICCode.Contains("##") ? "##" : objAutoSetting.SICCode;
            objAutoSetting.DUNSCode = AnyCode;
            objAutoSetting.NationalIDCode = AnyCode;
            objAutoSetting.URLCode = AnyCode;
            //objAutoSetting.ExcludeFromAutoAccept = Convert.ToBoolean(objAutoSetting.ExcludeFromAutoAccept != null ? true : false);
            objAutoSetting.Tags = string.IsNullOrEmpty(objAutoSetting.Tags) ? "" : objAutoSetting.Tags == "0" ? "" : objAutoSetting.Tags.TrimStart(',').TrimEnd(',');
            objAutoSetting.GroupName = new CommonDropdown().LoadCountryGroupEntity(this.CurrentClient.ApplicationDBConnectionString).Where(a => a.GroupId.Equals(objAutoSetting.GroupId)).Select(a => a.GroupName).FirstOrDefault();
            objAutoSetting.MatchDataCriteria = objAutoSetting.MatchDataCriteria;
            objAutoSetting.OperatingStatus = objAutoSetting.OperatingStatus;
            objAutoSetting.BusinessType = objAutoSetting.BusinessType;
            objAutoSetting.UserId = Helper.oUser.UserId;
            ViewBag.IsReview = IsReview;
            ViewBag.IsReviewConfirm = IsReview == true ? true : false;
            try
            {
                if (this.Validate(objAutoSetting))
                {
                    fac.InsertOrUpdateAcceptanceSettings(objAutoSetting);
                    return Json(new { result = true, message = objAutoSetting.CriteriaGroupId > 0 ? DandBSettingLang.msgUpdateAutoAcceptance : DandBSettingLang.msgInsertAutoAcceptance, IsFromReview = IsReview }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public bool Validate(AutoAdditionalAcceptanceCriteriaEntity model)
        {
            return model.IsValidSave;
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
            model.ZipCode = AnyCode;
            model.Density = AnyGrade;
            model.DensityCode = AnyCode;
            model.Uniqueness = AnyGrade;
            model.UniquenessCode = AnyCode;
            model.SIC = AnyGrade;
            model.SICCode = AnyCode;
            model.DUNSCode = AnyCode;
            model.NationalIDCode = AnyCode;
            model.URLCode = AnyCode;
        }

        [HttpGet]
        public ActionResult DeleteComment(string CriteriaGroupId, string ToCall, string OrgColumnName = null, string ExcelColumnName = null, string Tags = null, bool IsOverWrite = false, int CompanyScore = 0)
        {
            // Open Delete Comment popup for save the information for comment
            ViewBag.OrgColumnName = !string.IsNullOrEmpty(OrgColumnName) ? OrgColumnName : null;
            ViewBag.ExcelColumnName = !string.IsNullOrEmpty(ExcelColumnName) ? ExcelColumnName : null;
            ViewBag.Tags = Tags;
            ViewBag.IsOverWrite = IsOverWrite;
            ViewBag.CompanyScore = CompanyScore;
            ViewBag.CriteriaGroupId = CriteriaGroupId;
            ViewBag.ToCall = ToCall;
            return View();
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteAcceptance(string CriteriaGroupId, int CommentId)
        {
            try
            {
                // Delete Acceptance
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.DeleteAcceptance(CriteriaGroupId, Helper.oUser.UserId, CommentId);
                return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CleanseMatchDataMatchAutoAccept()
        {
            DataTable dt = new DataTable();
            if (Session["DandB_Data"] != null)
            {
                dt = Session["DandB_Data"] as DataTable;
            }
            bool IsTag = false;
            bool IsCompanyScore = false;
            //Get Import File Column Name to fill in dropdown
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            if (dt.Rows.Count > 0)
            {
                lstAllFilter.Add(new SelectListItem { Value = "0", Text = "-Select-" });
                int i = 0;
                foreach (DataColumn c in dt.Columns)
                {
                    lstAllFilter.Add(new SelectListItem { Value = (i + 1).ToString(), Text = Convert.ToString(c.ColumnName) });
                    i++;
                    if (c.ColumnName.ToLower() == "tags" || c.ColumnName.ToLower() == "tag")
                    {
                        IsTag = true;
                    }
                }
            }

            //check CompanyScore column is exists or not in imported file  
            if (dt != null && dt.Columns != null && dt.Rows.Count > 0 && dt.Columns.Contains("CompanyScore"))
            {
                IsCompanyScore = true;
            }
            ViewBag.IsCompanyScore = IsCompanyScore;
            // Get InpCompany Data Column Names
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtSecondaryAutoAcceptanceCriteria;
            //Get Secondary Auto-Acceptance Criteria Columns Name
            dtSecondaryAutoAcceptanceCriteria = sfac.GetSecondaryAutoAcceptanceCriteriaColumnsName();
            List<string> columnName = new List<string>();
            if (dtSecondaryAutoAcceptanceCriteria.Rows.Count > 0)
            {
                for (int k = 0; k < dtSecondaryAutoAcceptanceCriteria.Rows.Count; k++)  //loop through the columns. 
                {
                    if (Convert.ToString(dtSecondaryAutoAcceptanceCriteria.Rows[k][0]) != "ImportRowId" && Convert.ToString(dtSecondaryAutoAcceptanceCriteria.Rows[k][0]) != "ImportProcessId" && Convert.ToString(dtSecondaryAutoAcceptanceCriteria.Rows[k][0]) != "CountryGroupId")
                        columnName.Add(Convert.ToString(dtSecondaryAutoAcceptanceCriteria.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            ViewBag.ExternalColumn = lstAllFilter;
            ViewBag.IsContainsTags = IsTag;
            SessionHelper.DandB_IsTag = IsTag;
            SessionHelper.IsCompanyScore = IsCompanyScore;
            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), 1, 100000, 0);
            return View(pagedProducts);
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult CleanseMatchDataMatchAutoAccept(string OrgColumnName, string ExcelColumnName, string Tags = null, bool IsOverWrite = false, int? CommentId = null, int CompanyScore = 0)
        {
            bool IsTag = false;
            bool IsCompanyScore = false;
            IsTag = SessionHelper.DandB_IsTag;
            IsCompanyScore = SessionHelper.IsCompanyScore;
            string[] OrgColumnNameArray = OrgColumnName.Split(',');
            string[] ExcelColumnNameArray = ExcelColumnName.Split(',');

            string Message = string.Empty;
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = new DataTable();
            DataTable dtOrgColumns;
            DataTable dtColumns = new DataTable();
            if (Session["DandB_Data"] != null)
            {
                dt = Session["DandB_Data"] as DataTable;
            }

            //Get Secondary Auto-Acceptance Criteria Columns Name
            dtOrgColumns = sfac.GetSecondaryAutoAcceptanceCriteriaColumnsName();
            if (!IsTag)
            {
                DataColumn Col = dt.Columns.Add("Tags", typeof(System.String));
                foreach (DataRow d in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(Tags))
                    {
                        d["Tags"] = Convert.ToString(Tags);
                    }
                }
            }

            if (!IsCompanyScore)
            {
                DataColumn Col = dt.Columns.Add("CompanyScore", typeof(System.String));
                foreach (DataRow d in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(CompanyScore)))
                    {
                        d["CompanyScore"] = Convert.ToString(CompanyScore);
                    }
                }
            }

            if (!Helper.LicenseEnableTags)
            {
                // if the tag is disable and file contain tags field in table. We just remove tags field from data table 
                var list = new List<string>(OrgColumnNameArray);
                string tag = list.Where(x => x.ToUpperInvariant() == "Tags".ToUpperInvariant()).Select(x => x.ToUpperInvariant()).FirstOrDefault();
                if (!string.IsNullOrEmpty(tag))
                {
                    list.Remove("Tags");
                    OrgColumnNameArray = list.ToArray();
                }
            }
            dtColumns.Columns.Add("Tablecolumn");
            dtColumns.Columns.Add("Excelcolumn");
            DataRow dr = dtColumns.NewRow();
            // Merge Original and Excel column in single table and Manage Dynamic Column matching 
            for (int i = 0; i < OrgColumnNameArray.Length; i++)
            {
                if (Convert.ToString(OrgColumnNameArray[i]) != "-Select-")
                {
                    dr = dtColumns.NewRow();
                    dr["Tablecolumn"] = Convert.ToString(ExcelColumnNameArray[i]);
                    dr["Excelcolumn"] = Convert.ToString(OrgColumnNameArray[i]);
                    dtColumns.Rows.Add(dr);
                }
                if (i == OrgColumnNameArray.Length - 1)
                {
                    dr = dtColumns.NewRow();
                    dr["Tablecolumn"] = "CountryGroupId";
                    dr["Excelcolumn"] = "CountryGroupId";
                    dtColumns.Rows.Add(dr);
                }
            }
            try
            {
                //remove the blank rows in data table
                string strRowFilter = string.Empty;
                for (int m = 0; m < dtOrgColumns.Rows.Count; m++)
                {
                    if (Convert.ToString(dtOrgColumns.Rows[m][1]).ToLower() == "no")
                    {
                        for (int tt = 0; tt < dtColumns.Rows.Count; tt++)
                        {
                            if (Convert.ToString(dtColumns.Rows[tt][0]) == Convert.ToString(dtOrgColumns.Rows[m][0]))
                            {
                                if (Convert.ToString(dtOrgColumns.Rows[m][0]) != "CountryGroupId")
                                {
                                    strRowFilter += "([" + Convert.ToString(dtColumns.Rows[tt][1]) + "] <> '' OR [" + Convert.ToString(dtColumns.Rows[tt][1]) + "] <> 'NULL') AND";
                                }
                            }
                        }
                    }
                }
                strRowFilter = strRowFilter.Remove(strRowFilter.Length - 3, 3);
                DataTable dtTempDataNew = new DataTable();
                DataColumn Col = dt.Columns.Add("CountryGroupId", typeof(System.Int32));
                foreach (DataRow d in dt.Rows)
                {
                    d["CountryGroupId"] = 0;
                }
                DataTable dtCloned = new DataTable();
                dtCloned = dt.Clone();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dtCloned.Columns[i].DataType = typeof(string);
                }
                foreach (DataRow item in dt.Rows)
                {
                    dtCloned.ImportRow(item);
                }
                dtTempDataNew = dtCloned.Select(strRowFilter).CopyToDataTable();
                dt = dtTempDataNew;

                //bulk insert new records
                bool IsDataInsert = BulkInsert(dt, dtColumns, out Message, IsOverWrite, CommentId);

            }
            catch (Exception)
            {
                Message = CommonMessagesLang.msgCommanEnableFileImport;
            }
            return new JsonResult { Data = Message };
        }

        public ActionResult GetAutoAcceptanceCriteriaDetailByGroupId(int CriteriaGroupId)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<AutoAcceptanceCriteriaDetail> lstAutoAcceptanceCriteriaDetail = fac.GetAutoAcceptanceCriteriaDetailByGroupId(CriteriaGroupId);
            return PartialView("_AutoAcceptanceDetailView", lstAutoAcceptanceCriteriaDetail);
        }

        #region "Export Data"
        public ActionResult ExportToExcel(int? ConfidenceCode = null, int? CountyGroupId = null, string Tags = null, bool Active = true)
        {
            // Export data to Excel Sheet .
            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
            string[] hostParts = new System.Uri(url).Host.Split('.');
            string domain = hostParts[0];
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtAutoAcceptRules = new DataTable();
            int totalCount = 0;
            List<AutoAdditionalAcceptanceCriteriaEntity> lstAutoAccpetance = new List<AutoAdditionalAcceptanceCriteriaEntity>();

            lstAutoAccpetance = fac.GetAutoAcceptanceDetailsPagedSorted(11, 1, 100000, out totalCount, ConfidenceCode == null ? 0 : Convert.ToInt32(ConfidenceCode), CountyGroupId == null ? 0 : Convert.ToInt32(CountyGroupId), Tags, Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", Active);

            dtAutoAcceptRules = CommonMethod.ToDataTable(lstAutoAccpetance);

            #region Removing Unnecessary columns
            dtAutoAcceptRules.Columns.Remove("Error");
            dtAutoAcceptRules.Columns.Remove("lstAutoAcceptanceCriteriaDetail");
            dtAutoAcceptRules.Columns.Remove("item");
            dtAutoAcceptRules.Columns.Remove("CriteriaGroupId");
            dtAutoAcceptRules.Columns.Remove("groupId");
            dtAutoAcceptRules.Columns.Remove("UserId");
            dtAutoAcceptRules.Columns.Remove("UserName");
            dtAutoAcceptRules.Columns.Remove("IsValidSave");
            dtAutoAcceptRules.Columns.Remove("MatchGrade");
            dtAutoAcceptRules.Columns.Remove("MDPCode");
            if (dtAutoAcceptRules.Columns.Contains("CountryGroupId"))
                dtAutoAcceptRules.Columns.Remove("CountryGroupId");
            #endregion

            string fileName = domain + "_AutoAcceptRules_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            string SheetName = "Auto Accept Rules";
            // Remove Tags/Tag column from excel if License Enable Tags is unchecked
            if (!Helper.LicenseEnableTags && dtAutoAcceptRules.Columns.Contains("Tags"))
            {
                dtAutoAcceptRules.Columns.Remove("Tags");
            }
            if (!Helper.LicenseEnableTags && dtAutoAcceptRules.Columns.Contains("Tag"))
            {
                dtAutoAcceptRules.Columns.Remove("Tag");
            }
            // Make Excel sheet and download file 
            byte[] response = CommonExportMethods.ExportExcelFile(dtAutoAcceptRules, fileName, SheetName);
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        #endregion
        #region "Import Data"
        public ActionResult ImportData()
        {
            return View();
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult ImportData(HttpPostedFileBase file, bool header)
        {
            if (file != null && CommonMethod.CheckFileType(".xls,.xlsx,", file.FileName))
            {
                if (file.ContentLength > 0)
                {
                    DataTable dt = new DataTable();
                    string path = string.Empty;
                    try
                    {
                        string directory = Server.MapPath("~/Content/UploadDataFile");
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        //change file name with current date & time
                        string[] strfileName = file.FileName.Split('.');
                        string extension = strfileName[strfileName.Count() - 1];
                        string fileName = System.DateTime.Now.Ticks + "." + extension;

                        path = Path.Combine(directory, Path.GetFileName(fileName));
                        file.SaveAs(path);
                        //convert Excel file to data table
                        dt = CommonImportMethods.ExcelToDataTable(path, header);
                        Session["DandB_Data"] = dt;
                        System.IO.File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.Delete(path);
                        if (ex.Message.Contains("already belongs to this DataTable"))
                        {
                            return new JsonResult { Data = ex.Message.Replace("DataTable", "file") };
                        }
                        else
                        {
                            return new JsonResult { Data = "Error:" + ex.Message };
                        }
                    }
                }
                else
                {
                    return new JsonResult { Data = CommonMessagesLang.msgCommanFileEmpty };
                }
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgCommanChechExcelFile };
            }
            return new JsonResult { Data = "success" };
        }


        // Get Current Column value for display in Example Field.
        [RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult UpdateExamples(string CurrentColumn)
        {
            string strValue = string.Empty;
            DataTable dt = new DataTable();
            if (Session["DandB_Data"] != null)
            {
                dt = Session["DandB_Data"] as DataTable;
            }
            if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(CurrentColumn) > 0)
            {
                strValue = Convert.ToString(dt.Rows[0][Convert.ToInt32(CurrentColumn) - 1]);
            }
            return new JsonResult { Data = strValue };
        }

        //bulk insert for Secondary Auto-Acceptance Criteria
        private bool BulkInsert(DataTable dt, DataTable dtColumns, out string msg, bool IsOverWrite = false, int? CommentId = null)
        {
            bool DataInsert = false;
            using (SqlConnection connection = new SqlConnection(this.CurrentClient.ApplicationDBConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //Creating Transaction so that it can rollback if got any error while uploading
                SqlTransaction trans = connection.BeginTransaction();
                //Start bulkCopy
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers, trans))
                {
                    //Setting timeout to 0 means no time out for this command will not timeout until upload complete.
                    //Change as per you
                    bulkCopy.BulkCopyTimeout = 0;
                    foreach (DataRow drCol in dtColumns.Rows)
                    {
                        bulkCopy.ColumnMappings.Add(drCol["Excelcolumn"].ToString(), drCol["Tablecolumn"].ToString());
                    }
                    bulkCopy.DestinationTableName = "ext.SecondaryAutoAcceptanceCriteriaGroup";
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        trans.Commit();
                        DataInsert = true;
                        SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                        string Message = sfac.MergeSecondaryAutoAcceptCriteria(IsOverWrite, Helper.oUser.UserId, Convert.ToInt32(CommentId));
                        msg = DandBSettingLang.msgInsertUser;
                        if (!string.IsNullOrEmpty(Message))
                        {
                            msg = Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        DataInsert = false;
                    }
                }
            }
            return DataInsert;
        }
        #endregion

        #region "set Run Auto Acceptance Rule"
        [RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult RunAutoAcceptanceRule()
        {
            //set Run Auto Acceptance Rule
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.RunAutoAcceptanceRule();
            return Json(new { result = true, message = DandBSettingLang.msgRunRules }, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region "Preview Auto-Acceptance"
        [HttpPost]
        public ActionResult PreviewAutoAcceptance(AutoAdditionalAcceptanceCriteriaEntity objAutoSetting, bool IsReview)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            //set properties of AutoAdditionalAcceptanceCriteria
            string MatchGradeValue = "A,B,F,Z";
            objAutoSetting.CompanyGrade = objAutoSetting.CompanyGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.CompanyGrade.Contains("#") ? "#" : objAutoSetting.CompanyGrade);
            objAutoSetting.CompanyCode = objAutoSetting.CompanyCode.Contains("##") ? "##" : objAutoSetting.CompanyCode;
            objAutoSetting.StreetGrade = objAutoSetting.StreetGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.StreetGrade.Contains("#") ? "#" : objAutoSetting.StreetGrade);
            objAutoSetting.StreetCode = objAutoSetting.StreetCode.Contains("##") ? "##" : objAutoSetting.StreetCode;
            objAutoSetting.StreetNameGrade = objAutoSetting.StreetNameGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.StreetNameGrade.Contains("#") ? "#" : objAutoSetting.StreetNameGrade);
            objAutoSetting.StreetNameCode = objAutoSetting.StreetNameCode.Contains("##") ? "##" : objAutoSetting.StreetNameCode;
            objAutoSetting.CityGrade = objAutoSetting.CityGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.CityGrade.Contains("#") ? "#" : objAutoSetting.CityGrade);
            objAutoSetting.CityCode = objAutoSetting.CityCode.Contains("##") ? "##" : objAutoSetting.CityCode;
            objAutoSetting.StateGrade = objAutoSetting.StateGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.StateGrade.Contains("#") ? "#" : objAutoSetting.StateGrade);
            objAutoSetting.StateCode = objAutoSetting.StateCode.Contains("##") ? "##" : objAutoSetting.StateCode;
            objAutoSetting.AddressGrade = objAutoSetting.AddressGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.AddressGrade.Contains("#") ? "#" : objAutoSetting.AddressGrade);
            objAutoSetting.AddressCode = objAutoSetting.AddressCode.Contains("##") ? "##" : objAutoSetting.AddressCode;
            objAutoSetting.PhoneGrade = objAutoSetting.PhoneGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.PhoneGrade.Contains("#") ? "#" : objAutoSetting.PhoneGrade);
            objAutoSetting.PhoneCode = objAutoSetting.PhoneCode.Contains("##") ? "##" : objAutoSetting.PhoneCode;
            objAutoSetting.ZipGrade = objAutoSetting.ZipGrade.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.ZipGrade.Contains("#") ? "#" : objAutoSetting.ZipGrade);
            objAutoSetting.ZipCode = objAutoSetting.ZipCode.Contains("##") ? "##" : objAutoSetting.ZipCode;
            objAutoSetting.Density = objAutoSetting.Density.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.Density.Contains("#") ? "#" : objAutoSetting.Density);
            objAutoSetting.DensityCode = objAutoSetting.DensityCode.Contains("##") ? "##" : objAutoSetting.DensityCode;
            objAutoSetting.Uniqueness = objAutoSetting.Uniqueness.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.Uniqueness.Contains("#") ? "#" : objAutoSetting.Uniqueness);
            objAutoSetting.UniquenessCode = objAutoSetting.UniquenessCode.Contains("##") ? "##" : objAutoSetting.UniquenessCode;
            objAutoSetting.SIC = objAutoSetting.SIC.Replace(" ", "") == MatchGradeValue ? "#" : (objAutoSetting.SIC.Contains("#") ? "#" : objAutoSetting.SIC);
            objAutoSetting.SICCode = objAutoSetting.SICCode.Contains("##") ? "##" : objAutoSetting.SICCode;
            objAutoSetting.DUNSCode = AnyCode;
            objAutoSetting.NationalIDCode = AnyCode;
            objAutoSetting.URLCode = AnyCode;
            objAutoSetting.Tags = string.IsNullOrEmpty(objAutoSetting.Tags) ? "" : objAutoSetting.Tags == "0" ? "" : objAutoSetting.Tags.TrimStart(',').TrimEnd(',');
            objAutoSetting.GroupName = new CommonDropdown().LoadCountryGroupEntity(this.CurrentClient.ApplicationDBConnectionString).Where(a => a.GroupId.Equals(objAutoSetting.GroupId)).Select(a => a.GroupName).FirstOrDefault();
            objAutoSetting.MatchDataCriteria = objAutoSetting.MatchDataCriteria;
            objAutoSetting.OperatingStatus = objAutoSetting.OperatingStatus;
            objAutoSetting.BusinessType = objAutoSetting.BusinessType;
            objAutoSetting.UserId = Helper.oUser.UserId;
            ViewBag.ExcludeFromAutoAccept = objAutoSetting.ExcludeFromAutoAccept;
            ViewBag.SingleCandidateMatchOnly = objAutoSetting.SingleCandidateMatchOnly;
            List<PreviewAutoAcceptanceRuleEntity> lstAutoAdditionalAcceptanceCriteria = fac.GetStewPreviewAutoAcceptanceRule(objAutoSetting, false, false);
            SessionHelper.objAutoSetting = JsonConvert.SerializeObject(objAutoSetting);
            return View(lstAutoAdditionalAcceptanceCriteria);
        }



        public ActionResult PreviewApplyAutoAcceptance()
        {
            List<PreviewAutoAcceptanceRuleEntity> lstAutoAdditionalAcceptanceCriteria = new List<PreviewAutoAcceptanceRuleEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.objAutoSetting))
            {
                AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = JsonConvert.DeserializeObject<AutoAdditionalAcceptanceCriteriaEntity>(SessionHelper.objAutoSetting);
            }
            ViewBag.Message = DandBSettingLang.msgSuccessApplyAutoAcceptance;
            return Json(new { result = true, data = ViewBag.Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PreviewExportAutoAcceptance()
        {
            if (!string.IsNullOrEmpty(SessionHelper.objAutoSetting))
            {
                AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = JsonConvert.DeserializeObject<AutoAdditionalAcceptanceCriteriaEntity>(SessionHelper.objAutoSetting);
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                string url = Request.Url.Scheme + "://" + Request.Url.Authority;
                string[] hostParts = new System.Uri(url).Host.Split('.');
                string domain = hostParts[0];

                DataTable dt = fac.ExportStewPreviewAutoAcceptanceRule(objAutoSetting, false, true);
                string fileName = domain + "_PreviewAutoAcceptRules_" + DateTime.Now.Ticks.ToString() + ".xlsx";
                string SheetName = "Preview Auto Accept Rules";
                // Make Excel sheet and download file 
                byte[] response = CommonExportMethods.ExportExcelFile(dt, fileName, SheetName);
                return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            return RedirectToAction("Index", "StewardshipPortal");
        }
        #endregion

        #region "Disable Rule" 
        // Disable Rule in the auto acceptance list from the UI (right - click UI option)
        [HttpPost, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts, RequestFromSameDomain]
        public JsonResult DisableRule(string Parameters)
        {
            int GroupId = 0;
            bool Activate = false;
            string Message = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                GroupId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                Activate = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
            }
            try
            {
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                sfac.ManageSecondaryAutoAcceptanceCriteriaActivation(GroupId, Activate);
                Message = Activate ? DandBSettingLang.msgRuleEnabled : DandBSettingLang.msgRuleDisabled;
                return Json(new { result = true, message = Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region AutoAcceptanceDirectives
        [Route("DNB/AutoAcceptDirectives")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexAutoAcceptDirectives()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            AutoAcceptanceDirectivesFacade AADfac = new AutoAcceptanceDirectivesFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<AutoAcceptanceDirectives> listdirectives = AADfac.GetAllAutoAcceptanceDirectives();
            AutoAcceptanceDirectivesEntity oAutoAcceptanceDirectivesEntity = new AutoAcceptanceDirectivesEntity();
            if (listdirectives != null && listdirectives.Count > 0)
            {
                //set properties of AutoAcceptanceDirective
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId1 = listdirectives[0].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId2 = listdirectives[1].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId3 = listdirectives[2].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId4 = listdirectives[3].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId5 = listdirectives[4].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId6 = listdirectives[5].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId7 = listdirectives[6].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId8 = listdirectives[7].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId9 = listdirectives[8].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId10 = listdirectives[9].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId11 = listdirectives[10].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId12 = listdirectives[11].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId13 = listdirectives[12].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.AutoAcceptanceDirectiveId14 = listdirectives[13].AutoAcceptanceDirectiveId;
                oAutoAcceptanceDirectivesEntity.Directive1 = listdirectives[0].Directive;
                oAutoAcceptanceDirectivesEntity.Directive2 = listdirectives[1].Directive;
                oAutoAcceptanceDirectivesEntity.Directive3 = listdirectives[2].Directive;
                oAutoAcceptanceDirectivesEntity.Directive4 = listdirectives[3].Directive;
                oAutoAcceptanceDirectivesEntity.Directive5 = listdirectives[4].Directive;
                oAutoAcceptanceDirectivesEntity.Directive6 = listdirectives[5].Directive;
                oAutoAcceptanceDirectivesEntity.Directive7 = listdirectives[6].Directive;
                oAutoAcceptanceDirectivesEntity.Directive8 = listdirectives[7].Directive;
                oAutoAcceptanceDirectivesEntity.Directive9 = listdirectives[8].Directive;
                oAutoAcceptanceDirectivesEntity.Directive10 = listdirectives[9].Directive;
                oAutoAcceptanceDirectivesEntity.Directive11 = listdirectives[10].Directive;
                oAutoAcceptanceDirectivesEntity.Directive12 = listdirectives[11].Directive;
                oAutoAcceptanceDirectivesEntity.Directive13 = listdirectives[12].Directive;
                oAutoAcceptanceDirectivesEntity.Directive14 = listdirectives[13].Directive;
                oAutoAcceptanceDirectivesEntity.Active1 = listdirectives[0].Active;
                oAutoAcceptanceDirectivesEntity.Active2 = listdirectives[1].Active;
                oAutoAcceptanceDirectivesEntity.Active3 = listdirectives[2].Active;
                oAutoAcceptanceDirectivesEntity.Active4 = listdirectives[3].Active;
                oAutoAcceptanceDirectivesEntity.Active5 = listdirectives[4].Active;
                oAutoAcceptanceDirectivesEntity.Active6 = listdirectives[5].Active;
                oAutoAcceptanceDirectivesEntity.Active7 = listdirectives[6].Active;
                oAutoAcceptanceDirectivesEntity.Active8 = listdirectives[7].Active;
                oAutoAcceptanceDirectivesEntity.Active9 = listdirectives[8].Active;
                oAutoAcceptanceDirectivesEntity.Active10 = listdirectives[9].Active;
                oAutoAcceptanceDirectivesEntity.Active11 = listdirectives[10].Active;
                oAutoAcceptanceDirectivesEntity.Active12 = listdirectives[11].Active;
                oAutoAcceptanceDirectivesEntity.Active13 = listdirectives[12].Active;
                oAutoAcceptanceDirectivesEntity.Active14 = listdirectives[13].Active;
                oAutoAcceptanceDirectivesEntity.Tags1 = listdirectives[0].Tags;
                oAutoAcceptanceDirectivesEntity.Tags2 = listdirectives[1].Tags;
                oAutoAcceptanceDirectivesEntity.Tags3 = listdirectives[2].Tags;
                oAutoAcceptanceDirectivesEntity.Tags4 = listdirectives[3].Tags;
                oAutoAcceptanceDirectivesEntity.Tags5 = listdirectives[4].Tags;
                oAutoAcceptanceDirectivesEntity.Tags6 = listdirectives[5].Tags;
                //oAutoAcceptanceDirectivesEntity.Tags7 = listdirectives[6].Tags;
                oAutoAcceptanceDirectivesEntity.Tags8 = listdirectives[7].Tags;
                oAutoAcceptanceDirectivesEntity.Tags9 = listdirectives[8].Tags;
                oAutoAcceptanceDirectivesEntity.Tags10 = listdirectives[9].Tags;
                oAutoAcceptanceDirectivesEntity.Tags11 = listdirectives[10].Tags;
                oAutoAcceptanceDirectivesEntity.Tags12 = listdirectives[11].Tags;
                oAutoAcceptanceDirectivesEntity.Tags13 = listdirectives[12].Tags;
                oAutoAcceptanceDirectivesEntity.Tags14 = listdirectives[13].Tags;
            }
            model.oAutoAcceptanceDirectivesEntity = new AutoAcceptanceDirectivesEntity();
            model.oAutoAcceptanceDirectivesEntity = oAutoAcceptanceDirectivesEntity;

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
            {
                return PartialView(model);
            }
            else
            {
                ViewBag.SelectedTab = "Identity Resolution";
                ViewBag.SelectedIndividualTab = "Auto Accept Directives";
                return View("~/Views/DandB/Index.cshtml", model);
            }
        }
        [RequestFromSameDomain]
        public ActionResult AutoAcceptanceDirectives(string LimitDegreeOfSeparation, string AcceptActiveRecordsOnly, string Tags1, string PreferHeadquartersRecord, string Tags2, string AcceptHeadquartersRecordOnly, string Tags3, string AcceptSingleCandidateRecordsOnly, string Tags4, string AcceptLinkedRecordOnly, string Tags5, string PreferLinkedRecord, string Tags6, string RequireDegreeOfSeparation1, string Tags8, string RequireDegreeOfSeparation2, string Tags9, string RequireDegreeOfSeparation3, string Tags10, string RequireDegreeOfSeparation4, string Tags11, string RequireDegreeOfSeparation5, string Tags12, string RequireDegreeOfSeparation6, string Tags13, string RequireDegreeOfSeparation7, string Tags14)
        {
            if (!string.IsNullOrEmpty(Tags1))
                Tags1 = Tags1.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags2))
                Tags2 = Tags2.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags3))
                Tags3 = Tags3.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags4))
                Tags4 = Tags4.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags5))
                Tags5 = Tags5.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags6))
                Tags6 = Tags6.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags8))
                Tags8 = Tags8.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags9))
                Tags9 = Tags9.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags10))
                Tags10 = Tags10.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags11))
                Tags11 = Tags11.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags12))
                Tags12 = Tags12.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags13))
                Tags13 = Tags13.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(Tags14))
                Tags14 = Tags14.TrimStart(',').TrimEnd(',');

            //set properties of Auto-Acceptance Directives
            AutoAcceptanceDirectivesEntity obj = new AutoAcceptanceDirectivesEntity();
            obj.AutoAcceptanceDirectiveId7 = 20;
            obj.Active7 = Convert.ToBoolean(LimitDegreeOfSeparation);
            obj.AutoAcceptanceDirectiveId1 = 1;
            obj.Active1 = Convert.ToBoolean(AcceptActiveRecordsOnly);
            obj.Tags1 = Tags1;
            obj.AutoAcceptanceDirectiveId2 = 2;
            obj.Active2 = Convert.ToBoolean(PreferHeadquartersRecord);
            obj.Tags2 = Tags2;
            obj.AutoAcceptanceDirectiveId3 = 3;
            obj.Active3 = Convert.ToBoolean(AcceptHeadquartersRecordOnly);
            obj.Tags3 = Tags3;
            obj.AutoAcceptanceDirectiveId4 = 4;
            obj.Active4 = Convert.ToBoolean(AcceptSingleCandidateRecordsOnly);
            obj.Tags4 = Tags4;
            obj.AutoAcceptanceDirectiveId5 = 5;
            obj.Active5 = Convert.ToBoolean(AcceptLinkedRecordOnly);
            obj.Tags5 = Tags5;
            obj.AutoAcceptanceDirectiveId5 = 6;
            obj.Active6 = Convert.ToBoolean(PreferLinkedRecord);
            obj.Tags6 = Tags6;
            obj.AutoAcceptanceDirectiveId8 = 21;
            obj.Active8 = Convert.ToBoolean(RequireDegreeOfSeparation1);
            obj.Tags8 = Tags8;
            obj.AutoAcceptanceDirectiveId9 = 22;
            obj.Active9 = Convert.ToBoolean(RequireDegreeOfSeparation2);
            obj.Tags9 = Tags9;
            obj.AutoAcceptanceDirectiveId10 = 23;
            obj.Active10 = Convert.ToBoolean(RequireDegreeOfSeparation3);
            obj.Tags10 = Tags10;
            obj.AutoAcceptanceDirectiveId11 = 24;
            obj.Active11 = Convert.ToBoolean(RequireDegreeOfSeparation4);
            obj.Tags11 = Tags11;
            obj.AutoAcceptanceDirectiveId12 = 25;
            obj.Active12 = Convert.ToBoolean(RequireDegreeOfSeparation5);
            obj.Tags12 = Tags12;
            obj.AutoAcceptanceDirectiveId13 = 26;
            obj.Active13 = Convert.ToBoolean(RequireDegreeOfSeparation6);
            obj.Tags13 = Tags13;
            obj.AutoAcceptanceDirectiveId14 = 27;
            obj.Active14 = Convert.ToBoolean(RequireDegreeOfSeparation7);
            obj.Tags14 = Tags14;
            //update AutoAcceptanceDirectives Setting
            AutoAcceptanceDirectivesFacade AADfac = new AutoAcceptanceDirectivesFacade(this.CurrentClient.ApplicationDBConnectionString);
            AADfac.UpdateAutoAcceptanceDirectives(obj);
            return Json(new { result = true, message = DandBSettingLang.msgSettingUpdate }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetSecondaryAutoAcceptanceCriteriaGroupCount()
        {
            //set Run Auto Acceptance Rule
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            int count = Convert.ToInt32(fac.GetSecondaryAutoAcceptanceCriteriaGroupCount());
            return new JsonResult { Data = count };
        }
        public static List<TagsEntity> GetAutoAcceptanceFilterTags(string ConnectionString)
        {
            // Get All tags from the database and fill the dropdown 
            TagFacade fac = new TagFacade(ConnectionString);
            List<TagsEntity> model = fac.GetAutoAcceptanceFilterTags(Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "");
            return model;
        }
    }
}