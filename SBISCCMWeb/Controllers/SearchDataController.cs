using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Models.PreviewMatchData.Main;
using SBISCCMWeb.Utility;
using SBISCCMWeb.Utility.IdentityResolution;
using SBISCCMWeb.Utility.SearchByDomain;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, DandBLicenseEnabled, AllowDataStewardshipLicense]
    public class SearchDataController : BaseController
    {
        // GET: SearchData
        #region "Variable declaration"     
        APIResponse response;
        #endregion

        #region "Initialization" 
        public ActionResult Index()
        {
            MainMatchEntity matches = new MainMatchEntity();
            SearchModel objmodel = new SearchModel();
            if (matches.lstMatches == null)
            {
                matches.lstMatches = new List<MatchEntity>();
            }
            Tuple<MainMatchEntity, SearchModel> tuple = new Tuple<MainMatchEntity, SearchModel>(matches, objmodel);
            return PartialView("Index", tuple);
        }

        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        //Get data depends on searching criteria
        public ActionResult Index(SearchModel objmodel)
        {
            Helper.CompanyName = Convert.ToString(objmodel.CompanyName);
            Helper.Address = Convert.ToString(objmodel.Address);
            Helper.City = Convert.ToString(objmodel.City);
            Helper.State = Convert.ToString(objmodel.State);
            Helper.PhoneNbr = Convert.ToString(objmodel.PhoneNbr);
            Helper.Zip = Convert.ToString(objmodel.Zip);
            Helper.Address1 = Convert.ToString(objmodel.Address2);
            //List<MatchEntity> matches = new List<MatchEntity>();
            MainMatchEntity objmainMatchEntity = new MainMatchEntity();
            //if (objmodel.CompanyName != null && objmodel.Country != null)
            //{
            CommonSearchData common = new CommonSearchData();
            //use common method for get data
            objmainMatchEntity = common.LoadData(objmodel.CompanyName, objmodel.Address, objmodel.Address2, objmodel.City, objmodel.State, objmodel.Country, objmodel.Zip, objmodel.PhoneNbr, objmodel.ExcludeNonHeadQuarters, objmodel.ExcludeNonMarketable, objmodel.ExcludeOutofBusiness, objmodel.ExcludeUndeliverable, objmodel.ExcludeUnreachable, objmodel.Language, null, this.CurrentClient.ApplicationDBConnectionString, null);

            #region Tracking Events Configuration-Pendo API
            Pendo model = new Pendo();
            model.properties = new Properties();
            model.context = new Context();
            Utility.PendoAPI api = new Utility.PendoAPI();
            // call Pendo API
            model = api.pendoAPI(PendoEvents.SearchData.ToString());
            #endregion

            if (objmainMatchEntity.lstMatches == null)
            {
                objmainMatchEntity.lstMatches = new List<MatchEntity>();
            }
            SessionHelper.SearchMatch = JsonConvert.SerializeObject(objmainMatchEntity.lstMatches);
            SessionHelper.SearchModel = JsonConvert.SerializeObject(objmodel);
            if (Request.IsAjaxRequest())
            {
                ViewBag.SearchedRegNum = "";
                ViewBag.SearchedWebsite = "";
                return PartialView("_Index", objmainMatchEntity);
            }
            else
            {
                Tuple<MainMatchEntity, SearchModel> tuple = new Tuple<MainMatchEntity, SearchModel>(objmainMatchEntity, objmodel);
                return PartialView("Index", tuple);
            }
        }
        #endregion

        #region "Sorting Grid"
        //Sorting data List for Grid
        [Route("GetSortingList/{sortby?}/{sortorder?}")]
        public ActionResult GetSortingList(int? sortby, int? sortorder)
        {

            List<MatchEntity> matches = new List<MatchEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.SearchMatch))
            {
                matches = JsonConvert.DeserializeObject<List<MatchEntity>>(SessionHelper.SearchMatch);
            }
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            if (matches.Count > 0 && sortby != null && sortorder != null)
            {
                // sort order 1 for ascending order and 2 for descending order.
                if (sortorder == 1)
                {
                    switch (sortby)
                    {
                        case 1:
                            matches = matches.OrderBy(x => x.DnBDUNSNumber).ToList();
                            break;
                        case 2:
                            matches = matches.OrderBy(x => x.DnBOrganizationName).ToList();
                            break;
                        case 3:
                            matches = matches.OrderBy(x => x.StreetNo).ToList();
                            break;
                        case 4:
                            matches = matches.OrderBy(x => x.StreetName).ToList();
                            break;
                        case 5:
                            matches = matches.OrderBy(x => x.DnBPrimaryTownName).ToList();
                            break;
                        case 6:
                            matches = matches.OrderBy(x => x.DnBTerritoryAbbreviatedName).ToList();
                            break;
                        case 7:
                            matches = matches.OrderBy(x => x.DnBPostalCode).ToList();
                            break;
                        case 8:
                            matches = matches.OrderBy(x => x.DnBTelephoneNumber).ToList();
                            break;
                        case 9:
                            matches = matches.OrderBy(x => x.DnBOperatingStatus).ToList();
                            break;
                        case 10:
                            matches = matches.OrderBy(x => x.DnBFamilyTreeMemberRole).ToList();
                            break;
                        case 11:
                            matches = matches.OrderBy(x => int.Parse(x.DnBDisplaySequence)).ToList();
                            break;
                    }
                }
                else
                {
                    switch (sortby)
                    {
                        case 1:
                            matches = matches.OrderByDescending(x => x.DnBDUNSNumber).ToList();
                            break;
                        case 2:
                            matches = matches.OrderByDescending(x => x.DnBOrganizationName).ToList();
                            break;
                        case 3:
                            matches = matches.OrderByDescending(x => x.StreetNo).ToList();
                            break;
                        case 4:
                            matches = matches.OrderByDescending(x => x.StreetName).ToList();
                            break;
                        case 5:
                            matches = matches.OrderByDescending(x => x.DnBPrimaryTownName).ToList();
                            break;
                        case 6:
                            matches = matches.OrderByDescending(x => x.DnBTerritoryAbbreviatedName).ToList();
                            break;
                        case 7:
                            matches = matches.OrderByDescending(x => x.DnBPostalCode).ToList();
                            break;
                        case 8:
                            matches = matches.OrderByDescending(x => x.DnBTelephoneNumber).ToList();
                            break;
                        case 9:
                            matches = matches.OrderByDescending(x => x.DnBOperatingStatus).ToList();
                            break;
                        case 10:
                            matches = matches.OrderByDescending(x => x.DnBFamilyTreeMemberRole).ToList();
                            break;
                        case 11:
                            matches = matches.OrderByDescending(x => int.Parse(x.DnBDisplaySequence)).ToList();
                            break;

                    }
                }
            }

            MainMatchEntity mainMatchEntity = new MainMatchEntity();
            mainMatchEntity.lstMatches = matches;
            int currentPageIndex = Convert.ToInt32(TempData["SearchcurrentPageIndex"]);
            int pageno = Convert.ToInt32(TempData["Searchpageno"]);
            SessionHelper.SearchMatch = JsonConvert.SerializeObject(matches);
            int totalCount = 0;
            TempData.Keep();
            if (Request.IsAjaxRequest())
            {
                ViewBag.SearchedRegNum = "";
                ViewBag.SearchedWebsite = "";
                return PartialView("_Index", mainMatchEntity);
            }
            else
            {
                return View("Index", mainMatchEntity);
            }
        }
        #endregion

        #region "Open Match Detail Review Popup"
        //Open Matched Item Detail View with all Parameter pass with serialization form and set next and prevue data to manage next and prevue functionality.
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
            ViewBag.SelectData = "SearchData";
            List<MatchEntity> matches = new List<MatchEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.SearchMatch))
            {
                matches = JsonConvert.DeserializeObject<List<MatchEntity>>(SessionHelper.SearchMatch);
            }
            Match = matches.Where(x => x.DnBDUNSNumber == childButtonId).FirstOrDefault();
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
            ViewBag.childButtonId = id;
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
        #endregion

        #region "Search By DUNS Number"
        //Search By DUNS Number
        public ActionResult UpdateSearchData(string Parameters /*string DUNSNO, string FromPage, string SrcRecId,string InputId*/)
        {
            MainMatchEntity mainMatchEntity = new MainMatchEntity();
            mainMatchEntity.lstMatches = new List<MatchEntity>();
            string DUNSNO = string.Empty, FromPage = string.Empty, SrcRecId = string.Empty, InputId = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                DUNSNO = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                FromPage = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                SrcRecId = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                InputId = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));

            }
            //Resert values for Display Input field in match detail view(popup)
            Helper.CompanyName = "";
            Helper.Address = "";
            Helper.City = "";
            Helper.State = "";
            Helper.PhoneNbr = "";
            Helper.Zip = "";
            Helper.Address1 = "";

            CommonSearchData objCommonSearch = new CommonSearchData();
            CommonMethod objCommon = new CommonMethod();
            response = objCommonSearch.SearchByDUNS(DUNSNO, this.CurrentClient.ApplicationDBConnectionString);
            mainMatchEntity.ResponseErroeMessage = Helper.ResponseErroeMessage;
            if (response != null)
            {
                objCommon.InsertAPILogs(response.TransactionResponseDetail, this.CurrentClient.ApplicationDBConnectionString);
                CompanyFacade fcd = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
                fcd.InsertCleanseMatchCallResults(SrcRecId, response.ResponseJSON, response.APIRequest, Helper.oUser.UserId, InputId);
                int Count = Convert.ToInt32(response.MatchEntities.Count()) + 1;
                SessionHelper.SearchMatches = JsonConvert.SerializeObject(response.MatchEntities);
                SessionHelper.SearchMatch = JsonConvert.SerializeObject(response.MatchEntities);
                //List<MatchEntity> lstMatchEntity = new List<MatchEntity>(response.MatchEntities);
                mainMatchEntity.lstMatches = response.MatchEntities;
            }
            if (!string.IsNullOrEmpty(response?.ResponseJSON))
            {
                dynamic data = JObject.Parse(response.ResponseJSON);
                if (data.error != null && !string.IsNullOrEmpty(data.error.errorMessage.Value))
                {
                    mainMatchEntity.ResponseErroeMessage = data.error.errorMessage.Value;
                }
            }
            if (FromPage.ToLower() == "searchdata")
            {
                ViewBag.SearchedRegNum = "";
                ViewBag.SearchedWebsite = "";
                return PartialView("_Index", mainMatchEntity);
            }
            else
            {
                return PartialView("~/Views/BadInputData/_SearchData.cshtml", mainMatchEntity);
            }
        }
        #endregion

        #region "Bing search"
        //view bing search form to search data 
        public async Task<ActionResult> BingSearch(string Parameters)
        {
            string SearchValue = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                SearchValue = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    SearchValue = HttpUtility.UrlDecode(SearchValue);
                }
            }
            ViewBag.SearchValue = SearchValue;
            List<webSearch> SearchResults = new List<webSearch>();
            try
            {
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    var res = await CommonMethod.MakeRequest(SearchValue);

                    for (int i = 0; i < res.Count(); i++)
                    {
                        BingSearchModel bingser = res.ElementAt(i);
                        SearchResults.Add(new webSearch { WSer = bingser });
                    }
                }
            }
            catch (Exception ex)
            {
                return View(SearchResults);
            }
            return View(SearchResults);
        }
        [HttpPost, ValidateInput(true)]
        public async Task<ActionResult> BingSearch(string SearchValue, string btnSearch)
        {
            //bing search 
            List<webSearch> SearchResults = new List<webSearch>();
            try
            {
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    //send request for bing search 
                    var res = await CommonMethod.MakeRequest(SearchValue);

                    for (int i = 0; i < res.Count(); i++)
                    {
                        BingSearchModel bingser = res.ElementAt(i);
                        SearchResults.Add(new webSearch { WSer = bingser });
                    }
                }
                ViewBag.SearchValue = SearchValue;
            }
            catch (Exception ex)
            {
                return PartialView("_bingSearch", SearchResults);
            }
            return PartialView("_bingSearch", SearchResults);
        }
        #endregion

        #region "Search by Domain or Email"
        public ActionResult DomainOrEmailPopup(string Parameters, bool IsCleanSearch)
        {
            string type = "";
            string searchvalue = "";
            string SrcRecId = "";
            string InputId = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                type = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                searchvalue = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                SrcRecId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                InputId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
            }

            //Search by Domain or Email
            #region "Old Code"
            //ViewBag.type = type == null ? "domain" : type;
            //ViewBag.searchvalue = searchvalue == null ? "" : searchvalue;
            //ViewBag.SrcRecId = SrcRecId == null ? "" : SrcRecId;
            //ViewBag.InputId = InputId == null ? "" : InputId;
            //ViewBag.IsCleanSearch = IsCleanSearch;
            #endregion

            #region "Vijay - Removed viewbag and used model"
            DomainOrEmailPopupViewModel viewModel = new DomainOrEmailPopupViewModel();
            viewModel.type = string.IsNullOrEmpty(type) ? "domain" : type;
            viewModel.searchvalue = string.IsNullOrEmpty(searchvalue) ? "" : searchvalue;
            viewModel.SrcRecId = string.IsNullOrEmpty(SrcRecId) ? "" : SrcRecId;
            viewModel.InputId = string.IsNullOrEmpty(InputId) == null ? "" : InputId;
            viewModel.IsCleanSearch = IsCleanSearch;
            #endregion
            return View(viewModel);
        }

        //Search by Domain or Email
        [HttpPost, ValidateInput(true)]
        public ActionResult DomainOrEmailPopup(string Parameters)
        {
            MainMatchEntity mainMatchEntity = new MainMatchEntity();
            mainMatchEntity.lstMatches = new List<MatchEntity>();
            //Resert values for Display Input field in match detail view(popup)
            Helper.CompanyName = "";
            Helper.Address = "";
            Helper.City = "";
            Helper.State = "";
            Helper.PhoneNbr = "";
            Helper.Zip = "";
            Helper.Address1 = "";

            string searchValue = string.Empty, type = string.Empty, CountryCode = string.Empty, SrcRecId = string.Empty, InputId = string.Empty;
            bool IsCleanSearch = false;
            CompanyFacade fcd = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                searchValue = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                type = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                IsCleanSearch = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                CountryCode = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));
                SrcRecId = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));
                InputId = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1));

            }
            CommonMethod objCommon = new CommonMethod();
            Utility.Utility api = new Utility.Utility();
            var objResult = objCommon.LoadCleanseMatchSettings(this.CurrentClient.ApplicationDBConnectionString);

            bool IsGlobal = true;
            string LOBTag = null;
            int PageSize = 10, PageNumber = 1, SortOrder = 0;
            int totalCount = 0;
            DataTable lstThirdPartyAPICredentials = new DataTable();
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            lstThirdPartyAPICredentials = fac.GetMinConfidenceSettingsListPaging(IsGlobal, LOBTag);
            int confidenceLowerLevelThresholdValue = 0;
            foreach (DataRow row in lstThirdPartyAPICredentials.Rows)
            {
                confidenceLowerLevelThresholdValue = Convert.ToInt32(row["MinConfidenceCode"]);
            }

            List<MatchEntity> lstMatchEntity = new List<MatchEntity>();
            IPagedList<MatchEntity> pagedSearch = new StaticPagedList<MatchEntity>(lstMatchEntity, 1, 1, 1);
            string APItype = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.APIType.ToString());
            try // Validate Api Family is DirectPlus or Direct20 than according to API family call method for response.
            {

                if (APItype.ToLower() == ApiLayerType.Directplus.ToString().ToLower())
                {
                    response = api.SearchByDomainOrEmail(searchValue, type, this.CurrentClient.ApplicationDBConnectionString, confidenceLowerLevelThresholdValue, CountryCode);
                }
                else if (APItype.ToLower() == ApiLayerType.Direct20.ToString().ToLower())
                {
                    response = api.GetMatchByDomainOrEmail(searchValue, type, this.CurrentClient.ApplicationDBConnectionString, confidenceLowerLevelThresholdValue, CountryCode);
                }
                else
                {
                    mainMatchEntity.ResponseErroeMessage = CommonMessagesLang.msgNoDefaultKeyForSearch;
                }
            }
            catch (WebException webEx)
            {
                //TempData["DomainErrorMessage"] = "No match found.";
                using (var stream = webEx.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();

                        if (APItype.ToLower() == ApiLayerType.Directplus.ToString().ToLower())
                        {
                            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                            IdentityResolutionResponse objResponse = JsonConvert.DeserializeObject<IdentityResolutionResponse>(result, settings);
                            if (objResponse != null)
                            {
                                mainMatchEntity.ResponseErroeMessage = objResponse.error.errorMessage;
                                objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.transactionDetail.transactionID, Convert.ToDateTime(objResponse.transactionDetail.transactionTimestamp), null, null, objResponse.error.errorMessage, null, 0, null);
                            }
                        }
                        else if (APItype.ToLower() == ApiLayerType.Direct20.ToString().ToLower())
                        {
                            GetCleanseMatchResponseMain objResponse = serializer.Deserialize<GetCleanseMatchResponseMain>(result);
                            if (objResponse != null && objResponse.GetCleanseMatchResponse != null && objResponse.GetCleanseMatchResponse.TransactionResult != null)
                            {
                                mainMatchEntity.ResponseErroeMessage = objResponse.GetCleanseMatchResponse.TransactionResult.ResultText;
                            }
                        }
                    }
                }
            }
            if (response != null)
            {
                objCommon.InsertAPILogs(response.TransactionResponseDetail, this.CurrentClient.ApplicationDBConnectionString);
                fcd.InsertCleanseMatchCallResults(SrcRecId, response.ResponseJSON, response.APIRequest, Helper.oUser.UserId, InputId);
                int Count = Convert.ToInt32(response.MatchEntities.Count()) + 1;
                SessionHelper.SearchMatch = JsonConvert.SerializeObject(response.MatchEntities);
                SessionHelper.SearchMatches = JsonConvert.SerializeObject(response.MatchEntities);
                //lstMatchEntity = new List<MatchEntity>(response.MatchEntities);
                mainMatchEntity.lstMatches = response.MatchEntities;
            }
            if (IsCleanSearch)
            {
                return PartialView("~/Views/BadInputData/_SearchData.cshtml", mainMatchEntity);
            }
            else
            {
                ViewBag.SearchedRegNum = "";
                ViewBag.SearchedWebsite = type.ToLower() == "domain" ? searchValue : "";
                return PartialView("_Index", mainMatchEntity);
            }
        }
        #endregion

        #region Search by Registration Number
        [HttpPost]
        public ActionResult RegistrationNumberPopup(string Parameters)
        {
            MainMatchEntity mainMatchEntity = new MainMatchEntity();
            mainMatchEntity.lstMatches = new List<MatchEntity>();
            //Resert values for Display Input field in match detail view(popup)
            Helper.CompanyName = "";
            Helper.Address = "";
            Helper.City = "";
            Helper.State = "";
            Helper.PhoneNbr = "";
            Helper.Zip = "";
            Helper.Address1 = "";

            string RegistrationNoValue = string.Empty, type = string.Empty, CountryCode = string.Empty, SrcRecId = string.Empty, InputId = string.Empty;
            bool IsCleanSearch = false;
            CompanyFacade fcd = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                RegistrationNoValue = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                type = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                IsCleanSearch = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                CountryCode = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));
                SrcRecId = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));
                InputId = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1));

            }
            CommonMethod objCommon = new CommonMethod();
            Utility.Utility api = new Utility.Utility();
            var objResult = objCommon.LoadCleanseMatchSettings(this.CurrentClient.ApplicationDBConnectionString);

            bool IsGlobal = true;
            string LOBTag = null;
            int PageSize = 10, PageNumber = 1, SortOrder = 0;
            int totalCount = 0;
            DataTable lstThirdPartyAPICredentials = new DataTable();
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            lstThirdPartyAPICredentials = fac.GetMinConfidenceSettingsListPaging(IsGlobal, LOBTag);
            int confidenceLowerLevelThresholdValue = 0;
            foreach (DataRow row in lstThirdPartyAPICredentials.Rows)
            {
                confidenceLowerLevelThresholdValue = Convert.ToInt32(row["MinConfidenceCode"]);
            }

            List<MatchEntity> lstMatchEntity = new List<MatchEntity>();
            IPagedList<MatchEntity> pagedSearch = new StaticPagedList<MatchEntity>(lstMatchEntity, 1, 1, 1);
            string APItype = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.APIType.ToString());
            try // Validate Api Family is DirectPlus or Direct20 than according to API family call method for response.
            {
                if (APItype.ToLower() == ApiLayerType.Directplus.ToString().ToLower())
                {
                    response = api.GetMatchByRegistrationNoDirectPlus(CountryCode, this.CurrentClient.ApplicationDBConnectionString, RegistrationNoValue);
                }
                else if (APItype.ToLower() == ApiLayerType.Direct20.ToString().ToLower())
                {
                    response = api.GetMatchByRegistrationNo(CountryCode, this.CurrentClient.ApplicationDBConnectionString, RegistrationNoValue);
                }
                else
                {
                    mainMatchEntity.ResponseErroeMessage = CommonMessagesLang.msgNoDefaultKeyForSearch;
                }
            }
            catch (WebException webEx)
            {
                using (var stream = webEx.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();


                        if (APItype.ToLower() == ApiLayerType.Directplus.ToString().ToLower())
                        {
                            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                            IdentityResolutionResponse objResponse = JsonConvert.DeserializeObject<IdentityResolutionResponse>(result, settings);
                            if (objResponse != null)
                            {
                                mainMatchEntity.ResponseErroeMessage = objResponse.error.errorMessage;
                                objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.transactionDetail.transactionID, Convert.ToDateTime(objResponse.transactionDetail.transactionTimestamp), null, null, objResponse.error.errorMessage, null, 0, null);
                            }
                        }
                        else if (APItype.ToLower() == ApiLayerType.Direct20.ToString().ToLower())
                        {
                            GetCleanseMatchResponseMain objResponse = serializer.Deserialize<GetCleanseMatchResponseMain>(result);
                            if (objResponse != null && objResponse.GetCleanseMatchResponse != null && objResponse.GetCleanseMatchResponse.TransactionResult != null)
                            {
                                mainMatchEntity.ResponseErroeMessage = objResponse.GetCleanseMatchResponse.TransactionResult.ResultText;
                            }
                        }
                    }
                }
            }
            if (response != null)
            {
                objCommon.InsertAPILogs(response.TransactionResponseDetail, this.CurrentClient.ApplicationDBConnectionString);
                fcd.InsertCleanseMatchCallResults(SrcRecId, response.ResponseJSON, response.APIRequest, Helper.oUser.UserId, InputId);
                int Count = Convert.ToInt32(response.MatchEntities.Count()) + 1;
                SessionHelper.SearchMatch = JsonConvert.SerializeObject(response.MatchEntities);
                SessionHelper.SearchMatches = JsonConvert.SerializeObject(response.MatchEntities);
                //lstMatchEntity = new List<MatchEntity>(response.MatchEntities);
                mainMatchEntity.lstMatches = response.MatchEntities;

                if (!string.IsNullOrEmpty(response?.ResponseJSON))
                {
                    dynamic data = JObject.Parse(response.ResponseJSON);
                    if (data.error != null && !string.IsNullOrEmpty(data.error.errorMessage.Value))
                    {
                        mainMatchEntity.ResponseErroeMessage = data.error.errorMessage.Value;
                    }
                }
            }

            if (IsCleanSearch)
            {
                return PartialView("~/Views/BadInputData/_SearchData.cshtml", mainMatchEntity);
            }
            else
            {
                ViewBag.SearchedRegNum = RegistrationNoValue;
                ViewBag.SearchedWebsite = "";
                return PartialView("_Index", mainMatchEntity);
            }

        }
        #endregion
        #region Google Map 
        // Google map popup on clicking the location from Search Data
        public ActionResult GoogleMapPopUp()
        {
            MainMatchEntity objmainMatchEntity = new MainMatchEntity();
            objmainMatchEntity.lstMatches = JsonConvert.DeserializeObject<List<MatchEntity>>(SessionHelper.SearchMatch);
            if (objmainMatchEntity.lstMatches == null)
            {
                objmainMatchEntity.lstMatches = new List<MatchEntity>();
            }
            CompanyEntity Company = new CompanyEntity();
            Company.Matches = objmainMatchEntity.lstMatches;

            SearchModel objmodel = new SearchModel();
            objmodel = JsonConvert.DeserializeObject<SearchModel>(SessionHelper.SearchModel);
            if (objmodel != null)
            {
                Company.CompanyName = objmodel.CompanyName;
                Company.Address = objmodel.Address;
                Company.Address1 = objmodel.Address2;
                Company.City = objmodel.City;
                Company.State = objmodel.State;
                Company.PostalCode = objmodel.Zip;
                Company.CountryISOAlpha2Code = objmodel.Country;
                Company.PhoneNbr = objmodel.PhoneNbr;
            }
            TempData.Keep();
            return View("~/Views/StewardshipPortal/GoogleMapPopUp.cshtml", Company);
        }

        public ActionResult ValidateGoogleMapPopUp(string Parameters)
        {
            string address = string.Empty;
            bool isFromSearch = false;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                address = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                isFromSearch = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
            }
            ViewBag.Address = address;
            ViewBag.isFromSearch = isFromSearch;
            return View("GoogleMapPopUp");
        }
        #endregion

        #region Type Ahead Functionality Implementation
        [AllowAnonymous]
        public JsonResult SearchDataCompanyNameTypeAhead(string paramater, string defaultCountryCode)
        {
            string JsonResponse = string.Empty;
            CommonSearchData objSearchData = new CommonSearchData();
            JsonResponse = objSearchData.LoadTypeAheadData(paramater, defaultCountryCode);
            return Json(JsonResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}