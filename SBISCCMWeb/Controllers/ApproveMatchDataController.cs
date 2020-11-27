using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MvcSiteMapProvider;
using Newtonsoft.Json;
using PagedList;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{

    [Authorize, TwoStepVerification, AccessApproveMatch]
    public class ApproveMatchDataController : BaseController
    {
        // GET: ApproveMatchData
        #region "Initialization"
        StewardshipPortalModel _model = new StewardshipPortalModel();
        #endregion

        #region "Approve Match Data"
        public ActionResult Index(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            StewUserActivityCloseWindow();
            // Set User id in Session.


            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            StewardshipPortalModel model = new StewardshipPortalModel();
            Helper.IsApprovalScreen = true;
            // Get user Detail by UserId.
            UsersModel Users = new UsersModel();
            // Get Login Detail by the userId
            Users.objUsers = fac.StewUserLogIn(Helper.oUser.EmailAddress, null, true);
            // Check if User allow Enable2state and IsApproval screen or not.
            if (!Helper.Enable2StepUpdate)
                Helper.Approve = true;

            if (Helper.Enable2StepUpdate && !Helper.IsApprovalScreen)
                Helper.Approve = false;

            if (Helper.IsApprovalScreen)
                Helper.Approve = true;

            int pageNumber = (page ?? 1);
            TempData["page"] = page;
            // Find Company by userid and according to page no and page size wise.
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["pageno"])) && pagevalue == null)
                pagevalue = Convert.ToInt32(TempData["pageno"]);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;
            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageno = pagevalue.HasValue ? pagevalue.Value : 10;
            pageno = pageno < 5 ? 5 : pageno;
            TempData["pageno"] = pageno;
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = pageno;
            ViewBag.pagevalue = pagevalue;
            // Find Company by userid and according to page no and page size wise.

            Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, currentPageIndex, pageno, out totalCount, Helper.IsApprovalScreen);
            model.Companies = tuplecompany.Item1;
            TempData["ApproveQueueMessage"] = tuplecompany.Item2;
            model.Companies = SetOriginalMatchCount(model.Companies);
            if (Helper.IsApprovalScreen && model.Companies != null)
                model.Companies = FinishedLoadCompanies(model.Companies);

            string ViewId = Users.objUsers.UserLayoutPreference;
            // Check Default view mean display in grid view or panel view.
            if (string.IsNullOrEmpty(Convert.ToString(Session["ApproveMatchDataDefaultView"])))
            {
                if (ViewId == "GRID")
                {
                    Session["ApproveMatchDataDefaultView"] = "GRID";
                    Session["ApproveMatchDataCurrentView"] = "GRID";
                }
                else if (ViewId == "PANEL")
                {
                    Session["ApproveMatchDataDefaultView"] = "PANEL";
                    Session["ApproveMatchDataCurrentView"] = "PANEL";
                }
            }
            //Get session Filter data from the database and set in tempdata.
            TempData["TotalCount"] = totalCount;
            TempData["TempCompanies"] = model.Companies;
            IPagedList<CompanyEntity> pagedCompany = new StaticPagedList<CompanyEntity>(model.Companies.ToList(), currentPageIndex, pageno, totalCount);
            TempData.Keep();

            string ChildViewName = Convert.ToString(Session["ApproveMatchDataCurrentView"]) == "GRID" ? "_Index" : "_panelIndex";
            string ViewName = Convert.ToString(Session["ApproveMatchDataCurrentView"]) == "GRID" ? "Index" : "PanelIndex";
            if (Request.IsAjaxRequest())
                return PartialView(ChildViewName, pagedCompany);

            return View(ViewName, pagedCompany);
        }
        public ActionResult GetFilteredCompanyList(int? page, int? sortby, int? sortorder, int? pagevalue, int CompanyMatch = 0, int CityName = 0, int StreetNo = 0, int StateName = 0, int StreetName = 0, int PostalCode = 0, int Telephone = 0, int MatchGrade = 0, string minConfidentCode = "0", string Command = null, bool SelectTopMatch = false, string MatchItemID = null)
        {
            UserSessionFacade ufac = new UserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            // Set page no and Page size for the pagination.
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["page"])))
                page = Convert.ToInt32(TempData["page"]);

            if (!string.IsNullOrEmpty(Convert.ToString(TempData["pageno"])))
                pagevalue = Convert.ToInt32(TempData["pageno"]);

            int pageNumber = (page ?? 1);
            StewardshipPortalModel model = new StewardshipPortalModel();
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;
            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;
            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            if (TempData["TotalCount"] != null)
                totalCount = Convert.ToInt32(TempData["TotalCount"]);

            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageno = pagevalue.HasValue ? pagevalue.Value : 10;
            pageno = pageno < 5 ? 5 : pageno;
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pagevalue = pagevalue;
            if (minConfidentCode == "")
                minConfidentCode = "0";
            if (minConfidentCode.Contains('.'))
                minConfidentCode = minConfidentCode.Substring(0, minConfidentCode.IndexOf('.'));

            // Check and filter data from the session Filter.
            if (TempData["TempCompanies"] != null)
            {
                model.Companies = (TempData["TempCompanies"] as List<CompanyEntity>).Copy();
            }
            else
            {
                Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, currentPageIndex, pageno, out totalCount, Helper.IsApprovalScreen);
                model.Companies = tuplecompany.Item1;
                TempData["ApproveQueueMessage"] = tuplecompany.Item2;
                TempData["TempCompanies"] = (model.Companies as List<CompanyEntity>).Copy();
                TempData["TotalCount"] = totalCount;
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

            if (!string.IsNullOrEmpty(Command))
            {
                if (Command == "Update")
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
                Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, pageNumber, pageno, out totalCount, Helper.IsApprovalScreen);
                model.Companies = tuplecompany.Item1;
                TempData["ApproveQueueMessage"] = tuplecompany.Item2;
                if (totalCount <= 0 && model.Companies.Count == 0 && pageNumber > 1)
                {
                    tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, pageNumber - 1, pageno, out totalCount, Helper.IsApprovalScreen);
                    model.Companies = tuplecompany.Item1;
                    TempData["ApproveQueueMessage"] = tuplecompany.Item2;
                    TempData["page"] = pageNumber - 1;
                }

                model.Companies = SetOriginalMatchCount(model.Companies);
                TempData["TempCompanies"] = model.Companies;
                TempData["TotalCount"] = totalCount;
                if (SelectTopMatch)
                {
                    model.Companies.ForEach(x => x.Matches.RemoveRange(1, x.Matches.Count - 1));
                }
            }
            // Set model of Company entity to pass this model to view.
            IPagedList<CompanyEntity> pagedCompany = new StaticPagedList<CompanyEntity>(model.Companies.ToList(), pageNumber, pageno, totalCount);
            TempData.Keep();
            string ChildViewName = Convert.ToString(Session["ApproveMatchDataCurrentView"]) == "GRID" ? "_Index" : "_panelIndex";
            string ViewName = Convert.ToString(Session["ApproveMatchDataCurrentView"]) == "GRID" ? "Index" : "PanelIndex";
            if (Request.IsAjaxRequest())
                return PartialView(ChildViewName, pagedCompany);

            return View(ViewName, pagedCompany);
        }
        // Data Sort by sortorder and sortby
        public List<CompanyEntity> SortData(List<CompanyEntity> lstcompany, int sortorder, int sortby)
        {
            // sortorder 1 for ascending order and 2 for descending order.
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
            foreach (CompanyEntity co in lstCompany)
            {
                co.MatchesFiltered = co.Matches;
                co.RejectCompany = co.RejectAllMatches;
                if (co.MatchesFiltered.Any() && co.MatchesFiltered.Any(c => c.IsSelected))
                    co.SelectedMatchCount = 1;

            }
            return lstCompany;
        }
        #endregion

        #region "Match Details Popup "
        //Open Matched Item Detail View with all Parameter pass with serialization form and set next and prev data to manage next and prev functionality.
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
            ViewBag.dataNext = Convert.ToString(dataNext);
            ViewBag.dataPrev = Convert.ToString(dataPrev);
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            StewardshipPortalModel model = new StewardshipPortalModel();
            ViewBag.SelectData = "ApproveMatch";
            if (TempData["TempCompanies"] != null)
            {
                model.Companies = (TempData["TempCompanies"] as List<CompanyEntity>).Copy();
            }
            else
            {
                Helper.oUser.UserId = Convert.ToInt32(User.Identity.GetUserId());
                int page = Convert.ToInt32(TempData["page"]);
                int pagevalue = Convert.ToInt32(TempData["pageno"]);
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
            Helper.CompanyName = company.CompanyName == "" ? "Null" : company.CompanyName;
            Helper.Address = company.Address == "" ? "Null" : company.Address;
            Helper.City = company.City == "" ? "Null" : company.City;
            Helper.State = company.State == "" ? "Null" : company.State;
            Helper.PhoneNbr = company.PhoneNbr == "" ? "Null" : company.PhoneNbr;
            Helper.Zip = company.PostalCode == "" ? "" : company.PostalCode;

            Tuple<MatchEntity, CompanyEntity> tuple = new Tuple<MatchEntity, CompanyEntity>(Match, company);
            TempData.Keep();
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
            if (TempData["TempCompanies"] != null)
            {
                model.Companies = (TempData["TempCompanies"] as List<CompanyEntity>).Copy();
            }
            if (model.Companies.Any() && !string.IsNullOrEmpty(id))
            {
                foreach (var company in model.Companies)
                {
                    if (company.InputId == Convert.ToInt32(id))
                    {
                        if (Convert.ToString(MatchSeqence).ToLower() == "rejectall")
                        {
                            company.Matches.ForEach(x => x.IsSelected = false);
                            company.SelectedMatchCount = 0;
                            company.RejectCompany = true;
                        }
                        else if (Convert.ToString(MatchSeqence).ToLower() == "unrejectall")
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
            TempData["TempCompanies"] = model.Companies;
            Helper.IsDirty = true;
            TempData.Keep();
        }
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public void RejectLCMMatches(string id, string MatchSeqence)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            StewardshipPortalModel model = new StewardshipPortalModel();
            //We store all matches for Company in temp data and when user will select or deselect match at that time fill old matches to list
            //and get the id from the list to PreviousId and then remove from the list and add new id into list.
            if (TempData["TempCompanies"] != null)
            {
                model.Companies = (TempData["TempCompanies"] as List<CompanyEntity>).Copy();
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
            TempData["TempCompanies"] = model.Companies;
            Helper.IsDirty = false;
            TempData.Keep();
        }
        // Accept match and Update the current Model
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
        // Reject match and Update the current Model
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
                catch {
                    //Empty catch block to stop from breaking
                }
            }
        }

        // Delete Session filter for user.
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteSessionFilter()
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            fac.DeleteUserSessionFilter(Helper.oUser.UserId);
            return new JsonResult { Data = "success" };

        }
        public ActionResult ChangeView()
        {
            // Change view for Panel and Grid View
            if (!string.IsNullOrEmpty(Convert.ToString(Session["ApproveMatchDataCurrentView"])))
            { Session["ApproveMatchDataCurrentView"] = Convert.ToString(Session["ApproveMatchDataCurrentView"]) == "GRID" ? "PANEL" : "GRID"; }
            else { Session["ApproveMatchDataCurrentView"] = "GRID"; }
            return RedirectToAction("Index");
        }
        // Set Default layout for user (Grid or Panel)
        [HttpPost, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult SetUserLayoutPreference(string userLayout)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            fac.SetUserLayoutPreference(Helper.oUser.UserId, userLayout);
            if (userLayout == "PANEL")
            {
                Session["ApproveMatchDataDefaultView"] = "PANEL";
                Session["ApproveMatchDataCurrentView"] = "PANEL";
            }
            else
            {
                Session["ApproveMatchDataDefaultView"] = "GRID";
                Session["ApproveMatchDataCurrentView"] = "GRID";
            }
            return new JsonResult { Data = "success" };
        }
        [RequestFromAjax]
        public JsonResult GetNextMatchDetailRecord(int SrcId)
        {
            // Get next matches detail for company in match item detail view.
            StewardshipPortalModel model = new StewardshipPortalModel();
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["TempCompanies"])))
            {
            }
            return new JsonResult { Data = "success" };
        }
        #endregion

        #region "Open Search Note"
        // Get Stewardship note for matched. 
        [HttpGet]
        public ActionResult OpenSearchData(int InputId, int count, string Notes)
        {
            string Note = "";
            if (Helper.IsApprovalScreen == false)
            {
                StewardshipPortalModel model = new StewardshipPortalModel();
                if (TempData["TempCompanies"] != null)
                {
                    model.Companies = (TempData["TempCompanies"] as List<CompanyEntity>).Copy();
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
            TempData.Keep();
            ViewBag.StewardShipNote = Note;
            ViewBag.InputId = InputId;
            ViewBag.MatchCount = count;
            return PartialView("_StewardshipNotes");
        }
        // Save stewardship note detail in database.
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult OpenSearchData(string StewardshipNotes, int InputId, int Count)
        {
            if (Helper.IsApprovalScreen == false)
            {
                StewardshipPortalModel model = new StewardshipPortalModel();
                if (TempData["TempCompanies"] != null)
                {
                    model.Companies = (TempData["TempCompanies"] as List<CompanyEntity>).Copy();
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
                TempData["TempCompanies"] = model.Companies;
            }
            TempData.Keep();
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

        #region "Custom Attribute"
        [HttpGet]
        public ActionResult popupCompanyAttribute(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            string SrcRecordId = Parameters;
            //find custom attribute like tax id, Revenue etc.. by the id.
            DataTable dtAttributes = new DataTable();
            if (!string.IsNullOrEmpty(SrcRecordId))
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                dtAttributes = fac.GetComanyAttribute(SrcRecordId);
            }
            IPagedList<dynamic> pagedAttribute = new StaticPagedList<dynamic>(dtAttributes.AsDynamicEnumerable(), 1, 100000, 0);
            ViewBag.SrcRecordId = SrcRecordId;
            return View(pagedAttribute);
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
        public ActionResult GoogleMapPopUp(string id)
        {
            StewardshipPortalModel model = new StewardshipPortalModel();
            CompanyEntity Company = new CompanyEntity();
            if (TempData["TempCompanies"] != null)
            {
                model.Companies = (TempData["TempCompanies"] as List<CompanyEntity>).Copy();
            }
            // remove selected matches for rejection.
            if (model.Companies.Any() && !string.IsNullOrEmpty(id))
            {
                Company = model.Companies.Where(x => x.SrcRecordId == id).FirstOrDefault();
            }
            TempData.Keep();
            return View(Company);
        }
        #endregion
    }
}