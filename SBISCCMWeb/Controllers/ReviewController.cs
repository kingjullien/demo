using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PagedList;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, DandBLicenseEnabled, AllowDataStewardshipLicense]
    public class ReviewController : BaseController
    {
        public const string AnyGrade = "#", AnyCode = "##";
        // GET: Review
        #region "Page Load"
        [HttpGet]
        public ActionResult Index(string CountryGroup = null, string ConfidenceCode = "ALL", bool Export = false, bool TopMatchCandidate = true, int? page = null, int pagevalue = 50, string OrderBy = "State", bool IsLoad = true)
        {
            if (Request.IsAjaxRequest())
            {
                int totalCount = 0;
                int currentPageIndex = 0;
                int pageSize = pagevalue > 0 ? pagevalue : 50;                
                string Tags = string.Empty;

                ViewBag.TopMatchCandidate = TopMatchCandidate;
                ViewBag.CountryGroupId = Convert.ToInt32(CountryGroup);
                ViewBag.LOBTag = Helper.oUser.LOBTag;
                ViewBag.Tag = Tags;
                ViewBag.ConfidenceCode = ConfidenceCode;
                ViewBag.OrderByColumn = OrderBy;

                pagevalue = SessionHelper.pagevalueReviewData;
                TopMatchCandidate = SessionHelper.TopMatchReviewData;
                CountryGroup = Convert.ToString(SessionHelper.CountryGroupId);
                ConfidenceCode = SessionHelper.ConfidenceCode;
                OrderBy = SessionHelper.OrderByColumnReviewData;
                pageSize = SessionHelper.CurrentPageIndex;
                Helper.oUser.LOBTag = SessionHelper.LOBTag;
                Tags = SessionHelper.Tag;


                if (string.IsNullOrEmpty(CountryGroup))
                {
                    CountryGroup = Models.CountryGroupModel.GetCountry(this.CurrentClient.ApplicationDBConnectionString).Select(x => x.GroupId).FirstOrDefault().ToString();
                }

                if (IsLoad)
                {
                    SessionHelper.oReviewDataFilter = null;
                }

                ViewBag.CountryGroup = CountryGroup;
                CompanyFacade companyFac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                DataTable dtReview = new DataTable();
                // if country is selected than find data from the database. 
                if (!String.IsNullOrEmpty(CountryGroup))
                {
                    if (SessionHelper.oReviewDataFilter == null)
                    {
                        dtReview = companyFac.GetReviewAllData(TopMatchCandidate, Convert.ToInt32(CountryGroup), Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", Tags, !string.IsNullOrEmpty(ConfidenceCode) ? ConfidenceCode : "0", Helper.oUser.UserId, OrderBy, page.HasValue ? page.Value : 0, pageSize, out totalCount);
                    }
                    else
                    {
                        ReviewDataFilter sessionreviewDataFilter = JsonConvert.DeserializeObject<ReviewDataFilter>(SessionHelper.oReviewDataFilter);
                        dtReview = companyFac.GetReviewAllData(sessionreviewDataFilter.TopMatchCandidate, Convert.ToInt32(sessionreviewDataFilter.CountryGroup), Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", sessionreviewDataFilter.Tags, sessionreviewDataFilter.ConfidenceCode, Helper.oUser.UserId, OrderBy, page.HasValue ? page.Value : 0, pageSize, out totalCount);
                    }
                    SessionHelper.ListMatchEntity = Newtonsoft.Json.JsonConvert.SerializeObject(ConvertDataTable<MatchEntity>(dtReview));
                }
                else
                {
                    TopMatchCandidate = true;
                }
                ViewBag.OrderBy = OrderBy;
                ViewBag.Tags = Tags;
                ViewBag.TopMatchCandidate = TopMatchCandidate;
                ViewBag.CountryGroup = String.IsNullOrEmpty(CountryGroup) ? 0 : Convert.ToInt32(CountryGroup);
                //Get Paged Review Data List
                IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dtReview.AsDynamicEnumerable(), page.HasValue ? page.Value : 1, pageSize, totalCount);

                return PartialView("_Index", pagedProducts);
            }
            return View();
        }

        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult Index(string Parameters, string Tags, string CountryGroup = null, string ConfidenceCode = "ALL", bool Export = false, bool TopMatchCandidate = true, int? page = null, int? pagevalue = 50, string OrderBy = "State", bool IsLoad = true)
        {
            ConfidenceCode = ConfidenceCode == "None selected" ? "" : ConfidenceCode;
            #region pagination
            int pageNumber = (page ?? 1);

            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            #endregion
            #region Set Viewbag

            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            SessionHelper.Review_pageno = Convert.ToString(currentPageIndex);
            //set view bag for Review data

            ViewBag.lstConfidanceCode = ConfidenceCode.Replace(" ", "");

            #endregion

            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                TopMatchCandidate = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                CountryGroup = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
            }
            if (string.IsNullOrEmpty(CountryGroup))
            {
                CountryGroup = Models.CountryGroupModel.GetCountry(this.CurrentClient.ApplicationDBConnectionString).Select(x => x.GroupId).FirstOrDefault().ToString();
            }

            if (!Export)
            {
                if (!IsLoad)
                {
                    ReviewDataFilter reviewDataFilter = new ReviewDataFilter();
                    reviewDataFilter.ConfidenceCode = ConfidenceCode;
                    reviewDataFilter.Tags = Tags;
                    reviewDataFilter.pagevalue = Convert.ToInt32(pagevalue);
                    reviewDataFilter.OrderBy = OrderBy;
                    reviewDataFilter.TopMatchCandidate = TopMatchCandidate;
                    reviewDataFilter.CountryGroup = CountryGroup;
                    SessionHelper.oReviewDataFilter = JsonConvert.SerializeObject(reviewDataFilter);
                }
                else
                {
                    SessionHelper.oReviewDataFilter = null;
                }

                ViewBag.CountryGroup = CountryGroup;
                CompanyFacade companyFac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                DataTable dtReview = new DataTable();
                // if country is selected than find data from the database. 
                if (!String.IsNullOrEmpty(CountryGroup))
                {
                    ReviewDataFilter sessionreviewDataFilter = null;
                    if (SessionHelper.oReviewDataFilter != null)
                    {
                        sessionreviewDataFilter = JsonConvert.DeserializeObject<ReviewDataFilter>(SessionHelper.oReviewDataFilter);
                    }
                    dtReview = companyFac.GetReviewAllData(sessionreviewDataFilter == null ? TopMatchCandidate : sessionreviewDataFilter.TopMatchCandidate, Convert.ToInt32(CountryGroup), Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", Tags, !string.IsNullOrEmpty(ConfidenceCode) ? ConfidenceCode : "0", Helper.oUser.UserId, OrderBy, currentPageIndex, pageSize, out totalCount);
                    SessionHelper.ListMatchEntity = Newtonsoft.Json.JsonConvert.SerializeObject(ConvertDataTable<MatchEntity>(dtReview));
                }
                else
                {
                    TopMatchCandidate = true;
                }
                ViewBag.OrderBy = OrderBy;
                ViewBag.Tags = Tags;
                ViewBag.TopMatchCandidate = TopMatchCandidate;
                ViewBag.CountryGroup = String.IsNullOrEmpty(CountryGroup) ? 0 : Convert.ToInt32(CountryGroup);
                //Get Paged Review Data List
                IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dtReview.AsDynamicEnumerable(), currentPageIndex, pageSize, totalCount);
                return PartialView("_Index", pagedProducts);

            }
            else
            {
                // Export search data.
                Export = false;
                if (SessionHelper.oReviewDataFilter != null)
                {
                    ReviewDataFilter sessionreviewDataFilter = null;
                    sessionreviewDataFilter = JsonConvert.DeserializeObject<ReviewDataFilter>(SessionHelper.oReviewDataFilter);
                    return ExportToExcel(sessionreviewDataFilter.TopMatchCandidate, sessionreviewDataFilter.CountryGroup, sessionreviewDataFilter.Tags, sessionreviewDataFilter.ConfidenceCode);
                }
                else
                {
                    return ExportToExcel(TopMatchCandidate, CountryGroup, Tags, ConfidenceCode);
                }
            }
        }
        #endregion

        #region "Export Data"
        [HttpPost, RequestFromSameDomain, ValidateInput(true)]
        public ActionResult ExportToExcel(bool TopMatchCandidate, string CountryGroup, string Tags, string ConfidenceCode)
        {
            SessionHelper.EmptyDataMessage = string.Empty;
            // Export data to Excel Sheet .
            int totalCount = 0;
            CompanyFacade companyFac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataTable dtTopMatchResult = new DataTable();
            dtTopMatchResult = companyFac.GetReviewAllData(Convert.ToBoolean(TopMatchCandidate), Convert.ToInt32(CountryGroup), Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", Tags, ConfidenceCode, Helper.oUser.UserId, "SrcRecordId", 1, 100000, out totalCount);
            if (dtTopMatchResult != null && dtTopMatchResult.Rows != null && dtTopMatchResult.Rows.Count > 0)
            {
                // Remove Tags/Tag column from excel if License Enable Tags is unchecked
                if (Helper.LicenseEnableTags == false && dtTopMatchResult.Columns.Contains("Tags"))
                {
                    dtTopMatchResult.Columns.Remove("Tags");
                }
                if (Helper.LicenseEnableTags == false && dtTopMatchResult.Columns.Contains("Tag"))
                {
                    dtTopMatchResult.Columns.Remove("Tag");
                }
                string fileName = "ReviewData_" + DateTime.Now.Ticks.ToString() + ".xlsx";
                string SheetName = "Review Matches";
                byte[] response = CommonExportMethods.ExportExcelFile(dtTopMatchResult, fileName, SheetName);
                return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                SessionHelper.EmptyDataMessage = "No record(s) found";
                return RedirectToAction("Index", new { IsLoad = false });
            }

        }
        #endregion

        #region "Run - Auto Accept Rules"
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult RunAutoAcceptRules(string Parameters, bool TopMatchCandidate = false, string CountryGroup = null)
        {

            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                TopMatchCandidate = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                CountryGroup = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
            }

            // Run autoAcceptrule.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.RunAutoAcceptanceRule(Helper.oUser.UserId);
            return RedirectToAction("Index", new { TopMatchCandidate = TopMatchCandidate, CountryGroup = CountryGroup });
        }
        #endregion

        #region "Additional Acceptance Criteria" 
        // Set Default value for the Model
        private void SetDefaultValue(AutoAdditionalAcceptanceCriteriaEntity model)
        {
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
            model.ZipCode = AnyGrade;
            model.Density = AnyGrade;
            model.DensityCode = AnyGrade;
            model.Uniqueness = AnyGrade;
            model.UniquenessCode = AnyGrade;
            model.SIC = AnyGrade;
            model.SICCode = AnyGrade;
            model.DUNSCode = AnyCode;
            model.NationalIDCode = AnyCode;
            model.URLCode = AnyCode;
        }
        #endregion

        #region "Right Click Event"
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult Accepted_Item(string id, string TopMatchCandidate, string CountryGroup, string Parameters)
        {
            string duns = string.Empty;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                id = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                TopMatchCandidate = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                CountryGroup = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                duns = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
            }
            ReviewDataFilter sessionReviewDataFilter = null;
            if (SessionHelper.oReviewDataFilter != null)
            {
                sessionReviewDataFilter = JsonConvert.DeserializeObject<ReviewDataFilter>(SessionHelper.oReviewDataFilter);
            }
            //TopMatchCandidate = sessionReviewDataFilter != null ? Convert.ToString(sessionReviewDataFilter.TopMatchCandidate) : "true";
            // Accept data on right click of match in grid.
            List<MatchEntity> listMatches = new List<MatchEntity>();
            if (!string.IsNullOrEmpty(id))
            {
                id = id.TrimEnd(',');
                if (id.IndexOf(',') > -1)
                {
                    string[] newId = id.Split(',');
                    string[] arrDuns = duns.Split(',');
                    for (int i = 0; i < newId.Length; i++)
                    {
                        listMatches = FillMatchEntity(newId[i], TopMatchCandidate, CountryGroup, arrDuns[i]);
                        if (listMatches.Count > 0)
                        {
                            AcceptAllMatches(listMatches);
                        }
                    }
                }
                else
                {
                    listMatches = FillMatchEntity(id, TopMatchCandidate, CountryGroup, duns);
                    if (listMatches.Count > 0)
                    {
                        AcceptAllMatches(listMatches);
                    }
                }
            }
            // And remove from the list and display other data on list.
            return RedirectToAction("Index", new { Export = false, TopMatchCandidate = TopMatchCandidate, CountryGroup = CountryGroup });
        }
        public List<MatchEntity> FillMatchEntity(string id, string TopMatchCandidate, string CountryGroup, string duns)
        {
            // Get Data from the id and pass to selected match list.
            int totalCount = 0;
            CompanyFacade companyFac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataTable dtReview = new DataTable();
            MatchEntity Match = new MatchEntity();
            List<MatchEntity> listMatches = new List<MatchEntity>();
            if (Convert.ToInt32(CountryGroup) > 0)
            {
                dtReview = companyFac.GetReviewAllData(Convert.ToBoolean(TopMatchCandidate), Convert.ToInt32(CountryGroup), Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", "", "ALL", Helper.oUser.UserId, "SrcRecordId", 1, 100000, out totalCount);
            }
            if (dtReview != null && dtReview.Rows.Count > 0)
            {
                for (int i = 0; i < dtReview.Rows.Count; i++)
                {
                    if (Convert.ToString(dtReview.Rows[i]["InputId"]) == id && Convert.ToString(dtReview.Rows[i]["DnBDUNSNumber"]) == duns)
                    {
                        Match.InputId = Convert.ToInt32(dtReview.Rows[i]["InputId"]);
                        Match.SrcRecordId = Convert.ToString(dtReview.Rows[i]["SrcRecordId"]);
                        Match.DnBDUNSNumber = Convert.ToString(dtReview.Rows[i]["DnBDUNSNumber"]);
                        Match.DnBConfidenceCode = Convert.ToInt32(dtReview.Rows[i]["DnBConfidenceCode"]);
                        Match.DnBMatchGradeText = Convert.ToString(dtReview.Rows[i]["DnBMatchGradeText"]);
                        Match.DnBMatchDataProfileText = Convert.ToString(dtReview.Rows[i]["DnBMatchDataProfileText"]);
                        Match.DnBOrganizationName = Convert.ToString(dtReview.Rows[i]["DnBOrganizationName"]);
                        Match.MGVCompanyName = Convert.ToString(dtReview.Rows[i]["MGVCompanyName"]);
                        Match.MDPVCompanyName = Convert.ToString(dtReview.Rows[i]["MDPVCompanyName"]);
                        Match.DnBTradeStyleName = Convert.ToString(dtReview.Rows[i]["DnBTradeStyleName"]);
                        Match.DnBSeniorPrincipalName = Convert.ToString(dtReview.Rows[i]["DnBSeniorPrincipalName"]);
                        Match.DnBStreetAddressLine = Convert.ToString(dtReview.Rows[i]["DnBStreetAddressLine"]);
                        Match.MGVStreetNo = Convert.ToString(dtReview.Rows[i]["MGVStreetNo"]);
                        Match.MDPVStreetNo = Convert.ToString(dtReview.Rows[i]["MDPVStreetNo"]);
                        Match.MGVStreetName = Convert.ToString(dtReview.Rows[i]["MGVStreetName"]);
                        Match.MDPVStreetName = Convert.ToString(dtReview.Rows[i]["MDPVStreetName"]);
                        Match.DnBPrimaryTownName = Convert.ToString(dtReview.Rows[i]["DnBPrimaryTownName"]);
                        Match.MGVCity = Convert.ToString(dtReview.Rows[i]["MGVCity"]);
                        Match.MDPVCity = Convert.ToString(dtReview.Rows[i]["MDPVCity"]);
                        Match.DnBTerritoryAbbreviatedName = Convert.ToString(dtReview.Rows[i]["DnBTerritoryAbbreviatedName"]);
                        Match.MGVState = Convert.ToString(dtReview.Rows[i]["MGVState"]);
                        Match.MDPVState = Convert.ToString(dtReview.Rows[i]["MDPVState"]);
                        Match.MGVMailingAddress = Convert.ToString(dtReview.Rows[i]["MGVMailingAddress"]);
                        Match.MDPVMailingAddress = Convert.ToString(dtReview.Rows[i]["MDPVMailingAddress"]);
                        Match.DnBTelephoneNumber = Convert.ToString(dtReview.Rows[i]["DnBTelephoneNumber"]);
                        Match.MGVTelephone = Convert.ToString(dtReview.Rows[i]["MGVTelephone"]);
                        Match.MDPVTelephone = Convert.ToString(dtReview.Rows[i]["MDPVTelephone"]);
                        Match.DnBPostalCode = Convert.ToString(dtReview.Rows[i]["DnBPostalCode"]);
                        Match.DnBPostalCodeExtensionCode = Convert.ToString(dtReview.Rows[i]["DnBPostalCodeExtensionCode"]);
                        Match.MGVZipCode = Convert.ToString(dtReview.Rows[i]["MGVZipCode"]);
                        Match.DnBCountryISOAlpha2Code = Convert.ToString(dtReview.Rows[i]["DnBCountryISOAlpha2Code"]);
                        Match.DnBAddressUndeliverable = Convert.ToString(dtReview.Rows[i]["DnBAddressUndeliverable"]);
                        Match.DnBOperatingStatus = Convert.ToString(dtReview.Rows[i]["DnBOperatingStatus"]);
                        Match.DnBFamilyTreeMemberRole = Convert.ToString(dtReview.Rows[i]["DnBFamilyTreeMemberRole"]);
                        Match.DnBStandaloneOrganization = Convert.ToString(dtReview.Rows[i]["DnBStandaloneOrganization"]);
                        Match.DnBDisplaySequence = Convert.ToString(dtReview.Rows[i]["DnBDisplaySequence"]);
                        Match.MGVDensity = Convert.ToString(dtReview.Rows[i]["MGVDensity"]);
                        Match.MGVUniqueness = Convert.ToString(dtReview.Rows[i]["MGVUniqueness"]);
                        Match.MGVSIC = Convert.ToString(dtReview.Rows[i]["MGVSIC"]);
                        listMatches.Add(Match);
                    }
                }
            }
            return listMatches;
        }
        public void AcceptAllMatches(List<MatchEntity> listMatches)
        {
            // Accept data on right click of match in grid.
            CompanyFacade companyFac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            companyFac.AcceptLCMMatches(listMatches, Helper.oUser.UserId, true);
        }
        public ActionResult CC_Item(string Parameters)
        {
            int Country = 0;
            string Tags = "", ConfidanceCode = "";
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ConfidanceCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                if(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1) == "")
                {
                    Country = 0;
                }
                else
                {
                    Country = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                }
                Tags = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                Tags = !string.IsNullOrEmpty(Tags) ? Tags.Replace("@@", "::") : Tags;

            }

            // Open Popup for set Set Only ConfidanceCode to the popup and not selected Exclude From Auto Accept
            ViewBag.IsReview = true;
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString); ;
            AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
            objAutoSetting.ConfidenceCode = ConfidanceCode;
            objAutoSetting.CompanyCode = AnyCode;
            objAutoSetting.GroupId = Country;
            objAutoSetting.CompanyGrade = AnyGrade;
            objAutoSetting.StreetGrade = AnyGrade;
            objAutoSetting.StreetCode = AnyCode;
            objAutoSetting.StreetNameGrade = AnyGrade;
            objAutoSetting.StreetNameCode = AnyCode;
            objAutoSetting.CityGrade = AnyGrade;
            objAutoSetting.CityCode = AnyCode;
            objAutoSetting.StateGrade = AnyGrade;
            objAutoSetting.StateCode = AnyCode;
            objAutoSetting.AddressGrade = AnyGrade;
            objAutoSetting.AddressCode = AnyCode;
            objAutoSetting.PhoneGrade = AnyGrade;
            objAutoSetting.PhoneCode = AnyCode;
            objAutoSetting.ZipGrade = AnyGrade;
            objAutoSetting.ZipCode = AnyCode;
            objAutoSetting.Density = AnyGrade;
            objAutoSetting.DensityCode = AnyCode;
            objAutoSetting.Uniqueness = AnyGrade;
            objAutoSetting.UniquenessCode = AnyCode;
            objAutoSetting.SIC = AnyGrade;
            objAutoSetting.SICCode = AnyCode;
            objAutoSetting.ExcludeFromAutoAccept = false;
            objAutoSetting.Tags = Tags;
            objAutoSetting.DUNSCode = AnyCode;
            objAutoSetting.NationalIDCode = AnyCode;
            objAutoSetting.URLCode = AnyCode;
            objAutoSetting.SingleCandidateMatchOnly = false;
            objAutoSetting.MatchDataCriteria = objAutoSetting.MatchDataCriteria;
            objAutoSetting.OperatingStatus = objAutoSetting.OperatingStatus;
            objAutoSetting.BusinessType = objAutoSetting.BusinessType;
            return PartialView("~/Views/DNBIdentityResolution/InsertUpdateAutoAcceptance.cshtml", objAutoSetting);
        }
        public ActionResult CCMG_Item(string Parameters)
        {
            int Country = 0;
            string DnBMatchGradeText = "", ConfidanceCode = "";
            string Tags = "";
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ConfidanceCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                if (Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1) == "")
                {
                    Country = 0;
                }
                else
                {
                    Country = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                }
                DnBMatchGradeText = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                Tags = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));
                Tags = !string.IsNullOrEmpty(Tags) ? Tags.Replace("@@", "::") : Tags;
            }
            // Open Popup for set Set Only ConfidanceCode and MatchGrade to the popup and not selected Exclude From Auto Accept
            ViewBag.IsReview = true;
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();

            //set value of Code and Grade
            objAutoSetting = setCode_Grade(ConfidanceCode, Country, DnBMatchGradeText, false, false);
            objAutoSetting.Tags = Tags;
            return PartialView("~/Views/DNBIdentityResolution/InsertUpdateAutoAcceptance.cshtml", objAutoSetting);
        }
        public ActionResult CCMGMDP_Item(string Parameters)
        {

            int Country = 0;
            string DnBMatchGradeText = "", DnBMatchDataProfileText = "", ConfidanceCode = "";
            string Tags = "";
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ConfidanceCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                if (Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1) == "")
                {
                    Country = 0;
                }
                else
                {
                    Country = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                }
                DnBMatchGradeText = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                DnBMatchDataProfileText = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                Tags = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));
                Tags = !string.IsNullOrEmpty(Tags) ? Tags.Replace("@@", "::") : Tags;
            }
            // Open Popup for set Set Only ConfidanceCode, MatchGrade and match data profile to the popup and not selected Exclude From Auto Accept
            ViewBag.IsReview = true;
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString); ;
            AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
            objAutoSetting = setCode_Grade_MDP(ConfidanceCode, Country, DnBMatchGradeText, DnBMatchDataProfileText, false, false);
            objAutoSetting.Tags = Tags;
            return PartialView("~/Views/DNBIdentityResolution/InsertUpdateAutoAcceptance.cshtml", objAutoSetting);
        }
        public ActionResult ExCCMG_Item(string Parameters)
        {
            int Country = 0;
            string DnBMatchGradeText = "", ConfidanceCode = "";
            string Tags = "";
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

                ConfidanceCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                if (Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1) == "")
                {
                    Country = 0;
                }
                else
                {
                    Country = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                }
                DnBMatchGradeText = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                Tags = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));
                Tags = !string.IsNullOrEmpty(Tags) ? Tags.Replace("@@", "::") : Tags;
            }
            // Open Popup for set Set Only ConfidanceCode and MatchGrade to the popup and  selected Exclude From Auto Accept
            ViewBag.IsReview = true;
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString); ;
            AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();

            //set value of Code and Grade
            objAutoSetting = setCode_Grade(ConfidanceCode, Country, DnBMatchGradeText, true, false);
            objAutoSetting.Tags = Tags;
            return PartialView("~/Views/DNBIdentityResolution/InsertUpdateAutoAcceptance.cshtml", objAutoSetting);
        }
        public ActionResult ExCCMGMDP_Item(string Parameters)
        {
            int Country = 0;
            string DnBMatchGradeText = "", DnBMatchDataProfileText = "", ConfidanceCode = "";
            string Tags = "";
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ConfidanceCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                if (Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1) == "")
                {
                    Country = 0;
                }
                else
                {
                    Country = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                }
                DnBMatchGradeText = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                DnBMatchDataProfileText = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                Tags = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));
                Tags = !string.IsNullOrEmpty(Tags) ? Tags.Replace("@@", "::") : Tags;
            }

            // Open Popup for set Set Only ConfidanceCode, MatchGrade and MatchDataProfile to the popup and  selected Exclude From Auto Accept
            ViewBag.IsReview = true;
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString); ;
            AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
            objAutoSetting = setCode_Grade_MDP(ConfidanceCode, Country, DnBMatchGradeText, DnBMatchDataProfileText, true, false);
            objAutoSetting.Tags = Tags;
            return PartialView("~/Views/DNBIdentityResolution/InsertUpdateAutoAcceptance.cshtml", objAutoSetting);
        }
        [HttpGet]
        public ActionResult MatchDetailView_Item(string Parameters)
        {

            string id = "", TopMatchCandidate = "", CountryGroup = "", dataNext = "", dataPrev = "", DUNS = "", OrderBy = "";
            bool IsPartialView = false;
            MatchEntity Match = new MatchEntity();
            List<MatchEntity> listMatchEntity = new List<MatchEntity>();
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

                id = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                TopMatchCandidate = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                CountryGroup = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                dataNext = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                dataPrev = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
                DUNS = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1);
                OrderBy = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 6, 1);
                IsPartialView = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 7, 1));
            }
            if (!string.IsNullOrEmpty(SessionHelper.ListMatchEntity))
            {
                listMatchEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MatchEntity>>(SessionHelper.ListMatchEntity);
                Match = listMatchEntity.Where(x => x.InputId == Convert.ToInt32(id)).Where(x => x.DnBDUNSNumber == DUNS).FirstOrDefault();
            }
            else
            {

                int totalCount = 0;
                // Open Match Detail Popup with current data.
                DataTable dtReview = new DataTable();

                CompanyFacade companyFac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                bool IsTopMatch = true;

                ReviewDataFilter sessionReviewDataFilter = null;
                if (SessionHelper.oReviewDataFilter != null)
                {
                    sessionReviewDataFilter = JsonConvert.DeserializeObject<ReviewDataFilter>(SessionHelper.oReviewDataFilter);
                }

                if (sessionReviewDataFilter != null)
                {
                    IsTopMatch = sessionReviewDataFilter.TopMatchCandidate;
                }
                OrderBy = sessionReviewDataFilter != null ? string.IsNullOrEmpty(sessionReviewDataFilter.OrderBy) ? "State" : sessionReviewDataFilter.OrderBy : "State";
                if (!String.IsNullOrEmpty(CountryGroup))
                {
                    //dtReview = companyFac.GetReviewAllData(Convert.ToBoolean(TopMatchCandidate), Convert.ToInt32(CountryGroup), Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", "", "ALL", Helper.oUser.UserId, !string.IsNullOrEmpty(OrderBy) ? OrderBy : "State", 1, 100000, out totalCount);
                    dtReview = companyFac.GetReviewAllData(IsTopMatch, Convert.ToInt32(CountryGroup), Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", "", "ALL", Helper.oUser.UserId, !string.IsNullOrEmpty(OrderBy) ? OrderBy : "State", 1, 100000, out totalCount);

                    if (dtReview != null)
                    {
                        if (dtReview.Rows.Count > 0)
                        {
                            listMatchEntity = ConvertDataTable<MatchEntity>(dtReview);
                            for (int i = 0; i < dtReview.Rows.Count; i++)
                            {
                                if (Convert.ToString(dtReview.Rows[i]["InputId"]) == id && Convert.ToString(dtReview.Rows[i]["DnBDUNSNumber"]) == DUNS)
                                {
                                    Helper.CompanyName = Convert.ToString(dtReview.Rows[i]["CompanyName"]) == "" ? "Null" : Convert.ToString(dtReview.Rows[i]["CompanyName"]);
                                    Helper.Address = Convert.ToString(dtReview.Rows[i]["Address"]) == "" ? "Null" : Convert.ToString(dtReview.Rows[i]["Address"]);
                                    Helper.City = Convert.ToString(dtReview.Rows[i]["City"]) == "" ? "Null" : Convert.ToString(dtReview.Rows[i]["City"]);
                                    Helper.State = Convert.ToString(dtReview.Rows[i]["State"]) == "" ? "Null" : Convert.ToString(dtReview.Rows[i]["State"]);
                                    Helper.PhoneNbr = Convert.ToString(dtReview.Rows[i]["PhoneNbr"]) == "" ? "Null" : Convert.ToString(dtReview.Rows[i]["PhoneNbr"]);
                                    Helper.Zip = Convert.ToString(dtReview.Rows[i]["PostalCode"]) == "" ? "Null" : Convert.ToString(dtReview.Rows[i]["PostalCode"]);
                                }
                            }
                        }
                    }
                    SessionHelper.ListMatchEntity = Newtonsoft.Json.JsonConvert.SerializeObject(listMatchEntity);
                }

                if (listMatchEntity != null)
                {
                    if (listMatchEntity.Count > 0)
                    {
                        Match = listMatchEntity.Where(x => x.InputId == Convert.ToInt32(id)).Where(x => x.DnBDUNSNumber == DUNS).FirstOrDefault();
                    }
                }
            }
            ViewBag.dataNext = dataNext;
            ViewBag.dataPrev = dataPrev;
            ViewBag.SelectData = true;
            ViewBag.OrderBy = OrderBy;
            ViewBag.TopMatchCandidate = TopMatchCandidate;
            ViewBag.CountryGroup = CountryGroup;
            try
            {
                ViewBag.NextToNextDUNS = dataNext != "" ? listMatchEntity.SkipWhile(p => p.InputId != Convert.ToInt32(dataNext)).ElementAt(1).InputId : listMatchEntity.SkipWhile(p => p.InputId != Convert.ToInt32(id)).ElementAt(1).InputId;
            }
            catch
            {
                ViewBag.NextToNextDUNS = "";
            }
            try
            {
                ViewBag.PrevToPrevDUNS = dataPrev != "" ? listMatchEntity.TakeWhile(p => p.InputId != Convert.ToInt32(dataPrev)).LastOrDefault().InputId : listMatchEntity.TakeWhile(p => p.InputId != Convert.ToInt32(id)).LastOrDefault().InputId;
            }
            catch
            {
                ViewBag.PrevToPrevDUNS = "";

            }

            try
            {
                ViewBag.NextDUNS = dataNext != "" ? listMatchEntity.Where(p => p.InputId == Convert.ToInt32(dataNext)).FirstOrDefault().DnBDUNSNumber : listMatchEntity.Where(p => p.InputId == Convert.ToInt32(id)).FirstOrDefault().DnBDUNSNumber;
            }
            catch
            {
                ViewBag.NextDUNS = "";
            }
            try
            {
                ViewBag.PrevDUNS = dataPrev != "" ? listMatchEntity.Where(p => p.InputId == Convert.ToInt32(dataPrev)).FirstOrDefault().DnBDUNSNumber : listMatchEntity.Where(p => p.InputId == Convert.ToInt32(id)).FirstOrDefault().DnBDUNSNumber;
            }
            catch
            {
                ViewBag.PrevDUNS = "";

            }
            //when Popup is already open and click on next previous at that time we reload just partial view
            if (!IsPartialView)
            {
                return PartialView("_MatchedItemDetailView", Match);
            }
            else
            {
                return PartialView("_MatchDetails", Match);
            }
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            // Convert Data table to list
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            // Convert data row to generic type.
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        SetValue(obj, column.ColumnName, dr[column.ColumnName]);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
        public static void SetValue(object inputObject, string propertyName, object propertyVal)
        {
            //find out the type
            Type type = inputObject.GetType();
            //get the property information based on the type
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propertyName);
            //find the property type
            Type propertyType = propertyInfo.PropertyType;
            //Convert.ChangeType does not handle conversion to nullable types
            //if the property type is nullable, we need to get the underlying type of the property
            var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;
            //Returns an System.Object with the specified System.Type and whose value is
            //equivalent to the specified object.
            propertyVal = Convert.ChangeType(propertyVal, targetType);
            //Set the value of the property
            propertyInfo.SetValue(inputObject, propertyVal, null);
        }
        private static bool IsNullableType(Type type)
        {
            // to check type 
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
        [AllowAnonymous]
        public ActionResult GetLayoutPopUp()
        {
            return PartialView("~/Views/Shared/_LayoutPopup.cshtml");
        }
        #endregion

        #region "set ConfidanceCode and MatchGrade"
        //set  ConfidanceCode and MatchGrade to the popup
        public AutoAdditionalAcceptanceCriteriaEntity setCode_Grade(string ConfidanceCode, int Country, string DnBMatchGradeText, bool ExcludeFromAutoAccept, bool SingleCandidateMatchOnly)
        {
            AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
            objAutoSetting.ConfidenceCode = ConfidanceCode;
            objAutoSetting.CompanyCode = AnyCode;
            objAutoSetting.GroupId = Country;
            objAutoSetting.CompanyGrade = DnBMatchGradeText.Length > 0 ? DnBMatchGradeText.Substring(0, 1) : "#";
            objAutoSetting.StreetGrade = DnBMatchGradeText.Length > 1 ? DnBMatchGradeText.Substring(1, 1) : "#";
            objAutoSetting.StreetCode = AnyCode;
            objAutoSetting.StreetNameGrade = DnBMatchGradeText.Length > 2 ? DnBMatchGradeText.Substring(2, 1) : "#";
            objAutoSetting.StreetNameCode = AnyCode;
            objAutoSetting.CityGrade = DnBMatchGradeText.Length > 3 ? DnBMatchGradeText.Substring(3, 1) : "#";
            objAutoSetting.CityCode = AnyCode;
            objAutoSetting.StateGrade = DnBMatchGradeText.Length > 4 ? DnBMatchGradeText.Substring(4, 1) : "#";
            objAutoSetting.StateCode = AnyCode;
            objAutoSetting.AddressGrade = DnBMatchGradeText.Length > 5 ? DnBMatchGradeText.Substring(5, 1) : "#";
            objAutoSetting.AddressCode = AnyCode;
            objAutoSetting.PhoneGrade = DnBMatchGradeText.Length > 6 ? DnBMatchGradeText.Substring(6, 1) : "#";
            objAutoSetting.PhoneCode = AnyCode;
            objAutoSetting.ZipGrade = DnBMatchGradeText.Length > 7 ? DnBMatchGradeText.Substring(7, 1) : "#";
            objAutoSetting.ZipCode = AnyCode;
            objAutoSetting.Density = DnBMatchGradeText.Length > 8 ? DnBMatchGradeText.Substring(8, 1) : "#";
            objAutoSetting.DensityCode = AnyCode;
            objAutoSetting.Uniqueness = DnBMatchGradeText.Length > 9 ? DnBMatchGradeText.Substring(9, 1) : "#";
            objAutoSetting.UniquenessCode = AnyCode;
            objAutoSetting.SIC = DnBMatchGradeText.Length > 10 ? DnBMatchGradeText.Substring(10, 1) : "#";
            objAutoSetting.SICCode = AnyCode;
            objAutoSetting.DUNSCode = "##";
            objAutoSetting.NationalIDCode = "##";
            objAutoSetting.URLCode = "##";
            objAutoSetting.ExcludeFromAutoAccept = ExcludeFromAutoAccept;
            objAutoSetting.SingleCandidateMatchOnly = SingleCandidateMatchOnly;
            objAutoSetting.MatchDataCriteria = objAutoSetting.MatchDataCriteria;
            objAutoSetting.OperatingStatus = objAutoSetting.OperatingStatus;
            objAutoSetting.BusinessType = objAutoSetting.BusinessType;
            return objAutoSetting;
        }
        //set  ConfidanceCode and MatchGrade to the popup
        public AutoAdditionalAcceptanceCriteriaEntity setCode_Grade_MDP(string ConfidanceCode, int Country, string DnBMatchGradeText, string DnBMatchDataProfileText, bool ExcludeFromAutoAccept, bool SingleCandidateMatchOnly)
        {
            AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
            objAutoSetting.ConfidenceCode = ConfidanceCode;
            objAutoSetting.CompanyCode = DnBMatchDataProfileText.Length > 0 ? DnBMatchDataProfileText.Substring(0, 2) : AnyCode; ;
            objAutoSetting.GroupId = Country;
            objAutoSetting.CompanyGrade = DnBMatchGradeText.Length > 0 ? DnBMatchGradeText.Substring(0, 1) : "#";
            objAutoSetting.StreetGrade = DnBMatchGradeText.Length > 1 ? DnBMatchGradeText.Substring(1, 1) : "#";
            objAutoSetting.StreetCode = DnBMatchDataProfileText.Length > 3 ? DnBMatchDataProfileText.Substring(2, 2) : AnyCode;
            objAutoSetting.StreetNameGrade = DnBMatchGradeText.Length > 2 ? DnBMatchGradeText.Substring(2, 1) : "#";
            objAutoSetting.StreetNameCode = DnBMatchDataProfileText.Length > 5 ? DnBMatchDataProfileText.Substring(4, 2) : AnyCode;
            objAutoSetting.CityGrade = DnBMatchGradeText.Length > 3 ? DnBMatchGradeText.Substring(3, 1) : "#";
            objAutoSetting.CityCode = DnBMatchDataProfileText.Length > 7 ? DnBMatchDataProfileText.Substring(6, 2) : AnyCode;
            objAutoSetting.StateGrade = DnBMatchGradeText.Length > 4 ? DnBMatchGradeText.Substring(4, 1) : "#";
            objAutoSetting.StateCode = DnBMatchDataProfileText.Length > 9 ? DnBMatchDataProfileText.Substring(8, 2) : AnyCode;
            objAutoSetting.AddressGrade = DnBMatchGradeText.Length > 5 ? DnBMatchGradeText.Substring(5, 1) : "#";
            objAutoSetting.AddressCode = DnBMatchDataProfileText.Length > 11 ? DnBMatchDataProfileText.Substring(10, 2) : AnyCode;
            objAutoSetting.PhoneGrade = DnBMatchGradeText.Length > 6 ? DnBMatchGradeText.Substring(6, 1) : "#";
            objAutoSetting.PhoneCode = DnBMatchDataProfileText.Length > 13 ? DnBMatchDataProfileText.Substring(12, 2) : AnyCode;
            objAutoSetting.ZipGrade = DnBMatchGradeText.Length > 7 ? DnBMatchGradeText.Substring(7, 1) : "#";
            objAutoSetting.ZipCode = DnBMatchDataProfileText.Length > 15 ? DnBMatchDataProfileText.Substring(14, 2) : AnyCode;
            objAutoSetting.Density = DnBMatchGradeText.Length > 8 ? DnBMatchGradeText.Substring(8, 1) : "#";
            objAutoSetting.DensityCode = DnBMatchDataProfileText.Length > 17 ? DnBMatchDataProfileText.Substring(16, 2) : AnyCode;
            objAutoSetting.Uniqueness = DnBMatchGradeText.Length > 9 ? DnBMatchGradeText.Substring(9, 1) : "#";
            objAutoSetting.UniquenessCode = DnBMatchDataProfileText.Length > 19 ? DnBMatchDataProfileText.Substring(18, 2) : AnyCode;
            objAutoSetting.SIC = DnBMatchGradeText.Length > 10 ? DnBMatchGradeText.Substring(10, 1) : "#";
            objAutoSetting.SICCode = DnBMatchDataProfileText.Length > 21 ? DnBMatchDataProfileText.Substring(20, 2) : AnyCode;
            objAutoSetting.DUNSCode = DnBMatchDataProfileText.Length > 23 ? DnBMatchDataProfileText.Substring(22, 2) : AnyCode;
            objAutoSetting.NationalIDCode = DnBMatchDataProfileText.Length > 25 ? DnBMatchDataProfileText.Substring(24, 2) : AnyCode;
            objAutoSetting.URLCode = DnBMatchDataProfileText.Length > 27 ? DnBMatchDataProfileText.Substring(26, 2) : AnyCode;
            objAutoSetting.ExcludeFromAutoAccept = ExcludeFromAutoAccept;
            objAutoSetting.SingleCandidateMatchOnly = SingleCandidateMatchOnly;
            objAutoSetting.MatchDataCriteria = objAutoSetting.MatchDataCriteria;
            objAutoSetting.OperatingStatus = objAutoSetting.OperatingStatus;
            objAutoSetting.BusinessType = objAutoSetting.BusinessType;
            return objAutoSetting;
        }
        #endregion

        #region "Review Match Data Filters"
        public JsonResult GetOrderByColumnDD()
        {
            List<DropDownReturn> lstAllFilter = new List<DropDownReturn>();
            lstAllFilter.Add(new DropDownReturn { Value = "SrcRecordId", Text = "SrcRecordId" });
            lstAllFilter.Add(new DropDownReturn { Value = "Company", Text = "Company" });
            lstAllFilter.Add(new DropDownReturn { Value = "State", Text = "State" });
            return Json(new { Data = lstAllFilter }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTopMatchCandidateDD()
        {
            List<DropDownReturn> lstGetTopMatchCandidateDD = new List<DropDownReturn>();
            lstGetTopMatchCandidateDD.Add(new DropDownReturn { Value = "false", Text = "false" });
            lstGetTopMatchCandidateDD.Add(new DropDownReturn { Value = "true", Text = "true" });
            return Json(new { Data = lstGetTopMatchCandidateDD }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCountryGroupDD()
        {
            // Load Country Group Entity
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = fac.GetCountryGroupsInFilter();
            List<DropDownReturn> lstCountryGroup = new List<DropDownReturn>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstCountryGroup.Add(new DropDownReturn { Value = dt.Rows[i]["GroupId"].ToString(), Text = dt.Rows[i]["GroupName"].ToString() });
            }
            return Json(new { Data = lstCountryGroup }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNumberOfRecordsDD()
        {
            List<DropDownReturn> lstConfidenceCode = new List<DropDownReturn>();
            lstConfidenceCode.Add(new DropDownReturn { Value = "50", Text = "50" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "100", Text = "100" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "200", Text = "200" });
            return Json(new { Data = lstConfidenceCode }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetConfidenceCodeDD()
        {
            List<DropDownReturn> lstConfidenceCode = new List<DropDownReturn>();
            lstConfidenceCode.Add(new DropDownReturn { Value = "3", Text = "3" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "4", Text = "4" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "5", Text = "5" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "6", Text = "6" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "7", Text = "7" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "8", Text = "8" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "9", Text = "9" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "10", Text = "10" });
            return Json(new { Data = lstConfidenceCode }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTagsDD()
        {
            // Get All tags from the database and fill the dropdown 
            List<DropDownReturn> lstTags = new List<DropDownReturn>();
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = fac.GetAllTagsForUserInFilter(Helper.oUser.LOBTag, Helper.oUser.UserId, false);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTags.Add(new DropDownReturn { Value = dt.Rows[i]["TagName"].ToString(), Text = dt.Rows[i]["TagName"].ToString() });
            }
            return Json(new { Data = lstTags }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FilterRreviewMatchCandidates(List<FilterData> filters)
        {
            UserSessionFilterEntity filtermodel = new UserSessionFilterEntity();
            foreach (var item in filters)
            {
                var valLst = item.FilterValue.Split(',').ToList();
                if (item.FieldName == "OrderByColumn")
                    filtermodel.OrderByColumn = item.FilterValue;
                else if (item.FieldName == "TopMatchCandidate")
                    filtermodel.TopMatchCandidate = Convert.ToBoolean(item.FilterValue);
                else if (item.FieldName == "CountryGroup")
                    filtermodel.CountryGroupId = Convert.ToInt32(item.FilterValue);
                else if (item.FieldName == "NumberOfRecordsPerPage")
                    filtermodel.NumberOfRecordsPerPage = Convert.ToInt32(item.FilterValue);
                else if (item.FieldName == "ConfidenceCode")
                    filtermodel.ConfidenceCode = item.FilterValue;
                else if (item.FieldName == "Tag")
                    filtermodel.Tag = item.FilterValue;
            }
            filtermodel.UserId = Helper.oUser.UserId;
            filtermodel.ConfidenceCode = filtermodel.ConfidenceCode == null ? "ALL" : filtermodel.ConfidenceCode;
            filtermodel.NumberOfRecordsPerPage = filtermodel.NumberOfRecordsPerPage == 0 ? 50 : filtermodel.NumberOfRecordsPerPage;
            int totalCount = 0;
            int pagevalue = filtermodel.NumberOfRecordsPerPage;

            int currentPageIndex = 1;

            ViewBag.CountryGroup = filtermodel.CountryISOAlpha2Code;
            CompanyFacade companyFac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            DataTable dtReview = new DataTable();
            // if country is selected than find data from the database. 
            if (filtermodel.CountryGroupId == 0)
            {
                filtermodel.CountryGroupId = Models.CountryGroupModel.GetCountry(this.CurrentClient.ApplicationDBConnectionString).FirstOrDefault().GroupId;
            }
            if (filtermodel.CountryGroupId > 0)
            {
                dtReview = companyFac.GetReviewAllData(filtermodel.TopMatchCandidate, Convert.ToInt32(filtermodel.CountryGroupId), Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", filtermodel.Tag, !string.IsNullOrEmpty(filtermodel.ConfidenceCode) ? filtermodel.ConfidenceCode : "0", Helper.oUser.UserId, filtermodel.OrderByColumn, currentPageIndex, filtermodel.NumberOfRecordsPerPage, out totalCount);
                SessionHelper.ListMatchEntity = Newtonsoft.Json.JsonConvert.SerializeObject(ConvertDataTable<MatchEntity>(dtReview));
            }
            else
            {
                filtermodel.TopMatchCandidate = true;
            }

            SessionHelper.pagevalueReviewData = pagevalue;
            SessionHelper.TopMatchReviewData = filtermodel.TopMatchCandidate;
            SessionHelper.CountryGroupId = Convert.ToInt32(filtermodel.CountryGroupId);
            SessionHelper.ConfidenceCode = filtermodel.ConfidenceCode;
            SessionHelper.OrderByColumnReviewData = filtermodel.OrderByColumn;
            SessionHelper.LOBTag = filtermodel.LOBTag;
            SessionHelper.Tag = filtermodel.Tag;
            SessionHelper.CurrentPageIndex = filtermodel.NumberOfRecordsPerPage;

            //Get Paged Review Data List
            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dtReview.AsDynamicEnumerable(), currentPageIndex, filtermodel.NumberOfRecordsPerPage, totalCount);

            return PartialView("_Index", pagedProducts);
        }
        #endregion
    }
}