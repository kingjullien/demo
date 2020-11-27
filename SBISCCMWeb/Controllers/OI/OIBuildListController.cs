using Newtonsoft.Json;
using PagedList;
using SBISCCMWeb.Extensions;
using SBISCCMWeb.Models;
using SBISCCMWeb.Models.OI;
using SBISCCMWeb.Utility;
using SBISCCMWeb.Utility.BuildList;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Controllers.OI
{
    [Authorize, OrbLicenseEnabled, ValidateAccount, AllowLicense, TwoStepVerification]
    public class OIBuildListController : BaseController
    {
        #region Search
        //[HttpGet]
        //[Route("OI/BuildList")]
        [RequestFromSameDomain, ValidateInput(true)]
        // On changing the page number or from changing the page size dropdown
        public ActionResult Index(int? page, int? pagevalue)
        {
            try
            {
                #region  pagination
                int pageNumber = (page ?? 1);
                int totalCount = 0;
                int currentPageIndex = page.HasValue ? page.Value : 1;
                int pageSize = pagevalue.HasValue ? pagevalue.Value : 30;
                #endregion

                #region Set Viewbag
                ViewBag.pageno = currentPageIndex;
                ViewBag.pagevalue = pageSize;
                #endregion

                OIBuildListSearchModelEntity model = new OIBuildListSearchModelEntity();
                model.request_fields = Session["OIBuildDataRequestFields"] as RequestFields;

                if (page != null && model.request_fields != null)
                {
                    model.request_fields.offset = page.Value;
                }
                else if (page != null && model.request_fields == null)
                {
                    model.request_fields = JsonConvert.DeserializeObject<RequestFields>(Session["OIBuildRequestFields"].ToString());
                    model.request_fields.offset = page.Value;
                    model.request_fields = JsonConvert.DeserializeObject<RequestFields>(Session["ShowFullProfile"].ToString());
                    if (model.request_fields.show_full_profile == true)
                    {
                        model.request_fields.show_full_profile = true;
                        model.request_fields.offset = page.Value;
                    }
                }
                else if (page == null && model.request_fields == null)
                {
                    model.request_fields = JsonConvert.DeserializeObject<RequestFields>(Session["OIBuildRequestFields"].ToString());
                    model.request_fields.offset = 1;
                    model.request_fields = JsonConvert.DeserializeObject<RequestFields>(Session["ShowFullProfile"].ToString());
                    if (model.request_fields.show_full_profile == true)
                    {
                        model.request_fields.show_full_profile = true;
                    }
                }
                else
                    model.request_fields.offset = 1;

                Utility.OI.OIBuildList api = new Utility.OI.OIBuildList();
                string[] hostParts = new System.Uri(Request.Url.AbsoluteUri).Authority.Split('.');
                string SubDomain = hostParts[0];
                OIBuildListSearchModelEntity objResponse = api.OIBuildAList(model, pageSize, SubDomain);
                int pages = objResponse.results_count / pageSize;

                IPagedList<Result> pagedOIBuildListSearch = new StaticPagedList<Result>(objResponse.results, currentPageIndex, pageSize, pages);
                if (objResponse.request_fields.show_full_profile == true)
                {
                    return PartialView("~/Views/OI/OIBuildList/_SearchFullGrid.cshtml", pagedOIBuildListSearch);
                }
                else
                {
                    return PartialView("~/Views/OI/OIBuildList/_SearchGrid.cshtml", pagedOIBuildListSearch);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // GET: BuildList
        [HttpGet, Route("OI/BuildList/Search")]
        // Opens OI Build List Search
        public ActionResult Search()
        {
            Utility.Utility api = new Utility.Utility();
            OIBuildListSearchModelEntity model = new OIBuildListSearchModelEntity();
            model.request_fields = new RequestFields();
            Session["OIBuildDataRequestFields"] = null;
            return View("~/Views/OI/OIBuildList/Search.cshtml", model);
        }
        [HttpPost, ValidateInput(true), RequestFromSameDomain]
        [Route("OI/BuildList/Search")]
        // On searching the results
        public ActionResult Search(OIBuildListSearchModelEntity model, int? page, int? pagevalue)
        {
            Response response = new Response();
            try
            {
                #region  pagination
                int pageNumber = (page ?? 1);
                int currentPageIndex = page.HasValue ? page.Value : 1;
                int pageSize = pagevalue.HasValue ? pagevalue.Value : 30;
                #endregion

                #region Set Viewbag
                ViewBag.pageno = currentPageIndex;
                ViewBag.pagevalue = pageSize;
                #endregion

                string[] hostParts = new System.Uri(Request.Url.AbsoluteUri).Authority.Split('.');
                string SubDomain = hostParts[0];
                Utility.OI.OIBuildList api = new Utility.OI.OIBuildList();
                // API Call for OIBuildAList
                OIBuildListSearchModelEntity objResponse = api.OIBuildAList(model, pageSize, SubDomain);

                int pages = objResponse.results_count / pageSize;
                Session["OIBuildDataRequestFields"] = JsonConvert.SerializeObject(model.request_fields);
                // Used for inserting the records and storing the request and response in the db in BuildListResults table 
                DateTime dtRequestedTime = DateTime.Now;
                DateTime? dtResponseTime = DateTime.Now;
                var RequestJson = new JavaScriptSerializer().Serialize(model.request_fields);
                var ResponseJson = new JavaScriptSerializer().Serialize(objResponse.results);
                var ResultCount = pages;
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                object obj = fac.InsertOIBuildSearch(Helper.oUser.UserId, RequestJson, ResponseJson, dtRequestedTime, dtResponseTime, ResultCount);

                IPagedList<Result> pagedOIBuildListSearch = new StaticPagedList<Result>(objResponse.results, currentPageIndex, pageSize, pages);
                if (objResponse.request_fields.show_full_profile == true)
                {
                    return PartialView("~/Views/OI/OIBuildList/_SearchFullGrid.cshtml", pagedOIBuildListSearch);
                }
                else
                {
                    return PartialView("~/Views/OI/OIBuildList/_SearchGrid.cshtml", pagedOIBuildListSearch);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponseString = ex.Message;
                return Json(response);
            }
        }
        #endregion

        #region History - Popup
        [RequestFromSameDomain, ValidateInput(true)]
        // Opens view history popup based on search
        public ActionResult GetOISearchHistory()
        {
            try
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                var lst = fac.GetOIBuildSearchListResults(Helper.oUser.UserId).ToListof<OISearchListEntity>();
                List<OIBuildListSearchModelEntity> lstmodel = new List<OIBuildListSearchModelEntity>();
                var serializer = new JavaScriptSerializer();
                foreach (OISearchListEntity item in lst)
                {
                    OIBuildListSearchModelEntity model = new OIBuildListSearchModelEntity();
                    RequestFields obj = serializer.Deserialize<RequestFields>(item.RequestJson);
                    model.request_fields = obj;
                    Session["OIBuildRequestFields"] = JsonConvert.SerializeObject(model.request_fields);
                    model.SearchResultsId = item.SearchResultsId;
                    model.RequestJson = item.RequestJson;
                    model.ResponseJson = item.ResponseJson;
                    model.ResultCount = item.ResultCount;
                    model.RequestedDateTime = item.RequestDateTime == null ? string.Empty : Convert.ToDateTime(item.RequestDateTime).ToDatetimeFull();
                    lstmodel.Add(model);
                }
                return View("~/Views/OI/OIBuildList/SearchHistory.cshtml", lstmodel);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Select record from Popup
        // Redirects to the record on selecting it from the popup
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateInput(true)]
        public JsonResult ViewHistory(long Id, int? page, int? pagevalue)
        {
            Response response = new Response();
            try
            {
                #region  pagination
                int pageNumber = (page ?? 1);
                int totalCount = 0;
                int currentPageIndex = page.HasValue ? page.Value : 1;
                int pageSize = pagevalue.HasValue ? pagevalue.Value : 30;
                #endregion

                #region Set Viewbag
                ViewBag.pageno = currentPageIndex;
                ViewBag.pagevalue = pageSize;
                #endregion

                OIBuildListSearchModelEntity modelOIOBuildSearch = new OIBuildListSearchModelEntity();
                modelOIOBuildSearch.request_fields = JsonConvert.DeserializeObject<RequestFields>(Session["OIBuildRequestFields"].ToString());
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                var model = fac.ViewOIHistory(Id).ToListof<OISearchListEntity>();
                var serializer = new JavaScriptSerializer();
                List<Result> lstResult = serializer.Deserialize<List<Result>>(model.FirstOrDefault().ResponseJson);
                int lstResultCount = model.FirstOrDefault().ResultCount;
                RequestFields requestFields = serializer.Deserialize<RequestFields>(model.FirstOrDefault().RequestJson);
                Session["OIExportToExcel"] = JsonConvert.SerializeObject(lstResult);
                Session["OIBuildDataRequestFields"] = JsonConvert.SerializeObject(lstResult);
                Session["OIBuildDataRequestFields"] = JsonConvert.SerializeObject(Convert.ToInt64(Id));
                Session["ShowFullProfile"] = JsonConvert.SerializeObject(requestFields);
                if (requestFields.show_full_profile == true)
                {
                    pageSize = 20;
                }
                else
                {
                    page = lstResult.Count;
                }
                var skip = pageSize * (currentPageIndex - 1);
                IPagedList<Result> pagedOIBuildListSearch = new StaticPagedList<Result>(lstResult.Skip(skip).Take(pageSize).ToList(), currentPageIndex, pageSize, lstResultCount);
                response.Success = true;
                if (requestFields.show_full_profile == true)
                {
                    response.ResponseString = RenderViewAsString.RenderPartialViewToString(this, "~/Views/OI/OIBuildList/_SearchFullGrid.cshtml", pagedOIBuildListSearch);
                }
                else
                {
                    response.ResponseString = RenderViewAsString.RenderPartialViewToString(this, "~/Views/OI/OIBuildList/_SearchGrid.cshtml", pagedOIBuildListSearch);
                }
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

        #region "Export To Excel"
        [RequestFromSameDomain, ValidateInput(true)]
        public ActionResult ExportToExcel()
        {
            List<Result> lstResult = new List<Result>();
            lstResult = JsonConvert.DeserializeObject<List<Result>>(Session["OIExportToExcel"].ToString());

            if (lstResult == null)
            {
                int? pagevalue = null;
                int pageSize = pagevalue.HasValue ? pagevalue.Value : 30;
                OIBuildListSearchModelEntity model = new OIBuildListSearchModelEntity();
                string[] hostParts = new System.Uri(Request.Url.AbsoluteUri).Authority.Split('.');
                string SubDomain = hostParts[0];
                model.request_fields = JsonConvert.DeserializeObject<RequestFields>(Session["OIBuildDataRequestFields"].ToString());
                Utility.OI.OIBuildList api = new Utility.OI.OIBuildList();
                OIBuildListSearchModelEntity objResponse = api.OIBuildAList(model, pageSize, SubDomain);
                Session["OIBuildDataResults"] = JsonConvert.SerializeObject(objResponse.results);
                lstResult = JsonConvert.DeserializeObject<List<Result>>(Session["OIBuildDataResults"].ToString());
            }
            else
            {
                lstResult = JsonConvert.DeserializeObject<List<Result>>(Session["OIExportToExcel"].ToString());
            }

            DataTable data = CommonMethod.ToDataTable(lstResult);

            if (lstResult == null)
            {
                data = new DataTable();
            }
            string fileName = "OIBuildList_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            string SheetName = "OIBuildList";
            byte[] response = CommonExportMethods.ExportExcelFile(data, fileName, SheetName);
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        #endregion
    }
}