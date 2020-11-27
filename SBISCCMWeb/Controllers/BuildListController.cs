using Newtonsoft.Json;
using PagedList;
using SBISCCMWeb.Extensions;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCCMWeb.Utility.BuildList;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Controllers
{
    [TwoStepVerification, Authorize, AllowLicense, DandBLicenseEnabled, AllowDataStewardshipLicense]
    public class BuildListController : BaseController
    {
        #region Search
        public ActionResult Index(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            try
            {
                #region  pagination
                if (!(sortby.HasValue && sortby.Value > 0))
                    sortby = 1;

                if (!(sortorder.HasValue && sortorder.Value > 0))
                    sortorder = 2;

                int currentPageIndex = page.HasValue ? page.Value : 1;
                int pageSize = pagevalue.HasValue ? pagevalue.Value : 20;
                #endregion

                #region Set Viewbag
                ViewBag.SortBy = sortby;
                ViewBag.SortOrder = sortorder;
                ViewBag.pageno = currentPageIndex;
                ViewBag.pagevalue = pageSize;
                SessionHelper.BuildList_pageno = Convert.ToString(currentPageIndex);
                #endregion

                List<SearchCandidate> lstsesstion = JsonConvert.DeserializeObject<List<SearchCandidate>>(SessionHelper.BuildList_Data);
                if (lstsesstion != null)
                {
                    var skip = pageSize * (currentPageIndex - 1);
                    IPagedList<SearchCandidate> pglstpagedMonitorProfile = new StaticPagedList<SearchCandidate>(lstsesstion.Skip(skip).Take(pageSize).ToList(), currentPageIndex, pageSize, lstsesstion.Count);
                    return PartialView("~/Views/BuildList/SearchGrid.cshtml", pglstpagedMonitorProfile);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        // GET: BuildList
        public ActionResult Search()
        {
            BuildListSearchModel model = new BuildListSearchModel();
            model.Request = new SearchCriteriaRequest();
            model.Request.numberOfEmployees = new NumberOfEmployees();
            return View(model);
        }
        [HttpPost, ValidateInput(true), RequestFromSameDomain, RequestFromAjax]
        public JsonResult Search(BuildListSearchModel model, int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            Response response = new Response();
            try
            {
                #region  pagination
                if (!(sortby.HasValue && sortby.Value > 0))
                    sortby = 1;

                if (!(sortorder.HasValue && sortorder.Value > 0))
                    sortorder = 2;

                int currentPageIndex = page.HasValue ? page.Value : 1;
                int pageSize = pagevalue.HasValue ? pagevalue.Value : 20;
                #endregion

                #region Set Viewbag
                ViewBag.SortBy = sortby;
                ViewBag.SortOrder = sortorder;
                ViewBag.pageno = currentPageIndex;
                ViewBag.pagevalue = pageSize;
                SessionHelper.BuildList_pageno = Convert.ToString(currentPageIndex);
                #endregion

                Utility.Utility api = new Utility.Utility();
                model.Request.pageSize = ViewBag.pagevalue;
                model.Request.pageNumber = ViewBag.pageno;
                if (model.Request.usSicV4.Any())
                {
                    model.Request.usSicV4 = null;
                }

                if (model.Request.registrationNumbers.Any())
                {
                    model.Request.registrationNumbers = null;
                }
                model.Request.businessEntityType = null;
                model.Request.familytreeRolesPlayed = null;

                if (model.Request.locationRadius.lat == 0 && model.Request.locationRadius.lon == 0 && model.Request.locationRadius.radius == 0 && string.IsNullOrEmpty(model.Request.locationRadius.unit))
                {
                    model.Request.locationRadius = null;
                }

                if (!model.Request.yearlyRevenue.maximumValue.HasValue && !model.Request.yearlyRevenue.minimumValue.HasValue)
                {
                    model.Request.yearlyRevenue = null;
                }

                if (model.Request.globalUltimateFamilyTreeMembersCount.maximumValue == 0 && model.Request.globalUltimateFamilyTreeMembersCount.minimumValue == 0)
                {
                    model.Request.globalUltimateFamilyTreeMembersCount = null;
                }

                if (model.Request.numberOfEmployees.informationScope == 0 && model.Request.numberOfEmployees.maximumValue == null && model.Request.numberOfEmployees.minimumValue == null)
                {
                    model.Request.numberOfEmployees = null;
                }
                if (model.Request.registrationNumbers.Count == 0)
                {
                    model.Request.registrationNumbers = null;
                }
                if (model.Request.usSicV4.Count == 0)
                {
                    model.Request.usSicV4 = null;
                }
                DateTime dtRequestedTime = DateTime.Now;
                DateTime? dtResponseTime = null;
                bool IsFetchData = true;
                List<SearchCandidate> lstsesstion = new List<SearchCandidate>();
                // Checking APIType is there or not
                string APItype = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_BUILD_A_LIST.ToString(), ThirdPartyProperties.APIType.ToString());
                if (APItype != "")
                {
                    for (int i = 1; i <= (model.NoOfRecored / 20); i++)
                    {
                        if (IsFetchData)
                        {
                            model.Request.pageNumber = i;
                            model.Request.pageSize = 20;

                            SearchCriteriaResponse objResponse = api.BuildAList(model.Request);
                            if (objResponse.searchCandidates != null)
                            {
                                lstsesstion.AddRange(objResponse.searchCandidates);
                                if ((i * 20) >= objResponse.candidatesMatchedQuantity)
                                {
                                    IsFetchData = false;
                                }
                            }
                            else
                            {
                                if (i == 1)
                                {
                                    response.Success = false;
                                    response.ResponseString = objResponse.error != null ? objResponse.error.errorMessage : string.Empty;
                                }
                                IsFetchData = false;
                            }

                        }
                    }
                }

                dtResponseTime = DateTime.Now;

                if (lstsesstion.Any())
                {
                    SessionHelper.BuildList_Data = JsonConvert.SerializeObject(lstsesstion);
                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    var RequestJson = new JavaScriptSerializer().Serialize(model);
                    var ResponseJson = new JavaScriptSerializer().Serialize(lstsesstion);
                    object obj = fac.InsertBuildSearch(Helper.oUser.UserId, RequestJson, ResponseJson, dtRequestedTime, dtResponseTime);
                    Session["Id"] = Convert.ToInt64(obj);
                }

                if (lstsesstion.Any())
                {
                    var skip = pageSize * (currentPageIndex - 1);
                    IPagedList<SearchCandidate> pglstpagedMonitorProfile = new StaticPagedList<SearchCandidate>(lstsesstion.Skip(skip).Take(pageSize).ToList(), currentPageIndex, pageSize, lstsesstion.Count);
                    dtResponseTime = DateTime.Now;
                    response.Success = true;
                    response.ResponseString = RenderViewAsString.RenderPartialViewToString(this, "~/Views/BuildList/SearchGrid.cshtml", pglstpagedMonitorProfile);
                }
                else
                {
                    if (APItype == "")
                    {
                        response.Success = false;
                        response.ResponseString = CommonMessagesLang.msgNoDefaultKeyForSearch;
                    }
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponseString = ex.Message;
                return Json(response);
            }
        }
        #endregion

        #region Add Registration

        #endregion 
        public ActionResult AddRegistration()
        {
            return View("~/Views/BuildList/AddRegistration.cshtml");
        }
        public ActionResult AddusSicv4()
        {
            return View("~/Views/BuildList/AddusSicv4.cshtml");
        }
        #region History - Popup
        public ActionResult GetSearchHistory()
        {
            try
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                var lst = fac.GetSearchListResults(Helper.oUser.UserId).ToListof<SearchListEntity>();
                List<BuildListSearchModel> lstmodel = new List<BuildListSearchModel>();
                var serializer = new JavaScriptSerializer();
                foreach (SearchListEntity item in lst)
                {
                    BuildListSearchModel model = new BuildListSearchModel();
                    BuildListSearchModel obj = serializer.Deserialize<BuildListSearchModel>(item.RequestJson);
                    model.Request = obj.Request;
                    model.SearchResultsId = item.SearchResultsId;
                    model.RequestJson = item.RequestJson;
                    model.ResponseJson = item.ResponseJson;
                    model.RequestedDateTime = item.RequestDateTime == null ? string.Empty : Convert.ToDateTime(item.RequestDateTime).ToDatetimeFull();
                    lstmodel.Add(model);
                }
                return View("~/Views/BuildList/SearchHistory.cshtml", lstmodel);
            }
            catch (Exception)
            {
                return null;
            }
        }
        //public ActionResult HistoryIndex(int? page, int? sortby, int? sortorder, int? pagevalue)
        //{
        //    #region  pagination
        //    int pageNumber = (page ?? 1);
        //    if (!(sortby.HasValue && sortby.Value > 0))
        //        sortby = 1;

        //    if (!(sortorder.HasValue && sortorder.Value > 0))
        //        sortorder = 2;

        //    int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
        //    int totalCount = 0;
        //    int currentPageIndex = page.HasValue ? page.Value : 1;
        //    int pageSize = pagevalue.HasValue ? pagevalue.Value : 20;
        //    #endregion

        //    #region Set Viewbag
        //    ViewBag.HistorySortBy = sortby;
        //    ViewBag.HistorySortOrder = sortorder;
        //    ViewBag.Historypageno = currentPageIndex;
        //    ViewBag.Historypagevalue = pageSize;
        //    TempData["Historypageno"] = currentPageIndex;
        //    TempData["Historypagevalue"] = pageSize;
        //    #endregion

        //    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
        //    //string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);
        //    var lst = fac.GetSearchListResults(Helper.oUser.UserId, sortParam, currentPageIndex, pageSize, out totalCount).ToListof<SearchListEntity>();
        //    List<BuildListSearchModel> lstmodel = new List<BuildListSearchModel>();
        //    var serializer = new JavaScriptSerializer();
        //    foreach (SearchListEntity item in lst)
        //    {
        //        BuildListSearchModel model = new BuildListSearchModel();
        //        SearchCriteriaRequest obj = serializer.Deserialize<SearchCriteriaRequest>(item.RequestJson);
        //        model.Request = obj;
        //        model.SearchResultsId = item.SearchResultsId;
        //        model.RequestJson = item.RequestJson;
        //        model.ResponseJson = item.ResponseJson;
        //        lstmodel.Add(model);
        //    }
        //    IPagedList<BuildListSearchModel> pglstpagedMonitorProfile = new StaticPagedList<BuildListSearchModel>(lstmodel, currentPageIndex, pageSize, totalCount);
        //    return View("~/Views/BuildList/SearchHistory.cshtml", pglstpagedMonitorProfile);
        //}
        #endregion

        #region Select record from Popup
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateInput(true)]
        public JsonResult ViewHistory(long Id, int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            Response response = new Response();
            try
            {
                #region  pagination
                if (!(sortby.HasValue && sortby.Value > 0))
                    sortby = 1;

                if (!(sortorder.HasValue && sortorder.Value > 0))
                    sortorder = 2;

                int currentPageIndex = page.HasValue ? page.Value : 1;
                int pageSize = pagevalue.HasValue ? pagevalue.Value : 20;
                #endregion

                #region Set Viewbag
                ViewBag.SortBy = sortby;
                ViewBag.SortOrder = sortorder;
                ViewBag.pageno = currentPageIndex;
                ViewBag.pagevalue = pageSize;
                SessionHelper.BuildList_pageno = Convert.ToString(currentPageIndex);
                #endregion


                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                var model = fac.ViewHistory(Id).ToListof<SearchListEntity>();
                var serializer = new JavaScriptSerializer();
                List<SearchCandidate> lstsesstion = serializer.Deserialize<List<SearchCandidate>>(model.FirstOrDefault().ResponseJson);
                SessionHelper.BuildList_Data = JsonConvert.SerializeObject(lstsesstion);
                Session["Id"] = Convert.ToInt64(Id);
                var skip = pageSize * (currentPageIndex - 1);
                IPagedList<SearchCandidate> pglstpagedMonitorProfile = new StaticPagedList<SearchCandidate>(lstsesstion.Skip(skip).Take(pageSize).ToList(), currentPageIndex, pageSize, lstsesstion.Count);
                response.Success = true;
                response.ResponseString = RenderViewAsString.RenderPartialViewToString(this, "~/Views/BuildList/SearchGrid.cshtml", pglstpagedMonitorProfile);
                response.Object = model;
                return Json(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponseString = ex.Message;
                return Json(response);
            }
        }
        #endregion

        #region "Download Excel"
        public ActionResult ExportExcel()
        {
            long searchResultId = Convert.ToInt64(Session["Id"]);
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            var data = fac.ExportBuildResult(searchResultId);
            if (searchResultId <= 0)
            {
                data = new System.Data.DataTable();
            }
            string fileName = "BuildList_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            string SheetName = "BuildList";
            byte[] response = CommonExportMethods.ExportExcelFile(data, fileName, SheetName);
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        #endregion
    }
}