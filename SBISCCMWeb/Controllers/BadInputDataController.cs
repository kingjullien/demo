using Microsoft.AspNet.Identity;
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
using System.Linq;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, DandBLicenseEnabled, AllowDataStewardshipLicense]
    public class BadInputDataController : BaseController
    {
        // GET: BadInputData

        #region "Load Bad Input Data"
        public ActionResult Index(int? page, int? sortby, int? sortorder, int? pagevalue, string sort = null)
        {
            // Clear WorkQueue  for Data
            StewUserActivityCloseWindow();
            if (Request.IsAjaxRequest())
            {
                // Set User name in session.
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                #region Set pagination Code
                int pageNumber = (page ?? 1);
                if (!string.IsNullOrEmpty(SessionHelper.CleanMatchPageNo) && pagevalue == null)
                    pagevalue = Convert.ToInt32(SessionHelper.CleanMatchPageNo);

                if (pagevalue == null || Convert.ToInt32(pagevalue) == 0)
                {
                    SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    pagevalue = Convert.ToInt32(sfac.GetDefaultPageSize(Helper.oUser.UserId, "BadInputData"));
                    pagevalue = pagevalue == 0 ? 10 : pagevalue;
                }

                BadInputDataModel model = new BadInputDataModel();
                if (!(sortby.HasValue && sortby.Value > 0))
                    sortby = 1;

                if (!(sortorder.HasValue && sortorder.Value > 0))
                    sortorder = 1;

                int totalCount = 0;
                int pageno = pagevalue.HasValue ? pagevalue.Value : 10;
                pageno = pageno < 5 ? 5 : pageno;
                // Set page no , sortorder , sortby and Page size to session 
                ViewBag.SortBy = sortby;
                ViewBag.SortOrder = sortorder;
                ViewBag.Page = pageNumber;
                ViewBag.pageno = pageno;
                Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetBIDCompany(Helper.oUser.UserId, pageNumber, pageno, out totalCount);
                model.Companies = tuplecompany.Item1;
                SessionHelper.CleanQueueMessage = tuplecompany.Item2;

                SessionHelper.CleanMatchPage = Convert.ToString(pageNumber);
                SessionHelper.CleanMatchPageNo = Convert.ToString(pageno);
                SessionHelper.CleanTotalCount = Convert.ToString(totalCount);
                SessionHelper.CleanMatchedCompany = JsonConvert.SerializeObject(model.Companies);
                #endregion

                IPagedList<CompanyEntity> pagedCompany = new StaticPagedList<CompanyEntity>(model.Companies.ToList(), pageNumber, pageno, totalCount);// Set model of Company entity to pass this model to view.
                return PartialView("_Index", pagedCompany);
            }
            else
            {
                SessionHelper.BadInputData_IsFirstTimeFilter = true;
            }
            return View();
        }
        public ActionResult GetFilteredCompanyList(int? mode, int? page, int? sortby, int? sortorder, int? pagevalue, string sort = null)
        {
            // For Filtration of data .
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            #region Set Pagination code
            //Get Current Page size ,page Number.
            if (!string.IsNullOrEmpty(SessionHelper.CleanMatchPage))
                page = Convert.ToInt32(SessionHelper.CleanMatchPage);

            if (!string.IsNullOrEmpty(SessionHelper.CleanMatchPageNo))
                pagevalue = Convert.ToInt32(SessionHelper.CleanMatchPageNo);

            if (pagevalue == null || Convert.ToInt32(pagevalue) == 0)
            {
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                pagevalue = Convert.ToInt32(sfac.GetDefaultPageSize(Helper.oUser.UserId, "BadInputData"));
                pagevalue = pagevalue == 0 ? 10 : pagevalue;
            }

            int pageNumber = (page ?? 1);
            BadInputDataModel model = new BadInputDataModel();
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            int totalCount = 0;
            int pageno = pagevalue.HasValue ? pagevalue.Value : 10;
            pageno = pageno < 5 ? 5 : pageno;
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.Page = pageNumber;
            ViewBag.pageno = pageno;
            #endregion

            if (!string.IsNullOrEmpty(SessionHelper.CleanMatchedCompany)) //if Company is already in temp data then don't do db call to find company list
            {
                model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.CleanMatchedCompany);
                totalCount = !string.IsNullOrEmpty(SessionHelper.CleanTotalCount) ? Convert.ToInt32(SessionHelper.CleanTotalCount) : 0;
            }
            else
            {
                //model.Companies = fac.GetBIDCompany(Helper.oUser.UserId, pageNumber, pageno, out totalCount);//Get Company List if it is not found from the TempData
                Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetBIDCompany(Helper.oUser.UserId, pageNumber, pageno, out totalCount);
                model.Companies = tuplecompany.Item1;
                SessionHelper.CleanQueueMessage = tuplecompany.Item2;
            }
            if (sortorder != null && sortby != null)
                model.Companies = SortData(model.Companies, Convert.ToInt32(sortorder), Convert.ToInt32(sortby));

            if (totalCount <= 0 && model.Companies.Count == 0 && pageNumber > 1)
            {
                totalCount = Convert.ToInt32(pagevalue);
            }

            // Set model of Company entity to pass this model to view.
            IPagedList<CompanyEntity> pagedCompany = new StaticPagedList<CompanyEntity>(model.Companies.ToList(), pageNumber, pageno, totalCount);
            ViewBag.CloseAlert = "<script type='text/javascript'>$(document).ready(function(){parent.backToparent();});</script>";
            if (Request.IsAjaxRequest())
                return PartialView("_Index", pagedCompany);

            return View(pagedCompany);
        }
        // Sort Company by sort order.
        public List<CompanyEntity> SortData(List<CompanyEntity> lstcompany, int sortorder, int sortby)
        {
            switch (sortby)// sort order 1 for ascending order and 2 for descending order.
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
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.ErrorCode).ToList() : lstcompany.OrderByDescending(x => x.ErrorCode).ToList();
                    break;
                case 10:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.ErrorDescription).ToList() : lstcompany.OrderByDescending(x => x.ErrorDescription).ToList();
                    break;
                case 11:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.inLanguage).ToList() : lstcompany.OrderByDescending(x => x.inLanguage).ToList();
                    break;
            }
            return lstcompany;
        }

        public void StewUserActivityCloseWindow()
        {
            // Set window close event and manage changes discard for page.
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            fac.StewUserActivityCloseWindow(Helper.oUser.UserId);

        }
        #endregion

        #region "Update Company & Matches"
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult UpdateBadInputData(int InputId, string company, string address, string city, string state, string postalCode, string countryISOAlpha2Code, string phoneNbr, bool isRejected = false, string inLanguage = null)
        {
            // Update company by default functionality of grid to update current company records 
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            bool result = false;
            int pageNumber = Convert.ToInt32(SessionHelper.CleanMatchPage);
            int pageno = Convert.ToInt32(SessionHelper.CleanMatchPageNo);
            int totalCount = 0;
            List<CompanyEntity> GoodCompanies = new List<CompanyEntity>();
            List<CompanyEntity> DeletedCompanies = new List<CompanyEntity>();
            CompanyEntity companyEntity;

            Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetBIDCompany(Helper.oUser.UserId, pageNumber, pageno, out totalCount);
            companyEntity = tuplecompany.Item1.FirstOrDefault(x => x.InputId == InputId);
            SessionHelper.CleanQueueMessage = tuplecompany.Item2;

            if (companyEntity != null && companyEntity.InputId == InputId)
            {
                companyEntity.CompanyName = company;
                companyEntity.Address = address;
                companyEntity.City = city;
                companyEntity.State = state;
                companyEntity.PostalCode = postalCode;
                companyEntity.CountryISOAlpha2Code = countryISOAlpha2Code;
                companyEntity.PhoneNbr = phoneNbr;
                companyEntity.inLanguage = inLanguage;
                companyEntity.IsEdited = true;
                if (isRejected)// if  isRejected mean company is reject for clean match and delete company otherwise update company.
                {
                    companyEntity.RejectCompany = true;
                    DeletedCompanies.Add(companyEntity);
                    if (DeletedCompanies.Count > 0)
                        fac.DeleteBIDData(DeletedCompanies);

                }
                else
                {
                    if (!companyEntity.RejectCompany) // Selected Company or Not Rejected Company list
                    {
                        GoodCompanies.Add(companyEntity);
                    }
                    if (GoodCompanies.Count > 0)
                        fac.UpdateBIDRecord(GoodCompanies);
                }
            }
            result = true;
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteSessionFilter()
        {
            // Delete Session filter for user.
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            fac.DeleteUserSessionFilter(Helper.oUser.UserId);
            return new JsonResult { Data = "success" };
        }
        #endregion

        #region "Search Data Popup"
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult FillTempData(string id)
        {
            CompanyEntity company = new CompanyEntity();
            if (!string.IsNullOrWhiteSpace(id))
                company = SerializeHelper.DeserializeString<CompanyEntity>(id);

            SessionHelper.SearchCompany = JsonConvert.SerializeObject(company);
            return new JsonResult { Data = "success" };
        }
        public ActionResult SearchData(string Parameters)
        {
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            ViewBag.IsLanguageFlag = Parameters;
            //Open Search data Popup when click on search in Grid.
            CompanyEntity company = new CompanyEntity();
            if (!string.IsNullOrEmpty(SessionHelper.SearchCompany))
            {
                company = JsonConvert.DeserializeObject<CompanyEntity>(SessionHelper.SearchCompany);
                ViewBag.Company = company;
            }
            // Set all company data in session.
            ViewBag.ExcludeNonHeadQuarters = false;
            ViewBag.ExcludeNonMarketable = false;
            ViewBag.ExcludeOutofBusiness = false;
            ViewBag.ExcludeUndeliverable = false;
            ViewBag.ExcludeUnreachable = false;
            ViewBag.SrcRecordId = company.SrcRecordId;
            ViewBag.SrcRecId = company.SrcRecordId != null ? company.SrcRecordId : "";
            Helper.SrcRecordId = company.SrcRecordId != null ? company.SrcRecordId : "";
            Helper.CompanyName = company.CompanyName != null ? company.CompanyName : "";
            Helper.Address = company.Address != null ? company.Address : "";
            Helper.Address1 = company.Address1 != null ? company.Address1 : "";
            Helper.PhoneNbr = company.PhoneNbr != null ? company.PhoneNbr : "";
            Helper.City = company.City != null ? company.City : "";
            Helper.State = company.State != null ? company.State : "";
            Helper.Zip = company.PostalCode != null ? company.PostalCode : "";
            ViewBag.Country = company.CountryISOAlpha2Code != null ? company.CountryISOAlpha2Code : "";
            ViewBag.InputId = company.InputId;
            MainMatchEntity objmainMatchEntity = new MainMatchEntity();
            if (company.SrcRecordId != null)
            {
                SessionHelper.SearchCompanySrcId = company.SrcRecordId;
            }
            if (company.InputId > 0)
            {
                SessionHelper.SearchCompanyInputId = company.InputId.ToString();
            }
            if (objmainMatchEntity.lstMatches == null)
            {
                objmainMatchEntity.lstMatches = new List<MatchEntity>();
            }
            Tuple<MainMatchEntity, CompanyEntity> tuple = new Tuple<MainMatchEntity, CompanyEntity>(objmainMatchEntity, company);
            return PartialView("_PopUpSearchData", tuple);
        }
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult SearchData(int? page, int? sortby, int? sortorder, int? pagevalue, string SrcRecId, string Company, string Address, string Address2, string Phone, string City, string State, string Zip, string Country, bool ExcludeNonHeadQuarters = false, bool ExcludeNonMarketable = false, bool ExcludeOutofBusiness = false, bool ExcludeUndeliverable = false, bool ExcludeUnreachable = false, string inLanguage = null, string btnSearchData = null, string InputId = null)
        {
            CompanyEntity company = new CompanyEntity();
            if (!string.IsNullOrEmpty(SessionHelper.SearchCompany))
            {
                company = JsonConvert.DeserializeObject<CompanyEntity>(SessionHelper.SearchCompany);
                ViewBag.Company = company;
            }
            // On Click on search button on search popup page and find search data.
            int pageSize = 5;
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            Company = Company != null ? Company : "";
            Helper.CompanyName = Company != null ? Company : "";
            Helper.Address = Address != null ? Address : "";
            Helper.Address1 = Address2 != null ? Address2 : "";


            Helper.PhoneNbr = Phone != null ? Phone : "";
            Helper.City = City != null ? City : "";
            Helper.State = State != null ? State : "";
            Helper.Zip = Zip != null ? Zip : "";
            Helper.ExcludeNonHeadQuarters = ExcludeNonHeadQuarters;
            Helper.ExcludeNonMarketable = ExcludeNonMarketable;
            Helper.ExcludeOutofBusiness = ExcludeOutofBusiness;
            Helper.ExcludeUndeliverable = ExcludeUndeliverable;
            Helper.ExcludeUnreachable = ExcludeUnreachable;
            ViewBag.Country = Country != null ? Country : "";
            CommonSearchData common = new CommonSearchData();
            //use common method
            MainMatchEntity objmainMatchEntity;
            objmainMatchEntity = common.LoadData(Company, Address, Address2, City, State, Country, Zip, Phone, ExcludeNonHeadQuarters, ExcludeNonMarketable, ExcludeOutofBusiness, ExcludeUndeliverable, ExcludeUnreachable, inLanguage, SrcRecId, this.CurrentClient.ApplicationDBConnectionString, InputId);
            SessionHelper.SearchMatches = JsonConvert.SerializeObject(objmainMatchEntity.lstMatches);
            SessionHelper.SearchMatch = JsonConvert.SerializeObject(objmainMatchEntity.lstMatches);

            SearchModel objmodel = new SearchModel();
            objmodel.CompanyName = Company;
            objmodel.Address = Address;
            objmodel.Address2 = Address2;
            objmodel.City = City;
            objmodel.State = State;
            objmodel.Zip = Zip;
            objmodel.Country = Country;
            objmodel.PhoneNbr = Phone;
            SessionHelper.SearchModel = JsonConvert.SerializeObject(objmodel);

            if (objmainMatchEntity.lstMatches == null)
            {
                objmainMatchEntity.lstMatches = new List<MatchEntity>();
            }
            ViewBag.ExcludeNonHeadQuarters = ExcludeNonHeadQuarters;
            ViewBag.ExcludeNonMarketable = ExcludeNonMarketable;
            ViewBag.ExcludeOutofBusiness = ExcludeOutofBusiness;
            ViewBag.ExcludeUndeliverable = ExcludeUndeliverable;
            ViewBag.ExcludeUnreachable = ExcludeUnreachable;
            ViewBag.SrcRecordId = SrcRecId;
            ViewBag.CloseAlert = "<script type='text/javascript'>$(document).ready(function(){dynamicSearchDataHeight();});</script>";
            ViewBag.inLanguage = inLanguage;

            Tuple<MainMatchEntity, CompanyEntity> tuple = new Tuple<MainMatchEntity, CompanyEntity>(objmainMatchEntity, company);
            ViewBag.IsLanguageFlag = inLanguage;
            if (Request.IsAjaxRequest() && (btnSearchData == CleanDataLang.lblSearch || btnSearchData == "SearchByCompany" || btnSearchData == "SearchByAddress"))
            {
                return PartialView("_SearchData", objmainMatchEntity);
            }
            return PartialView("_PopUpSearchData", tuple);
        }

        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult AcceptBIDMatch(string InputId, string Match, string CompanyName = null, string address = null, string city = null, string state = null, string postalCode = null, string countryISOAlpha2Code = null, string phoneNbr = null)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            MatchEntity selectedMatchEntity = new MatchEntity();
            CompanyEntity company = new CompanyEntity();
            BadInputDataModel model = new BadInputDataModel();
            int pageNumber = Convert.ToInt32(SessionHelper.CleanMatchPage);
            int pageno = Convert.ToInt32(SessionHelper.CleanMatchPageNo);
            int totalCount = 0;
            // Get match from the parameter and desacralize the string and convert into MatchEntity.
            if (!string.IsNullOrWhiteSpace(Match))
            {
                selectedMatchEntity = SerializeHelper.DeserializeString<MatchEntity>(Match);
            }
            // if temp data for Company is null then get company from the database.
            if (!string.IsNullOrWhiteSpace(SessionHelper.CleanMatchedCompany))
            {
                model.Companies = JsonConvert.DeserializeObject<List<CompanyEntity>>(SessionHelper.CleanMatchedCompany);
            }
            else
            {
                Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetBIDCompany(Helper.oUser.UserId, pageNumber, pageno, out totalCount);
                model.Companies = tuplecompany.Item1;
                SessionHelper.CleanQueueMessage = tuplecompany.Item2;
            }
            // if current company src record id is equal to the match than current is acceptable company.
            if (model.Companies.Count > 0)
            {
                foreach (var companies in model.Companies)
                {
                    if (companies.InputId == Convert.ToInt32(InputId))
                        company = companies;

                }
                if (!Helper.IsSearchBYDUNS)
                {
                    company.CompanyName = CompanyName;
                    company.Address = address;
                    company.City = city;
                    company.State = state;
                    company.PostalCode = postalCode;
                    company.CountryISOAlpha2Code = countryISOAlpha2Code;
                }
                fac.AcceptBIDMatch(company, selectedMatchEntity);
            }
            return new JsonResult { Data = "success" };
        }
        //Open Matched Item Detail View with all Parameter pass with serialization form and set next and prev data to manage next and prev functionality.
        public ActionResult cShowMatchedItesDetailsView(string Parameters)
        {
            string id = string.Empty, childButtonId = string.Empty, dataNext = string.Empty, dataPrev = string.Empty, count = string.Empty, SrcId = string.Empty; bool IsPartialView = false;
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
                SrcId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 7, 1);
            }
            MatchEntity Match;
            List<MatchEntity> matches = new List<MatchEntity>();
            ViewBag.dataNext = dataNext;
            ViewBag.dataPrev = dataPrev;
            ViewBag.SelectData = "BadInputData";
            ViewBag.SrcId = SrcId;
            matches = JsonConvert.DeserializeObject<List<MatchEntity>>(SessionHelper.SearchMatches);
            Match = matches.FirstOrDefault(x => x.DnBDUNSNumber == childButtonId);
            try
            {
                ViewBag.NextToNextDUNS = dataNext != "" ? matches.SkipWhile(p => p.DnBDUNSNumber != dataNext).ElementAt(1).DnBDUNSNumber : matches.SkipWhile(p => p.DnBDUNSNumber != childButtonId).ElementAt(1).DnBDUNSNumber;
            }
            catch
            {
                ViewBag.NextToNextDUNS = "";
            }
            try
            {
                ViewBag.PrevToPrevDUNS = dataPrev != "" ? matches.TakeWhile(p => p.DnBDUNSNumber != dataPrev).LastOrDefault().DnBDUNSNumber : matches.TakeWhile(p => p.DnBDUNSNumber != childButtonId).LastOrDefault().DnBDUNSNumber;
            }
            catch
            {
                ViewBag.PrevToPrevDUNS = "";

            }
            CompanyEntity company = new CompanyEntity();
            Tuple<MatchEntity, CompanyEntity> tuple = new Tuple<MatchEntity, CompanyEntity>(Match, company);
            //when Popup is already open and click on next previous at that time we reload just partial view
            if (!IsPartialView)
            {
                return PartialView("~/Views/StewardshipPortal/_MatchedItemDetailView.cshtml", tuple);
            }
            else
            {
                return PartialView("~/Views/StewardshipPortal/_MatchDetails.cshtml", tuple);
            }
        }
        public ActionResult UpdateCompany(string InputId = null, string company = null, string address = null, string city = null, string state = null, string postalCode = null, string countryISOAlpha2Code = null, string phoneNbr = null, bool isRejected = false, string inLanguage = null, string Address2 = null)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            BadInputDataModel model = new BadInputDataModel();
            int pageNumber = Convert.ToInt32(SessionHelper.CleanMatchPage);
            int pageno = Convert.ToInt32(SessionHelper.CleanMatchPageNo);
            int totalCount = 0;
            totalCount = !string.IsNullOrEmpty(SessionHelper.CleanTotalCount) ? Convert.ToInt32(SessionHelper.CleanTotalCount) : 0;


            Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetBIDCompany(Helper.oUser.UserId, pageNumber, pageno, out totalCount);
            model.Companies = tuplecompany.Item1;


            foreach (var com in model.Companies)
            {
                if (com.InputId == Convert.ToInt32(InputId))
                {
                    com.inLanguage = string.IsNullOrEmpty(com.inLanguage) ? "" : com.inLanguage;
                    inLanguage = string.IsNullOrEmpty(inLanguage) ? "" : inLanguage;

                    if (com.CompanyName != company || com.Address != address || com.Address1 != Address2 || com.City != city || com.State != state || com.PostalCode != postalCode || com.CountryISOAlpha2Code != countryISOAlpha2Code || com.PhoneNbr != phoneNbr || com.inLanguage != inLanguage)
                    {
                        // Update company by default functionality of grid to update current company records 
                        UpdateBadInputData(Convert.ToInt32(InputId), company, address, city, state, postalCode, countryISOAlpha2Code, phoneNbr, isRejected, inLanguage);
                    }
                }

            }
            return null;
        }
        #endregion


        #region "Add Company By DUNS" 
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult FillMatchData(string MatchRecord)
        {
            MatchEntity Match = new MatchEntity();
            if (!string.IsNullOrWhiteSpace(MatchRecord))
                Match = SerializeHelper.DeserializeString<MatchEntity>(MatchRecord);

            SessionHelper.MatchRecord = JsonConvert.SerializeObject(Match);
            SessionHelper.DUNSNumber = "DUNS-" + Match.DnBDUNSNumber;
            return new JsonResult { Data = CommonMessagesLang.msgSuccess };
        }
        public ActionResult AddCompany(string Parameters)
        {
            string OriginalSrcRecId = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                OriginalSrcRecId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
            }
            // Open Popup for the add company from the matches
            ViewBag.OriginalSrcRecordId = OriginalSrcRecId;
            return View();
        }
        [HttpPost, ValidateInput(true), RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult AddCompany(string SrcId, string MatchRecord, string Tag, string OriginalSrcRecordId)
        {
            MatchFacade mac = new MatchFacade(this.CurrentClient.ApplicationDBConnectionString);
            try
            {
                // Validate SrcId for checking duplicate records at "Add Match as a new Company".
                mac.ValidateCompanySrcId(SrcId);
                MatchEntity Match = new MatchEntity();
                if (!string.IsNullOrWhiteSpace(MatchRecord))
                {
                    // Add Match Record as Company record
                    Match = SerializeHelper.DeserializeString<MatchEntity>(MatchRecord);
                    Match.Tags = Tag;
                    Match.OriginalSrcRecordId = OriginalSrcRecordId;
                    Match.SrcRecordId = SrcId;
                    ViewBag.matchRecord = Match;
                    mac.AddCompanyRecord(Match, Convert.ToInt32(User.Identity.GetUserId()));
                }
            }
            catch (SqlException ex)
            {
                return new JsonResult { Data = ex.Message };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message };
            }
            return new JsonResult { Data = CommonMessagesLang.msgSuccess };
        }
        #endregion

        #region Set session for collapse and expand
        public void SetSession(bool IsExpandCleanData)
        {
            Helper.IsExpandCleanData = IsExpandCleanData;
        }
        #endregion
        #region "Re Match Records"

        //Implement re-match queue (MP-14)
        [HttpGet]
        public ActionResult ReMatchRecords()
        {
            ReMatchRecordsEntity objReMatchRecordsEntity = new ReMatchRecordsEntity();
            objReMatchRecordsEntity.GetCountsOnly = true;
            return View(objReMatchRecordsEntity);
        }
        [RequestFromAjax, ValidateAntiForgeryToken, RequestFromSameDomain, ValidateInput(false)]
        public ActionResult ReMatchRecords(ReMatchRecordsEntity model)
        {
            //Implement re-match queue (MP-14)
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            model.UserId = Helper.oUser.UserId;
            string Message = fac.StewReMatchBadInputData(model);
            var response = string.Empty;
            if (model.GetCountsOnly)
            {
                Message = "Total " + Message + " records are affected, are you sure you want to continue ?";
                response = string.Format("<input id=\"ReMatchMessage\" name=\"ReMatchMessage\" type=\"hidden\" value=\"{0}\">", Message);
            }
            else
            {
                response = string.Format("<input id=\"ReMatchMessage\" name=\"ReMatchMessage\" type=\"hidden\" value=\"{0}\">", Message);
            }
            return Content(response);
        }

        #endregion

        #region Re-match record a single record from right clicking on UI
        //re-matches a single record from the UI (right - click UI option)
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult ReMatchRecord(string Parameters)
        {
            string SrcRecordId = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                SrcRecordId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
            }
            ReMatchRecordsEntity model = new ReMatchRecordsEntity();
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            model.SrcRecordId = SrcRecordId;
            model.UserId = Helper.oUser.UserId;
            string Message = fac.StewReMatchBadInputData(model);
            if (Message != "" && Message.Contains("Data ReMatch Request Completed Successfully"))
            {
                return new JsonResult { Data = Message };
            }
            else
            {
                return new JsonResult { Data = Message };
            }
        }
        #endregion
        #region Purge records-Additional Actions
        // Purge Recods from File-From Additional Actions dropdown
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult PurgeMultipleRecords(string[] Ids, string[] SrcIds)
        {
            DataTable dt = new DataTable();
            string Message = string.Empty;

            dt.Columns.Add(new DataColumn("SrcRecordId"));
            dt.Columns.Add(new DataColumn("InputId"));

            if (Ids != null && SrcIds != null)
            {
                DataRow dr;
                for (int i = 0; i < SrcIds.Length; i++)
                {
                    dr = dt.NewRow();
                    dr["SrcRecordId"] = Convert.ToString(SrcIds[i]);
                    dr["InputId"] = Convert.ToString(Ids[i]);
                    dt.Rows.Add(dr);
                }
                if (dt != null)
                {
                    try
                    {
                        //bulk insert new records
                        Message = BulkInsert(dt, true);
                    }
                    catch (Exception)
                    {
                        Message = CommonMessagesLang.msgCommanErrorMessage;
                    }
                }
                else
                {
                    return new JsonResult { Data = CommonMessagesLang.msgSomethingWentWrong };
                }
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgSomethingWentWrong };
            }
            return new JsonResult { Data = Message };
        }
        public string BulkInsert(DataTable dt, bool IsPurgeData)
        {
            string Message = string.Empty;
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
                    bulkCopy.ColumnMappings.Add("InputId", "InputId");
                    bulkCopy.ColumnMappings.Add("SrcRecordId", "SrcRecordId");

                    bulkCopy.DestinationTableName = "ext.StgImportDataForRejectPurge";
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        trans.Commit();
                        SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                        Message = sfac.RejectPurgeDataFromImport(Helper.oUser.UserId, IsPurgeData);
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message.ToString();
                    }
                }
            }
            return Message;
        }
        #endregion

        #region Re-Match records-Additional Actions
        // Re-Match Recods from File-From Additional Actions dropdown
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult ReMatchMultipleRecords(string[] SrcIds)
        {
            string Message = string.Empty;
            ReMatchRecordsEntity model = new ReMatchRecordsEntity();
            if (SrcIds != null)
            {
                for (int i = 0; i < SrcIds.Length; i++)
                {
                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    model.UserId = Helper.oUser.UserId;
                    model.SrcRecordId = SrcIds[i];
                    Message = fac.StewReMatchBadInputData(model);
                }
                if (Message != "" && Message.Contains("Data ReMatch Request Completed Successfully"))
                {
                    return new JsonResult { Data = Message };
                }
                else
                {
                    return new JsonResult { Data = Message };
                }
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgSomethingWentWrong };
            }
        }
        #endregion

        #region "Filter Clean Data"
        public ActionResult FilterCleanData(List<FilterData> filters)
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
                else if (item.FieldName == "ErrorCode")
                    filtermodel.ErrorCode = item.FilterValue;
            }
            filtermodel.UserId = Helper.oUser.UserId;
            int pagenumber = 0;
            if (string.IsNullOrEmpty(Convert.ToString(SessionHelper.BadInputData_IsFirstTimeFilter)) || !SessionHelper.BadInputData_IsFirstTimeFilter)
            {
                UserSessionFacade fac = new UserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.InsertOrUpdateUserSessionFilter(filtermodel);
                pagenumber = 1;
            }
            else
            {
                if (SessionHelper.BadInputData_IsFirstTimeFilter)
                    pagenumber = 1;
                else
                {
                    if (!string.IsNullOrEmpty(SessionHelper.CleanMatchPage))
                        pagenumber = Convert.ToInt32(SessionHelper.CleanMatchPage);
                    else
                        pagenumber = 1;
                }
            }
            SessionHelper.BadInputData_IsFirstTimeFilter = false;

            int totalCount = 0;
            int pagevalue = 0;

            if (!string.IsNullOrEmpty(SessionHelper.CleanMatchPageNo))
                pagevalue = Convert.ToInt32(SessionHelper.CleanMatchPageNo);
            if (pagevalue == null || pagevalue == 0)
            {
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                pagevalue = Convert.ToInt32(sfac.GetDefaultPageSize(Helper.oUser.UserId, "BadInputData"));
                pagevalue = pagevalue == 0 ? 10 : pagevalue;
            }
            pagevalue = pagevalue < 5 ? 5 : pagevalue;

            BadInputDataModel model = new BadInputDataModel();
            CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            Tuple<List<CompanyEntity>, string> tuplecompany = cfac.GetBIDCompany(Helper.oUser.UserId, 1, pagevalue, out totalCount);
            model.Companies = tuplecompany.Item1;
            SessionHelper.CleanQueueMessage = tuplecompany.Item2;

            SessionHelper.CleanMatchPage = pagenumber.ToString();
            SessionHelper.CleanMatchPageNo = Convert.ToString(pagevalue);
            SessionHelper.CleanTotalCount = Convert.ToString(totalCount);
            SessionHelper.CleanMatchedCompany = JsonConvert.SerializeObject(model.Companies);
            IPagedList<CompanyEntity> pagedCompany = new StaticPagedList<CompanyEntity>(model.Companies.ToList(), pagenumber, pagevalue, totalCount);// Set model of Company entity to pass this model to view.
            return PartialView("_Index", pagedCompany);
        }
        #endregion
    }
}