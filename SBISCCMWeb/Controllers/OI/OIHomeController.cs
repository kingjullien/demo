using MongoDB.Bson;
using MongoDB.Driver;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, OrbLicenseEnabled, ValidateAccount, TwoStepVerification]
    public class OIHomeController : BaseController
    {
        // GET: OrbHome
        [Route("OI/Home"), HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            OIDashboard model = new OIDashboard();
            //Set Data Queue count
            model = GetOIDashboard();
            try
            {

                string url = Request.Url.Scheme + "://" + Request.Url.Authority;
                string[] hostParts = new System.Uri(url).Host.Split('.');
                string SubDomain = hostParts[0];

                //set API Usage Count from mongo DB
                string CallType = "CMOI";
                int ResultCode = 200;

                model.MonthCount = Convert.ToString(await GetCurrentMonthLogsCount(SubDomain, CallType, ResultCode));
                model.FormatMonthCount = CommonMethod.FormatNumber(model.MonthCount);
                model.HourlyAPIUsageCount = Convert.ToString(await GetHourlyLogsCount(SubDomain, CallType, ResultCode));
                model.FormatHourlyAPIUsageCount = CommonMethod.FormatNumber(model.HourlyAPIUsageCount);

                #region Cosmos DB Code commented as it is not in use
                //model.AllAPIUsageCount = Convert.ToString(await GetAllLogsCount(SubDomain, CallType, ResultCode));
                //model.FormatAllAPIUsageCount = CommonMethod.FormatNumber(model.AllAPIUsageCount);
                //model.MonthCount = Convert.ToString(await GetMonthLogsCount(SubDomain, CallType, ResultCode));
                //model.FormatMonthCount = CommonMethod.FormatNumber(model.MonthCount);
                //model.YTDCount = Convert.ToString(await GetYearLogsCount(SubDomain, CallType, ResultCode));
                //model.FormatYTDCount = CommonMethod.FormatNumber(model.YTDCount);
                #endregion
            }
            catch (Exception ex)
            {
                model.MonthCount = "0";
                model.FormatMonthCount = "0";
            }

            Helper.CurrentProvider = ProviderType.OI.ToString();
            return View("~/Views/OI/OIHome/Index.cshtml", model);
        }
        public OIDashboard GetOIDashboard()
        {
            //Set Data Queue count
            OIDashboard oIDashboard = new OIDashboard();
            OIDashboardFacade fac = new OIDashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataSet ds = fac.GetDashboardQueueCount();

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    oIDashboard.ImportProcess = dt.Rows[0]["ImportProcess"] is DBNull ? "" : Convert.ToString(dt.Rows[0]["ImportProcess"]);
                    oIDashboard.AllCount = dt.Rows[0]["TotalRecordCount"] is DBNull ? "" : Convert.ToString(dt.Rows[0]["TotalRecordCount"]);
                    oIDashboard.InputRecordCount = dt.Rows[0]["InputRecordCount"] is DBNull ? "" : Convert.ToString(dt.Rows[0]["InputRecordCount"]);
                    oIDashboard.UnMatchRecordCount = dt.Rows[0]["UnMatchedRecordCount"] is DBNull ? "" : Convert.ToString(dt.Rows[0]["UnMatchedRecordCount"]);
                    oIDashboard.MatchedOutputQueueCount = dt.Rows[0]["MatchedOutputQueueCount"] is DBNull ? "" : Convert.ToString(dt.Rows[0]["MatchedOutputQueueCount"]);
                    oIDashboard.ArchivalQueueCount = dt.Rows[0]["ArchivalQueueCount"] is DBNull ? "" : Convert.ToString(dt.Rows[0]["ArchivalQueueCount"]);
                    oIDashboard.FirmographicsExportQueueCount = dt.Rows[0]["FirmographicsExportQueueCount"] is DBNull ? "" : Convert.ToString(dt.Rows[0]["FirmographicsExportQueueCount"]);
                    oIDashboard.FormatAllCount = CommonMethod.FormatNumber(oIDashboard.AllCount);
                    oIDashboard.FormatUnMatchRecordCount = CommonMethod.FormatNumber(oIDashboard.UnMatchRecordCount);
                    oIDashboard.FormatMatchedOutputQueueCount = CommonMethod.FormatNumber(oIDashboard.MatchedOutputQueueCount);
                    oIDashboard.FormatArchivalQueueCount = CommonMethod.FormatNumber(oIDashboard.ArchivalQueueCount);
                    oIDashboard.FormatFirmographicsExportQueueCount = CommonMethod.FormatNumber(oIDashboard.FirmographicsExportQueueCount);
                    oIDashboard.FormatInputRecordCount = CommonMethod.FormatNumber(oIDashboard.InputRecordCount);
                }
                else
                {
                    oIDashboard.ImportProcess = "0";
                    oIDashboard.InputRecordCount = "0";
                    oIDashboard.AllCount = "0";
                    oIDashboard.UnMatchRecordCount = "0";
                    oIDashboard.MatchedOutputQueueCount = "0";
                    oIDashboard.ArchivalQueueCount = "0";
                    oIDashboard.FirmographicsExportQueueCount = "0";
                    oIDashboard.FormatAllCount = "0";
                    oIDashboard.FormatUnMatchRecordCount = "0";
                    oIDashboard.FormatMatchedOutputQueueCount = "0";
                    oIDashboard.FormatArchivalQueueCount = "0";
                    oIDashboard.FormatFirmographicsExportQueueCount = "0";
                    oIDashboard.FormatInputRecordCount = "0";

                }
                if (ds.Tables.Count > 1)
                {
                    DataTable dt2 = ds.Tables[1];
                    if (dt2 != null && dt2.Rows != null && dt2.Rows.Count > 0)
                    {
                        oIDashboard.CorporateTreeExportQueueCount = dt2.Rows[0]["CorporateTreeExportQueueCount"] is DBNull ? "" : Convert.ToString(dt2.Rows[0]["CorporateTreeExportQueueCount"]);
                        oIDashboard.FormatCorporateTreeExportQueueCount = CommonMethod.FormatNumber(oIDashboard.CorporateTreeExportQueueCount);
                    }
                }
            }
            return oIDashboard;
        }


        [HttpGet]
        //Get Data Queue Statistics report of  API Usages
        public ActionResult OIAPIUsageStatisticsGrid(bool IsDownload = false)
        {
            OIDashboardFacade fac = new OIDashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataSet ds = fac.GetDashboardQueueCount();
            DataTable dt = new DataTable();
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows != null && dt.Rows.Count != 0)
                {
                    if (dt.Rows[0]["Importdate"] == DBNull.Value)
                    {
                        dt.Rows[0]["Importdate"] = DateTime.Now;
                    }
                    DataRow firstRow = (dt.Rows[0] as DataRow).Copy();
                    dt.Rows.Add(firstRow.ItemArray);
                    dt.Rows.RemoveAt(0);
                }
            }
            if (dt != null && IsDownload)
            {
                string fileName = "OI Data Queue Statistics_" + DateTime.Now.Ticks.ToString() + ".xlsx";
                string SheetName = "OI Data Queue Statistics";
                byte[] response = CommonExportMethods.ExportExcelFile(dt, fileName, SheetName);
                return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                ViewBag.DownloadErrormessage = CommonMessagesLang.msgCommanErrorMessage;
            }
            return View("~/Views/OI/OIHome/OIAPIUsageStatisticsGrid.cshtml", dt);
        }

        public JsonResult OIActiveStatisticsFilter()
        {
            OIDashboard model = GetOIDashboard();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #region "Background Process Statistics"
        [RequestFromAjax]
        public JsonResult OIBackgroundProcessStatistics(string Parameters)
        {
            // get information of Background process on Top Center side 
            OIDashboardFacade fac = new OIDashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataSet dtBackground = fac.GetDashboardBackgroundProcess();
            string dtresult = CommonMethod.DataSetToJSON(dtBackground.Tables[0]);
            string dtStatus = CommonMethod.DataSetToJSON(dtBackground.Tables[1]);
            return Json(new { Data1 = dtresult, Data2 = dtStatus }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "API Usages Statistics"
        [RequestFromAjax, RequestFromSameDomain]
        public async Task<JsonResult> OIAPIUsagestatisticsReFresh()
        {
            OIDashboard model = new OIDashboard();
            //model = GetOIDashboard();
            try
            {
                //set API Usage Count from cosmos DB
                string url = Request.Url.Scheme + "://" + Request.Url.Authority;
                string[] hostParts = new System.Uri(url).Host.Split('.');
                string SubDomain = hostParts[0];
                string CallType = "CMOI";
                int ResultCode = 200;

                /*Set default value to 0*/
                model.MonthCount = Convert.ToString(await GetCurrentMonthLogsCount(SubDomain, CallType, ResultCode));
                model.FormatMonthCount = CommonMethod.FormatNumber(model.MonthCount);
                model.HourlyAPIUsageCount = Convert.ToString(await GetHourlyLogsCount(SubDomain, CallType, ResultCode));
                model.FormatHourlyAPIUsageCount = CommonMethod.FormatNumber(model.HourlyAPIUsageCount);
            }
            catch (Exception ex)
            {
                //Empty catch block to stop from breaking
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Mongo DB call for get API Usage Statistics data
        // For getting last 24 hours API Usage
        public async Task<int> GetHourlyLogsCount(string subDomain, string callType, int ResultCode)
        {
            MongoClient client = new MongoClient(ConfigurationManager.AppSettings["MongoConnectionString"]);
            var db = client.GetDatabase(ConfigurationManager.AppSettings["MongoDatabase"]);
            var collection = db.GetCollection<MatchbookAPILogs>(ConfigurationManager.AppSettings["MongoCollection"]);
            var aggregate = collection.Aggregate().Match(p => p.Subdomain == subDomain && p.CallType == callType && p.ResultCode == ResultCode && p.TimePeriod > DateTime.UtcNow.AddHours(-24))
                                      .Group(new BsonDocument { { "_id", "" }, { "Count", new BsonDocument("$sum", "$Count") } }).FirstOrDefault();
            if (aggregate != null)
            {
                var cnt = aggregate.Elements.LastOrDefault().Value.ToInt32();
                return cnt;
            }
            else
            {
                var cnt = 0;
                return cnt;
            }
        }

        // For getting current month API Usage
        public async Task<int> GetCurrentMonthLogsCount(string subDomain, string callType, int ResultCode)
        {
            MongoClient client = new MongoClient(ConfigurationManager.AppSettings["MongoConnectionString"]);
            var db = client.GetDatabase(ConfigurationManager.AppSettings["MongoDatabaseCurrentMonth"]);
            var collection = db.GetCollection<MatchbookAPILogs>(ConfigurationManager.AppSettings["MongoCollectionCurrentMonth"]);
            var aggregate = collection.Aggregate().Match(p => p.Subdomain == subDomain && p.CallType == callType && p.ResultCode == ResultCode && p.TimePeriod > DateTime.UtcNow.AddMonths(-1))
                                      .Group(new BsonDocument { { "_id", "" }, { "Count", new BsonDocument("$sum", "$Count") } }).FirstOrDefault();
            if (aggregate != null)
            {
                var cnt = aggregate.Elements.LastOrDefault().Value.ToInt32();
                return cnt;
            }
            else
            {
                var cnt = 0;
                return cnt;
            }
        }
        #endregion
    }
}