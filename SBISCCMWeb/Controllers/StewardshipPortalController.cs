using ExcelDataReader;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Models.PreviewMatchData.Main;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, DandBLicenseEnabled, AllowDataStewardshipLicense]
    public class StewardshipPortalController : BaseController
    {
        #region "Match Data"
        [HttpGet, Authorize]
        public ActionResult Index(int? page, int? sortby, int? sortorder, int? pagevalue, int CompanyMatch = 0, int CityName = 0, int StreetNo = 0, int StateName = 0, int StreetName = 0, int PostalCode = 0, int Telephone = 0, int MatchGrade = 0, string minConfidentCode = "0", string Command = null, bool SelectTopMatch = false, string MatchItemID = null)
        {
            // Clear WorkQueue  for Data
            StewUserActivityCloseWindow();

            Helper.IsApprovalScreen = false;
            // Check if User allow Enable2state and IsApproval screen or not.
            if (!Helper.Enable2StepUpdate)
                Helper.Approve = true;

            if (Helper.Enable2StepUpdate && !Helper.IsApprovalScreen)
                Helper.Approve = false;

            if (Helper.IsApprovalScreen)
                Helper.Approve = true;

            // Set User id in Session.
            if (Request.IsAjaxRequest())
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
                StewardshipPortalModel model = new StewardshipPortalModel();

                // Get user Detail by UserId.
                UsersModel Users = new UsersModel();
                // Get Login Detail by the userId
                Users.objUsers = fac.StewUserLogIn(Helper.oUser.EmailAddress, null, true);


                if (!string.IsNullOrEmpty(SessionHelper.pagevalueStewData) && pagevalue == null)
                    pagevalue = Convert.ToInt32(SessionHelper.pagevalueStewData);

                if (!string.IsNullOrEmpty(SessionHelper.pageNumberStewData) && page == null)
                    page = Convert.ToInt32(SessionHelper.pageNumberStewData);

                if (pagevalue == null || Convert.ToInt32(pagevalue) == 0)
                {
                    SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    pagevalue = Convert.ToInt32(sfac.GetDefaultPageSize(Helper.oUser.UserId, "MatchData"));
                    pagevalue = pagevalue == 0 ? 10 : pagevalue;
                }
                if (!(sortby.HasValue && sortby.Value > 0))
                    sortby = 1;

                if (!(sortorder.HasValue && sortorder.Value > 0))
                    sortorder = 1;

                int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
                int totalCount = 0;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                pagevalue = pagevalue < 5 ? 5 : pagevalue;
                int pageNumber = (page ?? 1);
                SessionHelper.pagevalueStewData = Convert.ToString(pagevalue);
                SessionHelper.pageNumberStewData = Convert.ToString(pageNumber);
                ViewBag.SortBy = sortby;
                ViewBag.SortOrder = sortorder;
                ViewBag.pagevalue = pagevalue;
                ViewBag.pageNumber = pageNumber;
                // Find Company by userid and according to page no and page size wise.
                Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, currentPageIndex, Convert.ToInt32(pagevalue), out totalCount, Helper.IsApprovalScreen);
                model.Companies = tuplecompany.Item1;
                SessionHelper.QueueMessage = tuplecompany.Item2;
                model.Companies = SetOriginalMatchCount(model.Companies);
                if (Helper.IsApprovalScreen && model.Companies != null)
                    model.Companies = FinishedLoadCompanies(model.Companies);

                //Get session Filter data from the database and set in tempdata.
                SessionHelper.TotalCountStew = Convert.ToString(totalCount);
                SessionHelper.TempCompanies = JsonConvert.SerializeObject(model.Companies);
                IPagedList<CompanyEntity> pagedCompany = new StaticPagedList<CompanyEntity>(model.Companies.ToList(), currentPageIndex, Convert.ToInt32(pagevalue), totalCount);
                return PartialView("_Index", pagedCompany);
            }
            else
            {
                SessionHelper.Stew_IsFirstTimeFilter = true;
            }
            return View("Index");
        }
        [HttpPost, RequestFromSameDomain]
        [Route("StewardshipPortal/GetFilteredCompanyList/{page?}/{sortby?}/{sortorder?}/{mode?}/{CompanyMatch?}/{CityName?}/{StreetNo?}/{StateName?}/{StreetName?}/{PostalCode?}/{Telephone?}/{MatchGrade?}/{minConfidentCode?}/{SelectTopMatch?}/{MatchItemID?}/{isPaging?}")]
        public ActionResult GetFilteredCompanyList(int? page, int? sortby, int? sortorder, int? pagevalue, int CompanyMatch = 0, int CityName = 0, int StreetNo = 0, int StateName = 0, int StreetName = 0, int PostalCode = 0, int Telephone = 0, int MatchGrade = 0, string minConfidentCode = "0", string Command = null, bool SelectTopMatch = false, string MatchItemID = null, bool isPaging = false)
        {

            UserSessionFacade ufac = new UserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            // Set page no and Page size for the pagination.
            if (page == null && !string.IsNullOrEmpty(SessionHelper.pageNumberStewData))
                page = Convert.ToInt32(SessionHelper.pageNumberStewData);

            if (!string.IsNullOrEmpty(SessionHelper.pagevalueStewData))
                pagevalue = Convert.ToInt32(SessionHelper.pagevalueStewData);

            if (pagevalue == null || Convert.ToInt32(pagevalue) == 0)
            {
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                pagevalue = Convert.ToInt32(sfac.GetDefaultPageSize(Helper.oUser.UserId, "MatchData"));
                pagevalue = pagevalue == 0 ? 10 : pagevalue;
            }

            int pageNumber = (page ?? 1);
            StewardshipPortalModel model = new StewardshipPortalModel();
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            if (!string.IsNullOrEmpty(SessionHelper.TotalCountStew))
                totalCount = Convert.ToInt32(SessionHelper.TotalCountStew);

            int currentPageIndex = page.HasValue ? page.Value : 1;
            pagevalue = pagevalue.HasValue ? pagevalue.Value : 10;
            SessionHelper.pageNumberStewData = Convert.ToString(pageNumber);
            SessionHelper.pagevalueStewData = Convert.ToString(pagevalue);
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pagevalue = pagevalue;
            ViewBag.pagevalue = pagevalue;
            ViewBag.CompanyMatch = CompanyMatch;
            ViewBag.CityName = CityName;
            ViewBag.StreetNo = StreetNo;
            ViewBag.StateName = StateName;
            ViewBag.StreetName = StreetName;
            ViewBag.PostalCode = PostalCode;
            ViewBag.Telephone = Telephone;
            ViewBag.MatchGrade = MatchGrade;
            ViewBag.minConfidentCode = minConfidentCode;
            ViewBag.Command = Command;
            ViewBag.SelectTopMatch = SelectTopMatch;
            ViewBag.MatchItemID = MatchItemID;

            if (minConfidentCode == "")
                minConfidentCode = "0";

            if (minConfidentCode.Contains('.'))
                minConfidentCode = minConfidentCode.Substring(0, minConfidentCode.IndexOf('.'));

            // Check and filter data from the session Filter.
            if (!string.IsNullOrEmpty(SessionHelper.TempCompanies) && !isPaging)
            {
                model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.TempCompanies);
            }
            else
            {
                Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, currentPageIndex, Convert.ToInt32(pagevalue), out totalCount, Helper.IsApprovalScreen);
                model.Companies = tuplecompany.Item1;
                SessionHelper.QueueMessage = tuplecompany.Item2;
                SessionHelper.TempCompanies = JsonConvert.SerializeObject(model.Companies);
                SessionHelper.TotalCountStew = Convert.ToString(totalCount);
            }

            if (sortorder != null && sortby != null)
            {
                model.Companies = SortData(model.Companies, Convert.ToInt32(sortorder), Convert.ToInt32(sortby));
            }

            List<CompanyEntity> Companies = new List<CompanyEntity>();
            // Set Company matches for the particular company wise.
            model.Companies = SetOriginalMatchCount(model.Companies);
            foreach (var co in model.Companies)
            {
                List<MatchEntity> matches = (from m in co.Matches
                                             where m.DnBConfidenceCode >= Convert.ToInt32(minConfidentCode)
                                             where ((CompanyMatch > 0) ? SetMatchGradeFilterString(CompanyMatch).Contains(m.MGCompanyName.ToString()) : m.MGCompanyName.Contains(m.MGCompanyName))
                                             where ((StreetNo > 0) ? SetMatchGradeFilterString(StreetNo).Contains(m.MGStreetNo.ToString()) : m.MGStreetNo.Contains(m.MGStreetNo))
                                             where ((StreetName > 0) ? SetMatchGradeFilterString(StreetName).Contains(m.MGStreetName.ToString()) : m.MGStreetName.Contains(m.MGStreetName))
                                             where ((CityName > 0) ? SetMatchGradeFilterString(CityName).Contains(m.MGCity.ToString()) : m.MGCity.Contains(m.MGCity))
                                             where ((StateName > 0) ? SetMatchGradeFilterString(StateName).Contains(m.MGState.ToString()) : m.MGState.Contains(m.MGState))
                                             where ((Telephone > 0) ? SetMatchGradeFilterString(Telephone).Contains(m.MGTelephone.ToString()) : m.MGTelephone.Contains(m.MGTelephone))
                                             where ((PostalCode > 0) ? SetMatchGradeFilterString(PostalCode).Contains(m.MGZipCode.ToString()) : m.MGZipCode.Contains(m.MGZipCode))
                                             select m).ToList<MatchEntity>();
                co.Matches = matches;
                //if (matches.Count == 0)
                //{
                //    Companies.Add(co);
                //}
            }
            //if (Companies != null && Companies.Any())
            //{
            //    foreach (var company in Companies)
            //    {
            //        model.Companies.Remove(company);
            //    }
            //}
            // if select top 1 match records than select and set first records of each company as selected.
            if (SelectTopMatch)
            {
                model.Companies.ForEach(x => x.Matches.RemoveRange(1, x.Matches.Count - 1));
            }

            if (!string.IsNullOrEmpty(Command))
            {
                if (Command == MatchDataLang.lblUpdate)
                {
                    // if Match update than Match set per company.
                    model.Companies.ForEach(x => x.MatchesFiltered = x.Matches);
                    ViewBag.Message = "No record updated.";
                    if (model != null && model.Companies.Any())
                    {
                        foreach (CompanyEntity company in model.Companies)
                        {
                            if (company.RejectCompany)
                            {
                                RejectMatched(company);
                            }
                            else if (company.SelectedMatchCount > 0)
                            {
                                AcceptedMatched(company);
                            }
                        }
                    }
                }

                Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, pageNumber, Convert.ToInt32(pagevalue), out totalCount, Helper.IsApprovalScreen);
                model.Companies = tuplecompany.Item1;
                SessionHelper.QueueMessage = tuplecompany.Item2;
                if (totalCount <= 0 && model.Companies.Count == 0 && pageNumber > 1)
                {
                    tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, pageNumber - 1, Convert.ToInt32(pagevalue), out totalCount, Helper.IsApprovalScreen);
                    model.Companies = tuplecompany.Item1;
                    SessionHelper.QueueMessage = tuplecompany.Item2;
                    SessionHelper.pageNumberStewData = Convert.ToString(pageNumber - 1);
                }
                model.Companies = SetOriginalMatchCount(model.Companies);
                SessionHelper.TempCompanies = JsonConvert.SerializeObject(model.Companies);
                SessionHelper.TotalCountStew = Convert.ToString(totalCount);
                if (SelectTopMatch)
                {
                    model.Companies.ForEach(x => x.Matches.RemoveRange(1, x.Matches.Count - 1));
                }
            }

            if (totalCount <= 0 && model.Companies.Count == 0 && pageNumber > 1)
            {
                totalCount = Convert.ToInt32(pagevalue);
            }
            // Set model of Company entity to pass this model to view.
            IPagedList<CompanyEntity> pagedCompany = new StaticPagedList<CompanyEntity>(model.Companies.ToList(), pageNumber, Convert.ToInt32(pagevalue), totalCount);
            string ChildViewName = "_Index";
            string ViewName = "Index";
            if (Request.IsAjaxRequest())
                return PartialView(ChildViewName, pagedCompany);

            return View(ViewName, pagedCompany);
        }
        // Data Sort by sort order and sort by
        public List<CompanyEntity> SortData(List<CompanyEntity> lstcompany, int sortorder, int sortby)
        {
            // sort order 1 for ascending order and 2 for descending order.
            switch (sortby)
            {
                case 1:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.SrcRecordId).ToList() : lstcompany.OrderByDescending(x => x.SrcRecordId).ToList();
                    break;
                case 2:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.CompanyName).ToList() : lstcompany.OrderByDescending(x => x.CompanyName).ToList();
                    break;
                case 3:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.Address).ToList() : lstcompany.OrderByDescending(x => x.Address).ToList();
                    break;
                case 4:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.City).ToList() : lstcompany.OrderByDescending(x => x.City).ToList();
                    break;
                case 5:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.State).ToList() : lstcompany.OrderByDescending(x => x.State).ToList();
                    break;
                case 6:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.PostalCode).ToList() : lstcompany.OrderByDescending(x => x.PostalCode).ToList();
                    break;
                case 7:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.CountryISOAlpha2Code).ToList() : lstcompany.OrderByDescending(x => x.CountryISOAlpha2Code).ToList();
                    break;
                case 8:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.PhoneNbr).ToList() : lstcompany.OrderByDescending(x => x.PhoneNbr).ToList();
                    break;
                case 9:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.MatchCount).ToList() : lstcompany.OrderByDescending(x => x.MatchCount).ToList();
                    break;
            }
            return lstcompany;
        }
        // If company select as match than change in model and make company as select or reject.
        public List<CompanyEntity> FinishedLoadCompanies(List<CompanyEntity> lstCompany)
        {
            lstCompany.ForEach(x =>
            {
                x.MatchesFiltered = x.Matches;
                x.RejectCompany = x.RejectAllMatches;
                if (x.MatchesFiltered.Any(c => c.IsSelected))
                    x.SelectedMatchCount = 1;
            });
            return lstCompany;
        }
        #endregion

        #region "Match Details Popup "
        //Open Matched Item Detail View with all Parameter pass with serialization form and set next and previous data to manage next and previous functionality.
        public ActionResult cShowMatchedItesDetailsView(string Parameters)
        {
            string id = string.Empty, childButtonId = string.Empty, dataNext = string.Empty, dataPrev = string.Empty, count = string.Empty; bool IsPartialView = false;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                id = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                childButtonId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                dataNext = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                dataPrev = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                count = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
                ViewBag.AdditionalFields = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1);
                IsPartialView = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 6, 1));
            }
            MatchEntity Match = new MatchEntity();
            ViewBag.dataNext = dataNext;
            ViewBag.dataPrev = dataPrev;
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            StewardshipPortalModel model = new StewardshipPortalModel();
            ViewBag.SelectData = "StewardshipPortal";
            if (!string.IsNullOrEmpty(SessionHelper.TempCompanies))
            {
                model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.TempCompanies);
            }
            else
            {
                Helper.oUser.UserId = Convert.ToInt32(User.Identity.GetUserId());
                int page = Convert.ToInt32(SessionHelper.pageNumberStewData);
                int pagevalue = Convert.ToInt32(SessionHelper.pagevalueStewData);
                int totalCount = 0;
                Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, page == 0 ? 1 : page, pagevalue == 0 ? 10 : pagevalue, out totalCount, Helper.IsApprovalScreen);
                model.Companies = tuplecompany.Item1;
            }
            model.Companies = SetOriginalMatchCount(model.Companies);
            // Set All Company details in session 
            CompanyEntity company = new CompanyEntity();
            company = model.Companies.Where(x => x.InputId == Convert.ToInt32(id)).FirstOrDefault();
            Match = company.Matches.Where(x => x.DnBDUNSNumber == childButtonId).FirstOrDefault();
            try
            {
                ViewBag.NextToNextDUNS = dataNext != "" ? company.Matches.SkipWhile(p => p.DnBDUNSNumber != dataNext).ElementAt(1).DnBDUNSNumber : company.Matches.SkipWhile(p => p.DnBDUNSNumber != childButtonId).ElementAt(1).DnBDUNSNumber;
            }
            catch
            {
                ViewBag.NextToNextDUNS = "";
            }
            try
            {
                ViewBag.PrevToPrevDUNS = dataPrev != "" ? company.Matches.TakeWhile(p => p.DnBDUNSNumber != dataPrev).LastOrDefault().DnBDUNSNumber : company.Matches.TakeWhile(p => p.DnBDUNSNumber != childButtonId).LastOrDefault().DnBDUNSNumber;
            }
            catch
            {
                ViewBag.PrevToPrevDUNS = "";

            }
            var IsSelected = company != null ? company.Matches.Where(x => x.DnBDUNSNumber == Match.DnBDUNSNumber).Select(x => x.IsSelected).FirstOrDefault() : false;
            Match.IsSelected = IsSelected;
            Tuple<MatchEntity, CompanyEntity> tuple = new Tuple<MatchEntity, CompanyEntity>(new MatchEntity(), new CompanyEntity());
            if (company != null)
            {
                Helper.CompanyName = company.CompanyName == "" ? "Null" : company.CompanyName;
                Helper.Address = company.Address == "" ? "Null" : company.Address;
                Helper.Address1 = company.Address1 == "" ? "Null" : company.Address1;
                Helper.City = company.City == "" ? "Null" : company.City;
                Helper.State = company.State == "" ? "Null" : company.State;
                Helper.PhoneNbr = company.PhoneNbr == "" ? "Null" : company.PhoneNbr;
                Helper.Zip = company.PostalCode == "" ? "" : company.PostalCode;

                tuple = new Tuple<MatchEntity, CompanyEntity>(Match, company);
            }

            //when Popup is already open and click on next previous at that time we reload just partial view
            if (!IsPartialView)
            {
                return PartialView("_MatchedItemDetailView", tuple);
            }
            else
            {
                return PartialView("_MatchDetails", tuple);
            }

        }
        [AllowAnonymous]
        public ActionResult GetLayoutPopUp()
        {
            return PartialView("~/Views/Shared/_LayoutPopup.cshtml");
        }
        #endregion

        #region "Other Method"
        private string[] SetMatchGradeFilterString(int SelectionIndex)
        {
            // Set Match Grade For Filtration process.
            string[] strArrayAll = { "A", "B", "F", "Z" };
            string[] strArraySame = { "A" };
            string[] strArraySimilar = { "A", "B" };
            string[] result;
            switch (SelectionIndex)
            {
                case 1:
                    result = strArraySame;
                    break;
                case 2:
                    result = strArraySimilar;
                    break;
                default:
                    result = strArrayAll;
                    break;
            }
            return result;
        }
        // Accept matches for Company and update company with selected matches.
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public void AcceptLCMMatches(string id, string MatchSeqence)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            StewardshipPortalModel model = new StewardshipPortalModel();
            if (!string.IsNullOrEmpty(SessionHelper.TempCompanies))
            {
                model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.TempCompanies);
            }
            if (model.Companies.Any() && !string.IsNullOrEmpty(id))
            {
                foreach (var company in model.Companies)
                {
                    if (company.InputId == Convert.ToInt32(id))
                    {
                        if (MatchSeqence.ToLower() == "rejectall")
                        {
                            company.Matches.ForEach(x => x.IsSelected = false);
                            company.SelectedMatchCount = 0;
                            company.RejectCompany = true;
                        }
                        else if (MatchSeqence.ToLower() == "unrejectall")
                        {
                            company.RejectCompany = false;
                        }
                        else
                        {
                            company.Matches.ForEach(x => x.IsSelected = false);
                            company.Matches[Convert.ToInt32(MatchSeqence) - 1].IsSelected = true;
                            company.SelectedMatchCount = 1;
                            company.RejectCompany = false;
                        }
                    }
                }
            }
            SessionHelper.TempCompanies = JsonConvert.SerializeObject(model.Companies);
            Helper.IsDirty = true;
        }
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public void RejectLCMMatches(string id, string MatchSeqence)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            StewardshipPortalModel model = new StewardshipPortalModel();
            //We store all matches for Company in temp data and when user will select or deselect match at that time fill old matches to list
            //and get the id from the list to PreviousId and then remove from the list and add new id into list.
            if (!string.IsNullOrEmpty(SessionHelper.TempCompanies))
            {
                model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.TempCompanies);
            }
            // remove selected matches for rejection.
            if (model.Companies.Any() && !string.IsNullOrEmpty(id))
            {
                foreach (var company in model.Companies)
                {
                    if (company.InputId == Convert.ToInt32(id))
                    {
                        company.Matches[Convert.ToInt32(MatchSeqence) - 1].IsSelected = false;
                        company.SelectedMatchCount = 0;
                        company.StewardshipNotes = "";
                        company.Matches[Convert.ToInt32(MatchSeqence) - 1].StewardshipNotes = "";
                    }
                }
            }
            SessionHelper.TempCompanies = JsonConvert.SerializeObject(model.Companies);
            Helper.IsDirty = false;
        }
        public void AcceptedMatched(CompanyEntity company)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            List<MatchEntity> SelectedMatches = new List<MatchEntity>();
            {
                if (!company.RejectCompany)
                {
                    foreach (MatchEntity match in company.Matches)
                    {
                        if (match.IsSelected)
                        {
                            SelectedMatches.Add(match);
                        }
                    }
                }
            }
            if (SelectedMatches.Count > 0)
            {
                try
                {
                    // update company matches to database.
                    fac.AcceptLCMMatches(SelectedMatches, Helper.oUser.UserId, Helper.Approve);
                    ViewBag.Message = Helper.UpdateMessage;
                }
                catch {
                    //Empty catch block to stop from breaking
                }
            }
        }
        public void RejectMatched(CompanyEntity company)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            List<CompanyEntity> rejectCompanies = new List<CompanyEntity>();
            {
                if (company.RejectCompany)
                {
                    rejectCompanies.Add(company);
                }
                else
                {
                    int mCount = (from m in company.Matches where m.IsSelected == true select m).Count();
                    if (mCount == 0)
                    {
                        rejectCompanies.Add(company);
                    }
                }
            }
            if (rejectCompanies.Count > 0)
            {
                try
                {
                    // update company matches to database.
                    fac.RejectLCMMatches(rejectCompanies, Helper.oUser.UserId, Helper.Approve);
                    ViewBag.Message = Helper.UpdateMessage;
                }
                catch
                {
                    //Empty catch block to stop from breaking}
                }
            }
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteSessionFilter()
        {
            // Delete Session filter for user.
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            fac.DeleteUserSessionFilter(Helper.oUser.UserId);
            return new JsonResult { Data = "success" };
        }
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetNextMatchDetailRecord(int SrcId)
        {
            // Get next matches detail for company in match item detail view.
            StewardshipPortalModel model = new StewardshipPortalModel();
            if (!string.IsNullOrEmpty(SessionHelper.TempCompanies))
            {
            }
            return new JsonResult { Data = CommonMessagesLang.msgSuccess };
        }
        #endregion

        #region "Open Search Note"
        // Get Stewardship note for match. 
        [HttpGet]
        public ActionResult OpenSearchData(int InputId, int count, string Notes)
        {
            string Note = "";
            if (Helper.IsApprovalScreen == false)
            {
                StewardshipPortalModel model = new StewardshipPortalModel();
                if (!string.IsNullOrEmpty(SessionHelper.TempCompanies))
                {
                    model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.TempCompanies);
                }
                Note = Convert.ToString(model.Companies.Where(x => x.InputId == InputId).FirstOrDefault().StewardshipNotes);
                if (model.Companies.Count > 0)
                {
                    foreach (var item in model.Companies)
                    {
                        if (item.InputId == InputId)
                        {
                            if (count == -1)
                            {
                                Note = item.StewardshipNotes;
                            }
                            else
                            {
                                int i = 1;
                                foreach (var match in item.Matches)
                                {
                                    if (i == count)
                                    {
                                        Note = match.StewardshipNotes;
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Note = Notes;
            }
            ViewBag.StewardShipNote = Note;
            ViewBag.InputId = InputId;
            ViewBag.MatchCount = count;
            return PartialView("_StewardshipNotes");
        }
        // Save stewardship note detail in database.
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain, ValidateInput(true)]
        public ActionResult OpenSearchData(string StewardshipNotes, int InputId, int Count)
        {
            if (Helper.IsApprovalScreen == false)
            {
                StewardshipPortalModel model = new StewardshipPortalModel();
                if (!string.IsNullOrEmpty(SessionHelper.TempCompanies))
                {
                    model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.TempCompanies);
                }
                if (model.Companies.Count > 0)
                {
                    foreach (var item in model.Companies)
                    {
                        if (item.InputId == InputId)
                        {
                            if (Count == -1)
                            {
                                item.StewardshipNotes = StewardshipNotes;
                            }
                            else
                            {
                                item.StewardshipNotes = "";
                                int i = 1;
                                foreach (var match in item.Matches)
                                {
                                    if (i == Count)
                                    {
                                        match.StewardshipNotes = StewardshipNotes;
                                        item.StewardshipNotes = StewardshipNotes;
                                    }
                                    else
                                    {
                                        match.StewardshipNotes = "";
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                }
                SessionHelper.TempCompanies = JsonConvert.SerializeObject(model.Companies);
            }
            ViewBag.StewardShipNote = StewardshipNotes;
            ViewBag.CloseAlert = "<script type='text/javascript'>$(document).ready(function(){parent.backToparent2();});</script>";
            return PartialView("_StewardshipNotes");
        }
        #endregion

        #region "Window Close Event"
        public void StewUserActivityCloseWindow()
        {
            // Set window close event and manage changes discard for page.
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            fac.StewUserActivityCloseWindow(Helper.oUser.UserId);
        }
        #endregion

        public List<CompanyEntity> SetOriginalMatchCount(List<CompanyEntity> lstCompanies)
        {
            if (lstCompanies != null && lstCompanies.Any())
            {
                lstCompanies.ForEach(x => x.OriginalMatchCount = x.MatchCount);
            }
            return lstCompanies;
        }

        #region Set session for collapse and expand
        public void SetSession(bool IsExpand)
        {
            Helper.IsExpand = IsExpand;
        }
        #endregion

        #region Google Map 
        // Google map popup on clicking the location from Match Data
        public ActionResult GoogleMapPopUp(string Parameters)
        {
            string id = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                id = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
            }
            StewardshipPortalModel model = new StewardshipPortalModel();
            CompanyEntity Company = new CompanyEntity();
            if (!string.IsNullOrEmpty(SessionHelper.TempCompanies))
            {
                model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.TempCompanies);
            }
            // remove selected matches for rejection.
            if (model.Companies.Any() && !string.IsNullOrEmpty(id))
            {
                Company = model.Companies.Where(x => x.SrcRecordId == id).FirstOrDefault();
            }
            if (Company == null)
            {
                Company = new CompanyEntity();
            }

            return View(Company);
        }
        #endregion

        #region Reject All functionality 
        [HttpGet]
        public ActionResult RejectAllRecords(bool IsMatchData)
        {
            ViewBag.IsMatchData = IsMatchData;
            return View();
        }
        [RequestFromAjax, RequestFromSameDomain, ValidateInput(false)]
        public JsonResult RejectAllRecords(string SrcRecordId = null, string City = null, string State = null, string ImportPorcess = null, string ConfidenceCode = null, string CountryISOAlpha2Code = null, int CountryGroupId = 0, string Tag = null, bool CityExactMatch = false, bool StateExactMatch = false, string GetCountsOnly = null, bool Purge = false, bool IsMatchData = false)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            int count = 0;
            bool IsRecordRejected = false;
            if (string.IsNullOrEmpty(ConfidenceCode))
            {
                ConfidenceCode = "0";
            }
            if (IsMatchData)
            {
                //if GetCountsOnly is true mean only get count of affected data and if false than reject affected data.
                count = fac.StewRejectAllLCMRecords(CountryISOAlpha2Code, CountryGroupId, Tag, ImportPorcess, Convert.ToInt32(ConfidenceCode), SrcRecordId, City, State, CityExactMatch, StateExactMatch, Helper.oUser.UserId, Convert.ToBoolean(GetCountsOnly), Purge);
            }
            else
            {
                count = fac.PurgeAllBIDRecords(CountryISOAlpha2Code, CountryGroupId, Tag, ImportPorcess, SrcRecordId, City, State, CityExactMatch, StateExactMatch, Helper.oUser.UserId, Convert.ToBoolean(GetCountsOnly));
            }
            if (Convert.ToBoolean(GetCountsOnly))
            {
                if (count > 0)
                {
                    return Json(new { result = true, message = CommonMessagesLang.msgTotal + " " + count + " " + CommonMessagesLang.msgRecordsAffected }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = CleanDataLang.msgNoRecords }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { result = true, message = CommonMessagesLang.msgDataRejected }, JsonRequestBehavior.AllowGet);
            }
        }




        //purges a single record from the UI (right - click UI option)
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult StewPurgeSingleRecord(string Parameters)
        {
            string InputId = string.Empty, SrcRecordId = string.Empty, Queue = string.Empty;
            if (!string.IsNullOrEmpty(Parameters)) ;
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                InputId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                SrcRecordId = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                Queue = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
            }
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            fac.StewPurgeSingleRecord(InputId, SrcRecordId, Helper.oUser.UserId, Queue);
            return new JsonResult { Data = DandBSettingLang.msgCommanPurgeMessage };
        }

        #endregion

        #region Save default Page Size
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateInput(true)]
        public JsonResult SaveDefaultPageSize(string Parameters)
        {
            //set default page size for match-data page 
            try
            {
                int pageSize = 0; string Section = string.Empty;
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    pageSize = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                    Section = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                }
                if (pageSize > 0)
                {
                    SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    fac.UpdateDefaultPageSize(Helper.oUser.UserId, Section, pageSize);
                }
                return new JsonResult { Data = DandBSettingLang.msgSettingUpdate.ToString() };
            }
            catch (Exception)
            {
                return new JsonResult { Data = DandBSettingLang.msgSettingNotUpdate.ToString() };
            }
        }
        #endregion

        #region Reject From File
        public ActionResult RejectFromFile(bool IsPurgeData)
        {
            DataTable dt = new DataTable();
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtRejectData = new DataTable();
            dtRejectData = sfac.GetRejectPurgeColumnsName();
            List<string> columnName = new List<string>();
            if (dtRejectData.Rows.Count > 0)
            {
                for (int k = 0; k < dtRejectData.Rows.Count; k++)  //loop through the columns. 
                {
                    columnName.Add(Convert.ToString(dtRejectData.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            ViewBag.IsPurgeData = IsPurgeData;
            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), 1, 100000, 0);
            return PartialView("_rejectImportFile", pagedProducts);
        }
        [HttpPost]
        public ActionResult BindRejectMapping(HttpPostedFileBase file)
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            DataTable dt = new DataTable();

            //Get Import File Column Name to fill in dropdown


            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtRejectData = new DataTable();
            dtRejectData = sfac.GetRejectPurgeColumnsName();

            if (file != null && CommonMethod.CheckFileType(".xls,.xlsx,", file.FileName))
            {
                string path = string.Empty;
                string directory = Server.MapPath("~/Upload/UploadCommandFile");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                FileInfo oFileInfo = new FileInfo(file.FileName);
                string fileExtension = oFileInfo.Extension;
                string fileName = System.DateTime.Now.Ticks + fileExtension;
                path = Path.Combine(directory, Path.GetFileName(fileName));
                file.SaveAs(path);
                string extension = Path.GetExtension(file.FileName);
                // Read excel file & set column header in datatable
                try
                {
                    IExcelDataReader reader = null;
                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                    if (extension.Equals(".xls"))
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else if (extension.Equals(".xlsx"))
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    if (reader != null)
                    {
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                        };
                        //Fill DataSet
                        DataSet content = reader.AsDataSet(conf);
                        dt = content.Tables[0];
                    }
                    Session["Stew_Data"] = dt;
                    stream.Close();
                }
                catch (Exception)
                {
                    //Empty catch block to stop from breaking
                }
                if (dt != null && dt.Columns != null && dt.Columns.Count > 0)
                {
                    lstAllFilter.Add(new SelectListItem { Value = "0", Text = "-Select-" });
                    int i = 0;
                    foreach (DataColumn c in dt.Columns)
                    {
                        lstAllFilter.Add(new SelectListItem { Value = (i + 1).ToString(), Text = Convert.ToString(c.ColumnName) });
                        i++;

                    }
                }
                System.IO.File.Delete(path);

            }
            List<string> columnName = new List<string>();
            if (dtRejectData.Rows.Count > 0)
            {
                for (int k = 0; k < dtRejectData.Rows.Count; k++)  //loop through the columns. 
                {
                    columnName.Add(Convert.ToString(dtRejectData.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            ViewBag.ExternalColumn = lstAllFilter;

            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(new List<dynamic>(), 1, 10000, 0);

            return PartialView("_bindRejectMapping", pagedProducts);
        }
        [HttpPost]
        public JsonResult RejectData(string[] OrgColumnName, string[] ExcelColumnName, bool IsPurgeData)
        {
            string Message = string.Empty;
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = new DataTable();
            DataTable dtOrgColumns = new DataTable();
            DataTable dtColumns = new DataTable();

            if (Session["Stew_Data"] != null)
            {
                dt = Session["Stew_Data"] as DataTable;

                //Get Get Country groups Columns Name
                dtOrgColumns = sfac.GetRejectPurgeColumnsName();
                dtColumns.Columns.Add("Tablecolumn");
                dtColumns.Columns.Add("Excelcolumn");
                DataRow dr = dtColumns.NewRow();
                for (int i = 0; i < OrgColumnName.Length; i++)
                {
                    if (Convert.ToString(OrgColumnName[i]) != "-Select-")
                    {
                        dr = dtColumns.NewRow();
                        dr["Tablecolumn"] = Convert.ToString(ExcelColumnName[i]);
                        dr["Excelcolumn"] = Convert.ToString(OrgColumnName[i]);
                        dtColumns.Rows.Add(dr);
                    }
                }
                try
                {
                    //bulk insert new records
                    bool IsDataInsert = BulkInsert(dt, dtColumns, IsPurgeData, out Message);
                }
                catch (Exception ex)
                {
                    Message = CommonMessagesLang.msgCommanEnableFileImport;
                }
                return new JsonResult { Data = Message };
            }
            else
            {
                return new JsonResult { Data = "Something went wrong! Error Occured." };
            }

        }
        private bool BulkInsert(DataTable dt, DataTable dtColumns, bool IsPurgeData, out string msg)
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
                    bulkCopy.DestinationTableName = "ext.StgImportDataForRejectPurge";
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        trans.Commit();
                        DataInsert = true;
                        SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                        string Message = sfac.RejectPurgeDataFromImport(Helper.oUser.UserId, IsPurgeData);
                        if (IsPurgeData)
                        {
                            msg = DandBSettingLang.msgCommanPurgeMessage;
                        }
                        else
                        {
                            msg = DandBSettingLang.msgCommanRejectMessage;
                        }
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

        #region Accept From File
        public ActionResult AcceptFromFile(bool IsPurgeData)
        {
            DataTable dt = new DataTable();
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtAcceptData = new DataTable();
            dtAcceptData = sfac.GetImportDataForAcceptColumnsName();
            Session["dtAcceptData"] = dtAcceptData;
            List<string> columnName = new List<string>();
            if (dtAcceptData.Rows.Count > 0)
            {
                for (int k = 0; k < dtAcceptData.Rows.Count; k++)  //loop through the columns. 
                {
                    columnName.Add(Convert.ToString(dtAcceptData.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), 1, 100000, 0);
            return PartialView("_acceptImportFile", pagedProducts);
        }

        [HttpPost]
        public ActionResult BindAcceptFileMapping(HttpPostedFileBase file)
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            DataTable dt = new DataTable();


            //Get Import File Column Name to fill in dropdown
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtAcceptData = new DataTable();
            if (Session["dtAcceptData"] == null)
            {
                dtAcceptData = sfac.GetImportDataForAcceptColumnsName();
            }
            else
            {
                dtAcceptData = Session["dtAcceptData"] as DataTable;
            }

            if (file != null && CommonMethod.CheckFileType(".xls,.xlsx,", file.FileName))
            {
                string path = string.Empty;
                string directory = Server.MapPath("~/Upload/UploadAcceptFile");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                FileInfo oFileInfo = new FileInfo(file.FileName);
                string fileExtension = oFileInfo.Extension;
                string fileName = System.DateTime.Now.Ticks + fileExtension;
                path = Path.Combine(directory, Path.GetFileName(fileName));
                file.SaveAs(path);
                string extension = Path.GetExtension(file.FileName);
                // Read excel file & set column header in datatable
                try
                {
                    IExcelDataReader reader = null;
                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                    if (extension.Equals(".xls"))
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else if (extension.Equals(".xlsx"))
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    if (reader != null)
                    {
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                        };
                        //Fill DataSet
                        DataSet content = reader.AsDataSet(conf);
                        dt = content.Tables[0];
                    }
                    Session["Stew_Data"] = dt;
                    stream.Close();
                }
                catch (Exception)
                {
                    //Empty catch block to stop from breaking
                }
                if (dt != null && dt.Columns != null && dt.Columns.Count > 0)
                {
                    lstAllFilter.Add(new SelectListItem { Value = "0", Text = "-Select-" });
                    int i = 0;
                    foreach (DataColumn c in dt.Columns)
                    {
                        lstAllFilter.Add(new SelectListItem { Value = (i + 1).ToString(), Text = Convert.ToString(c.ColumnName) });
                        i++;
                    }
                }
                System.IO.File.Delete(path);

            }
            List<string> columnName = new List<string>();
            if (dtAcceptData.Rows.Count > 0)
            {
                for (int k = 0; k < dtAcceptData.Rows.Count; k++)  //loop through the columns. 
                {
                    columnName.Add(Convert.ToString(dtAcceptData.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            ViewBag.ExternalColumn = lstAllFilter;

            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(new List<dynamic>(), 1, 10000, 0);

            return PartialView("_bindAcceptMapping", pagedProducts);
        }
        public JsonResult AcceptDataFile(string[] OrgColumnName, string[] ExcelColumnName)
        {
            string Message = string.Empty;
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = new DataTable();
            DataTable dtOrgColumns = new DataTable();
            DataTable dtColumns = new DataTable();

            if (Session["Stew_Data"] != null)
            {
                dt = Session["Stew_Data"] as DataTable;
                //Get Get Country groups Columns Name
                if (Session["dtAcceptData"] == null)
                {
                    dtOrgColumns = sfac.GetImportDataForAcceptColumnsName();
                }
                else
                {
                    dtOrgColumns = Session["dtAcceptData"] as DataTable;
                }
                dtColumns.Columns.Add("Tablecolumn");
                dtColumns.Columns.Add("Excelcolumn");
                DataRow dr = dtColumns.NewRow();
                for (int i = 0; i < OrgColumnName.Length; i++)
                {
                    if (Convert.ToString(OrgColumnName[i]) != "-Select-")
                    {
                        dr = dtColumns.NewRow();
                        dr["Tablecolumn"] = Convert.ToString(ExcelColumnName[i]);
                        dr["Excelcolumn"] = Convert.ToString(OrgColumnName[i]);
                        dtColumns.Rows.Add(dr);
                    }
                }
                try
                {
                    //bulk insert new records
                    bool IsDataInsert = AcceptFileBulkInsert(dt, dtColumns, out Message);
                }
                catch (Exception ex)
                {
                    Message = CommonMessagesLang.msgCommanEnableFileImport;
                }
                return new JsonResult { Data = Message };
            }
            else
            {
                return new JsonResult { Data = "Something went wrong! Error Occured." };
            }
        }

        private bool AcceptFileBulkInsert(DataTable dt, DataTable dtColumns, out string msg)
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
                    bulkCopy.DestinationTableName = "ext.StgImportDataForAccept";
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        trans.Commit();
                        DataInsert = true;
                        SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                        string Message = sfac.AcceptLCMDataFromImport(Helper.oUser.UserId);
                        msg = Message;
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

        #region Reject Data From Page
        // Reject Data from Additional Actions dropdown menu
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts, ValidateInput(true)]
        public JsonResult RejectDataFromPage(string Parameters)
        {
            int CompanyMatch = 0, CityName = 0, StreetNo = 0, StateName = 0, StreetName = 0, PostalCode = 0, Telephone = 0, MatchGrade = 0;
            string minConfidentCode = "0";
            bool SelectTopMatch = false;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                CompanyMatch = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                CityName = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                StreetNo = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                StateName = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));
                StreetName = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));
                PostalCode = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1));
                Telephone = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 6, 1));
                minConfidentCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 8, 1);
                SelectTopMatch = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 7, 1));
            }
            StewardshipPortalModel model = new StewardshipPortalModel();
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            #region Current Page Records
            // gives the current pagesize,page number and number of records on the page
            int? page = null;
            int pagevalue = 0;
            int totalCount = 0;

            if (page == null && !string.IsNullOrEmpty(SessionHelper.pageNumberStewData))
                page = Convert.ToInt32(SessionHelper.pageNumberStewData);

            if (!string.IsNullOrEmpty(SessionHelper.pagevalueStewData))
                pagevalue = Convert.ToInt32(SessionHelper.pagevalueStewData);

            if (pagevalue == null || Convert.ToInt32(pagevalue) == 0)
            {
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                pagevalue = Convert.ToInt32(sfac.GetDefaultPageSize(Helper.oUser.UserId, "MatchData"));
                pagevalue = pagevalue == 0 ? 10 : pagevalue;
            }
            int currentPageIndex = page.HasValue ? page.Value : 1;

            pagevalue = pagevalue < 5 ? 5 : pagevalue;
            int pageNumber = (page ?? 1);
            SessionHelper.pagevalueStewData = Convert.ToString(pagevalue);
            SessionHelper.pageNumberStewData = Convert.ToString(pageNumber);

            if (minConfidentCode == "")
                minConfidentCode = "0";

            if (minConfidentCode.Contains('.'))
                minConfidentCode = minConfidentCode.Substring(0, minConfidentCode.IndexOf('.'));

            //Check and filter data from the session Filter.
            if (!string.IsNullOrEmpty(SessionHelper.TempCompanies))
            {
                model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.TempCompanies);
            }
            else
            {
                Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, currentPageIndex, Convert.ToInt32(pagevalue), out totalCount, Helper.IsApprovalScreen);
                model.Companies = tuplecompany.Item1;
                SessionHelper.QueueMessage = tuplecompany.Item2;
                model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.TempCompanies);
                SessionHelper.TotalCountStew = Convert.ToString(totalCount);
            }
            List<CompanyEntity> Companies = new List<CompanyEntity>();
            // Set Company matches for the particular company wise.
            model.Companies = SetOriginalMatchCount(model.Companies);
            foreach (var co in model.Companies)
            {
                List<MatchEntity> matches = (from m in co.Matches
                                             where m.DnBConfidenceCode >= Convert.ToInt32(minConfidentCode)
                                             where ((CompanyMatch > 0) ? SetMatchGradeFilterString(CompanyMatch).Contains(m.MGCompanyName.ToString()) : m.MGCompanyName.Contains(m.MGCompanyName))
                                             where ((StreetNo > 0) ? SetMatchGradeFilterString(StreetNo).Contains(m.MGStreetNo.ToString()) : m.MGStreetNo.Contains(m.MGStreetNo))
                                             where ((StreetName > 0) ? SetMatchGradeFilterString(StreetName).Contains(m.MGStreetName.ToString()) : m.MGStreetName.Contains(m.MGStreetName))
                                             where ((CityName > 0) ? SetMatchGradeFilterString(CityName).Contains(m.MGCity.ToString()) : m.MGCity.Contains(m.MGCity))
                                             where ((StateName > 0) ? SetMatchGradeFilterString(StateName).Contains(m.MGState.ToString()) : m.MGState.Contains(m.MGState))
                                             where ((Telephone > 0) ? SetMatchGradeFilterString(Telephone).Contains(m.MGTelephone.ToString()) : m.MGTelephone.Contains(m.MGTelephone))
                                             where ((PostalCode > 0) ? SetMatchGradeFilterString(PostalCode).Contains(m.MGZipCode.ToString()) : m.MGZipCode.Contains(m.MGZipCode))
                                             select m).ToList<MatchEntity>();
                co.Matches = matches;
                if (matches.Count == 0)
                {
                    Companies.Add(co);
                }
            }
            if (Companies != null && Companies.Any())
            {
                foreach (var company in Companies)
                {
                    model.Companies.Remove(company);
                }
            }
            // if select top 1 match records than select and set first records of each company as selected.
            if (SelectTopMatch)
            {
                model.Companies.ForEach(x => x.Matches.RemoveRange(1, x.Matches.Count - 1));
            }
            SessionHelper.TotalCountStew = Convert.ToString(totalCount);
            SessionHelper.TempCompanies = JsonConvert.SerializeObject(model.Companies);
            #endregion

            if (model != null && model.Companies.Any())
            {
                foreach (CompanyEntity company in model.Companies)
                {
                    // Rejects all the records
                    RejectMatched(company);
                }
                return new JsonResult { Data = DandBSettingLang.msgCommanRejectMessage };
            }
            return new JsonResult { Data = "" };
        }
        #endregion

        #region Preview Enrichment Data
        [HttpGet]
        public ActionResult PreviewEnrichmentData(string Parameters)
        {
            string DunsNumber = "", Company = "", Address = "", City = "", State = "", Postal = "", Country = "", Phone = "", SrcId = "", CountryCode = "", RegNo = "", Website = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                DunsNumber = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                Company = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                Address = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                City = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                State = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
                Postal = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1);
                CountryCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 6, 1);
                SrcId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 7, 1);
                Phone = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 8, 1);
                Country = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 9, 1);
                RegNo = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 10, 1);
                Website = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 11, 1);
            }
            DataTable dt = new DataTable();
            CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            dt = cfac.UXGetFirmographicsURL(DunsNumber, Country);

            bool EnrichmentExists = Convert.ToBoolean(dt.Rows[0]["EnrichmentExists"]);
            string EnrichmentURL = Convert.ToString(dt.Rows[0]["EnrichmentURL"]);
            string AuthToken = Convert.ToString(dt.Rows[0]["AuthToken"]);
            int DnBAPIId = Convert.ToInt32(dt.Rows[0]["DnBAPIId"]);
            int CredentialId = Convert.ToInt32(dt.Rows[0]["CredentialId"]);
            string APIFamily = Convert.ToString(dt.Rows[0]["APIFamily"]);

            DunsInfo model = new DunsInfo();
            model.Input = new SFI_Parent_Input();
            model.Input.DunsNumber = DunsNumber;
            model.Input.Company = Company == "Null" ? "" : Company;
            model.Input.Address = Address == "Null" ? "" : Address;
            model.Input.City = City == "Null" ? "" : City;
            model.Input.State = State == "Null" ? "" : State;
            model.Input.Postal = Postal == "Null" ? "" : Postal;
            model.Input.Country = Country == "Null" ? "" : Country;
            model.Input.SrcId = SrcId;
            model.Input.Phone = Phone == "Null" ? "" : Phone;
            model.Input.RegistrationNum = RegNo == "Null" ? "" : RegNo;
            model.Input.Website = Website == "Null" ? "" : Website;

            if (string.IsNullOrEmpty(AuthToken))
            {
                model.Base = new SFI_CMPELK_Baseclass();
                model.lstCurrentPrincipals = new List<SFI_CMPELK_CurrentPrincipalsclass>();
                model.lstIndustryCodes = new List<SFI_CMPELK_IndustryCodesclass>();
                model.lstRegistrationNumbers = new List<SFI_CMPELK_RegistrationNumbersclass>();
                model.lstStockExchanges = new List<SFI_CMPELK_StockExchangesclass>();
                return View(model);
            }
            if (EnrichmentExists == true)
            {
                PreviewMatchDataFacade fac = new PreviewMatchDataFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                DataSet ds = fac.PreviewEnrichmentData(DunsNumber);
                CommonMethod.EnrichmentDataPreview(model, ds);
            }
            else
            {
                Utility.PreviewEnrichmentData api = new Utility.PreviewEnrichmentData();
                string previewDataResponse = api.PreviewData(EnrichmentURL, AuthToken);
                dynamic data = JObject.Parse(previewDataResponse);
                if (data != null)
                {
                    if (APIFamily.ToLower() == ApiLayerType.Direct20.ToString().ToLower())
                    {

                        if (data.OrderProductResponse != null && data.OrderProductResponse.TransactionResult != null && !string.IsNullOrEmpty(data.OrderProductResponse.TransactionResult.ResultText.Value))
                        {
                            model.Base = new SFI_CMPELK_Baseclass();
                            model.lstCurrentPrincipals = new List<SFI_CMPELK_CurrentPrincipalsclass>();
                            model.lstIndustryCodes = new List<SFI_CMPELK_IndustryCodesclass>();
                            model.lstRegistrationNumbers = new List<SFI_CMPELK_RegistrationNumbersclass>();
                            model.lstStockExchanges = new List<SFI_CMPELK_StockExchangesclass>();
                            return View(model);
                        }
                        else
                        {
                            DateTime transactionTimestamp = new DateTime();
                            if (data.OrderProductResponse != null && data.OrderProductResponse.TransactionDetail != null && data.OrderProductResponse.TransactionDetail.TransactionTimestamp != null)
                            {
                                transactionTimestamp = Convert.ToDateTime(data.OrderProductResponse.TransactionDetail.TransactionTimestamp);
                            }
                            cfac.ProcessDataForEnrichment(0, DnBAPIId, DunsNumber, Country, previewDataResponse, "", transactionTimestamp, CredentialId);
                            PreviewMatchDataFacade fac = new PreviewMatchDataFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                            DataSet ds = fac.PreviewEnrichmentData(DunsNumber);
                            CommonMethod.EnrichmentDataPreview(model, ds);
                        }
                    }
                    else if (APIFamily.ToLower() == ApiLayerType.Directplus.ToString().ToLower())
                    {
                        if (data.Message != null && !string.IsNullOrEmpty(data.Message.Value))
                        {
                            model.Base = new SFI_CMPELK_Baseclass();
                            model.lstCurrentPrincipals = new List<SFI_CMPELK_CurrentPrincipalsclass>();
                            model.lstIndustryCodes = new List<SFI_CMPELK_IndustryCodesclass>();
                            model.lstRegistrationNumbers = new List<SFI_CMPELK_RegistrationNumbersclass>();
                            model.lstStockExchanges = new List<SFI_CMPELK_StockExchangesclass>();
                            return View(model);
                        }
                        else if (data.error != null && data.error.errorMessage != null && !string.IsNullOrEmpty(data.error.errorMessage.Value))
                        {
                            model.Base = new SFI_CMPELK_Baseclass();
                            model.lstCurrentPrincipals = new List<SFI_CMPELK_CurrentPrincipalsclass>();
                            model.lstIndustryCodes = new List<SFI_CMPELK_IndustryCodesclass>();
                            model.lstRegistrationNumbers = new List<SFI_CMPELK_RegistrationNumbersclass>();
                            model.lstStockExchanges = new List<SFI_CMPELK_StockExchangesclass>();
                            return View(model);
                        }
                        else
                        {
                            DateTime transactionTimestamp = Convert.ToDateTime(data.transactionDetail.transactionTimestamp.Value);
                            cfac.ProcessDataForEnrichment(0, DnBAPIId, DunsNumber, Country, previewDataResponse, "", transactionTimestamp, CredentialId);
                            PreviewMatchDataFacade fac = new PreviewMatchDataFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                            DataSet ds = fac.PreviewEnrichmentData(DunsNumber);
                            CommonMethod.EnrichmentDataPreview(model, ds);
                        }
                    }

                }
            }
            ViewBag.DunsNumber = DunsNumber;
            return View(model);
        }
        #endregion

        #region "Match Data Filters"

        public JsonResult GetOrderByColumnDD()
        {
            List<DropDownReturn> lstAllFilter = new List<DropDownReturn>();
            lstAllFilter.Add(new DropDownReturn { Value = "SrcRecordId", Text = "SrcRecordId" });
            lstAllFilter.Add(new DropDownReturn { Value = "CompanyName", Text = "CompanyName" });
            lstAllFilter.Add(new DropDownReturn { Value = "Address", Text = "Address" });
            lstAllFilter.Add(new DropDownReturn { Value = "City", Text = "City" });
            lstAllFilter.Add(new DropDownReturn { Value = "State", Text = "State" });
            lstAllFilter.Add(new DropDownReturn { Value = "PostalCode", Text = "PostalCode" });
            lstAllFilter.Add(new DropDownReturn { Value = "Country", Text = "Country" });
            lstAllFilter.Add(new DropDownReturn { Value = "PhoneNbr", Text = "PhoneNbr" });
            return Json(new { Data = lstAllFilter }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCountryDD()
        {
            UserSessionFacade fac = new UserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<CountryEntity> lstCountry = fac.GetCountries();
            List<DropDownReturn> lstAllFilter = new List<DropDownReturn>();
            lstAllFilter.Add(new DropDownReturn { Value = "", Text = "None" });
            foreach (var item in lstCountry)
            {
                lstAllFilter.Add(new DropDownReturn { Value = item.ISOAlpha2Code, Text = item.CountryWithISOCode });
            }
            return Json(new { Data = lstAllFilter }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetImportProcessDD()
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            DataTable dt = fac.GetImportProcessesByQueue(ImportProcess.SessionFilter.ToString(), false);
            List<DropDownReturn> lstAllFilter = new List<DropDownReturn>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAllFilter.Add(new DropDownReturn { Value = dt.Rows[i]["ImportProcess"].ToString(), Text = dt.Rows[i]["ImportProcess"].ToString() });
            }
            return Json(new { Data = lstAllFilter }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCountryGroupDD()
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<CountryGroupEntity> countryGroups = fac.GetCountryGroup();
            List<DropDownReturn> lstAllFilter = new List<DropDownReturn>();
            foreach (var item in countryGroups)
            {
                lstAllFilter.Add(new DropDownReturn { Value = item.GroupId.ToString(), Text = item.GroupName });
            }
            return Json(new { Data = lstAllFilter }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTagsDD()
        {
            List<TagsEntity> model = new List<TagsEntity>();
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            model = fac.GetAllTagsForUser(Helper.oUser.LOBTag, Helper.oUser.UserId, false);
            List<DropDownReturn> lstAllFilter = new List<DropDownReturn>();
            foreach (var item in model)
            {
                lstAllFilter.Add(new DropDownReturn { Value = item.Tag, Text = item.TagName });
            }
            return Json(new { Data = lstAllFilter }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterMatchData(List<FilterData> filters)
        {
            UserSessionFilterEntity filtermodel = new UserSessionFilterEntity();
            foreach (var item in filters)
            {
                if (item.FieldName == "SrcRecordId")
                    filtermodel.SrcRecordId = item.FilterValue;
                else if (item.FieldName == "CompanyName")
                    filtermodel.CompanyName = item.FilterValue;
                else if (item.FieldName == "City")
                    filtermodel.City = item.FilterValue;
                else if (item.FieldName == "State")
                    filtermodel.State = item.FilterValue;
                else if (item.FieldName == "Country")
                    filtermodel.CountryISOAlpha2Code = item.FilterValue;
                else if (item.FieldName == "ImportProcess")
                    filtermodel.ImportProcess = item.FilterValue;
                else if (item.FieldName == "CountryGroup")
                    filtermodel.CountryGroupId = Convert.ToInt32(item.FilterValue);
                else if (item.FieldName == "Tag")
                    filtermodel.Tag = item.FilterValue;
                else if (item.FieldName == "OrderByColumn")
                    filtermodel.OrderByColumn = item.FilterValue;
            }
            filtermodel.UserId = Helper.oUser.UserId;
            int pagenumber = 0;
            if (!SessionHelper.Stew_IsFirstTimeFilter)
            {
                UserSessionFacade fac = new UserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.InsertOrUpdateUserSessionFilter(filtermodel);
                pagenumber = 1;
            }
            else
            {
                if (SessionHelper.Stew_IsFirstTimeFilter)
                    pagenumber = 1;
                else
                {
                    if (!string.IsNullOrEmpty(SessionHelper.pageNumberStewData))
                        pagenumber = Convert.ToInt32(SessionHelper.pageNumberStewData);
                    else
                        pagenumber = 1;
                }
            }

            SessionHelper.Stew_IsFirstTimeFilter = false;
            int totalCount = 0;
            int pagevalue = 0;

            if (!string.IsNullOrEmpty(SessionHelper.pagevalueStewData))
                pagevalue = Convert.ToInt32(SessionHelper.pagevalueStewData);

            if (pagevalue == 0 || pagevalue == null)
            {
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                pagevalue = Convert.ToInt32(sfac.GetDefaultPageSize(Helper.oUser.UserId, "MatchData"));
                pagevalue = pagevalue == 0 ? 10 : pagevalue;
            }
            pagevalue = pagevalue < 5 ? 5 : pagevalue;
            CompanyFacade efac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            Tuple<List<CompanyEntity>, string> tuplecompany = efac.GetLCMCompany(Helper.oUser.UserId, pagenumber, pagevalue, out totalCount, Helper.IsApprovalScreen);
            StewardshipPortalModel model = new StewardshipPortalModel();
            model.Companies = tuplecompany.Item1;
            SessionHelper.QueueMessage = tuplecompany.Item2;
            model.Companies = SetOriginalMatchCount(model.Companies);
            if (Helper.IsApprovalScreen && model.Companies != null)
                model.Companies = FinishedLoadCompanies(model.Companies);

            SessionHelper.pagevalueStewData = Convert.ToString(pagevalue);
            SessionHelper.pageNumberStewData = Convert.ToString(pagenumber);
            SessionHelper.TotalCountStew = Convert.ToString(totalCount);
            SessionHelper.TempCompanies = JsonConvert.SerializeObject(model.Companies);
            IPagedList<CompanyEntity> pagedCompany = new StaticPagedList<CompanyEntity>(model.Companies.ToList(), pagenumber, pagevalue, totalCount);
            return PartialView("_Index", pagedCompany);
        }

        #endregion
    }
}
