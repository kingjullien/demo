using Newtonsoft.Json;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [TwoStepVerification, Authorize, AllowLicense, DandBLicenseEnabled]
    public class ReportsListController : BaseController
    {
        // GET: ReportsList
        public ActionResult Index(string id)
        {
            string UserGroup = "";
            ViewBag.Id = id;
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataTable dtDataQueue = fac.GetDataQueueDashboardReport();
            Session["lstDataQueue"] = dtDataQueue;
            DataSet dtDataStewardStatistics = fac.GetdtDataStewardStatisticsReport(UserGroup);
            SessionHelper.dtDataStewardStatistics = JsonConvert.SerializeObject(dtDataStewardStatistics);

            DataSet dsDataAPIUsage = fac.GetdtAPIReport();
            SessionHelper.dsDataAPIUsage = JsonConvert.SerializeObject(dsDataAPIUsage);

            return View();
        }
        #region "Data Queue Report"
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetDataQueue()
        {
            DataTable dtDataQueue = new DataTable();
            List<DataQueueChart> lstDataQueue = new List<DataQueueChart>();
            if (Session["lstDataQueue"] != null)
            {
                dtDataQueue = Session["lstDataQueue"] as DataTable;
            }
            else
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                dtDataQueue = fac.GetDataQueueDashboardReport();
            }
            for (int i = 0; i < dtDataQueue.Rows.Count; i++)
            {
                DataQueueChart objDataQueue = new DataQueueChart();
                objDataQueue.x = dtDataQueue.Rows[i]["Type"].ToString();
                objDataQueue.y = Convert.ToInt32(dtDataQueue.Rows[i]["Number of Records"].ToString());
                lstDataQueue.Add(objDataQueue);
            }
            return Json(lstDataQueue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DataQueueDashboard()
        {
            // //report for Data Queue Dashboard
            DataTable dtDataQueue = new DataTable();
            if (Session["lstDataQueue"] != null)
            {
                dtDataQueue = Session["lstDataQueue"] as DataTable;
            }
            else
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                dtDataQueue = fac.GetDataQueueDashboardReport();
            }
            return PartialView("DataQueueTitle", dtDataQueue);
        }
        #endregion

        #region "Data Stewardship Statistics Report"
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetDataStewardshipStatisticsByUser(string Parameters = null)
        {
            //report for Data Stewardship Statistics through User
            string UserGroup = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                UserGroup = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }

            DataTable dtDataStewardByuser = new DataTable();
            DataSet dsDataStewardStatistics = new DataSet();
            List<DataStewrdStatisticsChart> lstDataStewrdStatistics = new List<DataStewrdStatisticsChart>();
            if (!string.IsNullOrEmpty(SessionHelper.dtDataStewardStatistics) && string.IsNullOrEmpty(UserGroup))
            {
                dsDataStewardStatistics = JsonConvert.DeserializeObject<DataSet>(SessionHelper.dtDataStewardStatistics);
            }
            else
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                dsDataStewardStatistics = fac.GetdtDataStewardStatisticsReport(UserGroup);
            }
            dtDataStewardByuser = dsDataStewardStatistics.Tables[1];
            for (int i = 0; i < dtDataStewardByuser.Rows.Count; i++)
            {
                DataStewrdStatisticsChart objDataStewrdStatistics = new DataStewrdStatisticsChart();
                objDataStewrdStatistics.x = dtDataStewardByuser.Rows[i]["User"].ToString();
                objDataStewrdStatistics.y = Convert.ToInt32(dtDataStewardByuser.Rows[i]["Total Matched Rows"].ToString());
                objDataStewrdStatistics.userGroup = dtDataStewardByuser.Rows[i]["User Group"].ToString();
                lstDataStewrdStatistics.Add(objDataStewrdStatistics);
            }
            return Json(lstDataStewrdStatistics, JsonRequestBehavior.AllowGet);
        }

        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetDataStewardshipStatisticsByCC(string Parameters = null)
        {
            //report for Data Stewardship Statistics through Confidance Code
            string UserGroup = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                UserGroup = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            DataTable dtDataStewardByCC = new DataTable();
            DataSet dsDataStewardStatistics = new DataSet();
            List<DataStewrdStatisticsChart> lstDataStewrdStatistics = new List<DataStewrdStatisticsChart>();
            if (!string.IsNullOrEmpty(SessionHelper.dtDataStewardStatistics) && string.IsNullOrEmpty(UserGroup))
            {
                dsDataStewardStatistics = JsonConvert.DeserializeObject<DataSet>(SessionHelper.dtDataStewardStatistics);
            }
            else
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                dsDataStewardStatistics = fac.GetdtDataStewardStatisticsReport(UserGroup);
            }
            dtDataStewardByCC = dsDataStewardStatistics.Tables[2];
            for (int i = 0; i < dtDataStewardByCC.Rows.Count; i++)
            {
                DataStewrdStatisticsChart objDataStewrdStatistics = new DataStewrdStatisticsChart();
                objDataStewrdStatistics.x = dtDataStewardByCC.Rows[i]["Confidence Code"].ToString();
                objDataStewrdStatistics.userGroup = dtDataStewardByCC.Rows[i]["User Group"].ToString();
                objDataStewrdStatistics.y = Convert.ToInt32(dtDataStewardByCC.Rows[i]["Total Matched Rows"].ToString());
                lstDataStewrdStatistics.Add(objDataStewrdStatistics);
            }
            return Json(lstDataStewrdStatistics, JsonRequestBehavior.AllowGet);
        }
        //UserGroup  DropDown to changes Data Stewardship Statistics Confidence Code and User
        public static List<SelectListItem> GetUserGroups(string connectionString, DataSet dtDataStewardStatistics)
        {
            List<SelectListItem> lstUserGroups = new List<SelectListItem>();
            DataTable dtUserGroups = dtDataStewardStatistics.Tables[0];

            foreach (DataRow row in dtUserGroups.Rows)
            {
                lstUserGroups.Add(new SelectListItem()
                {
                    Text = row["User Group"].ToString(),
                    Value = row["User Group"].ToString()
                });
            }
            return lstUserGroups;
        }
        #endregion

        #region "API Usage Report"
        //report for API Usage Statistics
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetCurrntMonthCnt()
        {
            //Get Current Month Count for API Usage
            DataSet dsDataAPIUsage = new DataSet();
            DataTable dtCurrentMonthCnt = new DataTable();
            List<APIUsagesCurntMonthCntChart> lstAPIUsagesCurntMonthCnt = new List<APIUsagesCurntMonthCntChart>();
            if (!string.IsNullOrEmpty(SessionHelper.dsDataAPIUsage))
            {
                dsDataAPIUsage = JsonConvert.DeserializeObject<DataSet>(SessionHelper.dsDataAPIUsage);
            }
            else
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                dsDataAPIUsage = fac.GetdtAPIReport();
            }
            SessionHelper.dsDataAPIUsage = JsonConvert.SerializeObject(dsDataAPIUsage);
            dtCurrentMonthCnt = dsDataAPIUsage.Tables[1];
            for (int i = 0; i < dtCurrentMonthCnt.Rows.Count; i++)
            {
                APIUsagesCurntMonthCntChart objAPIUsagesCurntMonthCnt = new APIUsagesCurntMonthCntChart();
                objAPIUsagesCurntMonthCnt.x = dtCurrentMonthCnt.Rows[i]["CallType"].ToString();
                objAPIUsagesCurntMonthCnt.y = Convert.ToInt32(dtCurrentMonthCnt.Rows[i]["CurrentMonthCount"].ToString());
                lstAPIUsagesCurntMonthCnt.Add(objAPIUsagesCurntMonthCnt);
            }
            return Json(lstAPIUsagesCurntMonthCnt, JsonRequestBehavior.AllowGet);
        }
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetCurrntYearCnt()
        {
            // Get Current Year Count for API Usage
            DataSet dsDataAPIUsage = new DataSet();
            DataTable dtCurrentYearCnt = new DataTable();
            List<APIUsagesCurntYearCntChart> lstAPIUsagesCurntYearCnt = new List<APIUsagesCurntYearCntChart>();
            if (!string.IsNullOrEmpty(SessionHelper.dsDataAPIUsage))
            {
                dsDataAPIUsage = JsonConvert.DeserializeObject<DataSet>(SessionHelper.dsDataAPIUsage);
            }
            else
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                dsDataAPIUsage = fac.GetdtAPIReport();
            }
            SessionHelper.dsDataAPIUsage = JsonConvert.SerializeObject(dsDataAPIUsage);
            dtCurrentYearCnt = dsDataAPIUsage.Tables[2];
            for (int i = 0; i < dtCurrentYearCnt.Rows.Count; i++)
            {
                APIUsagesCurntYearCntChart objAPIUsagesCurntYearCnt = new APIUsagesCurntYearCntChart();
                objAPIUsagesCurntYearCnt.name = dtCurrentYearCnt.Rows[i]["DnBAPIName"].ToString();
                objAPIUsagesCurntYearCnt.y = Convert.ToInt32(dtCurrentYearCnt.Rows[i]["CurrentYearCount"].ToString());
                lstAPIUsagesCurntYearCnt.Add(objAPIUsagesCurntYearCnt);
            }
            return Json(lstAPIUsagesCurntYearCnt, JsonRequestBehavior.AllowGet);
        }
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetTotalYearsCnt()
        {
            // Get Total Year Count for API Usage
            DataSet dsDataAPIUsage = new DataSet();
            DataTable dtLineTotalCnt = new DataTable();
            List<LineTotalCntChart> lstAPIUsagesLineTotalCnt = new List<LineTotalCntChart>();
            if (!string.IsNullOrEmpty(SessionHelper.dsDataAPIUsage))
            {
                dsDataAPIUsage = JsonConvert.DeserializeObject<DataSet>(SessionHelper.dsDataAPIUsage);
            }
            else
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                dsDataAPIUsage = fac.GetdtAPIReport();
            }
            SessionHelper.dsDataAPIUsage = JsonConvert.SerializeObject(dsDataAPIUsage);
            dtLineTotalCnt = dsDataAPIUsage.Tables[3];
            for (int i = 0; i < dtLineTotalCnt.Rows.Count; i++)
            {
                LineTotalCntChart objLineTotalCnt = new LineTotalCntChart();
                objLineTotalCnt.Month = dtLineTotalCnt.Rows[i]["Months"].ToString();
                objLineTotalCnt.year = Convert.ToInt32(dtLineTotalCnt.Rows[i]["Years"].ToString());
                objLineTotalCnt.TotalCalls = dtLineTotalCnt.Rows[i]["TotalCalls"].ToString();
                lstAPIUsagesLineTotalCnt.Add(objLineTotalCnt);
            }
            return Json(lstAPIUsagesLineTotalCnt, JsonRequestBehavior.AllowGet);
        }
        [RequestFromAjax, RequestFromSameDomain]
        public ActionResult GetAPIUsageGrid()
        {
            // Get API Usage list for API Usage
            DataSet dsDataAPIUsage = new DataSet();
            DataTable dtCurrentMonthCnt = new DataTable();
            List<APIUsagesCurntMonthCntChart> lstAPIUsagesCurntMonthCnt = new List<APIUsagesCurntMonthCntChart>();
            if (!string.IsNullOrEmpty(SessionHelper.dsDataAPIUsage))
            {
                dsDataAPIUsage = JsonConvert.DeserializeObject<DataSet>(SessionHelper.dsDataAPIUsage);
            }
            else
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                dsDataAPIUsage = fac.GetdtAPIReport();
            }
            SessionHelper.dsDataAPIUsage = JsonConvert.SerializeObject(dsDataAPIUsage);
            dtCurrentMonthCnt = dsDataAPIUsage.Tables[0];
            return PartialView("APIusageGrid", dtCurrentMonthCnt);
        }
        #endregion



    }
}