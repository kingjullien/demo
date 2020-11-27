using Microsoft.AspNet.Identity;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;

using System.Management;
using System.Data;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification]
    public class AboutUsController : BaseController
    {
        // GET: AboutUs

        public ActionResult Index()
        {
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            //Get Current User Name
            Helper.UserName = Convert.ToString(User.Identity.GetUserName());
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            //Get User detail by the user Id.
            if (Helper.oUser == null)
            {
                Helper.oUser = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
            }
            UsersModel Users = new UsersModel();
            //Get Login User Detail 
            Users.objUsers = fac.StewUserLogIn(Helper.oUser.EmailAddress, null, true);
            if (Users.objUsers != null)
            {
                ViewBag.ClientGUID = Users.objUsers.ClientGUID;
                ViewBag.ClientName = Users.objUsers.ClientName;
            }
            ViewBag.User = Helper.oUser.UserFullName + "/" + Helper.oUser.EmailAddress;
            // Get Database detail from the connection string.
            using (SqlConnection connection = new SqlConnection(this.CurrentClient.ApplicationDBConnectionString))
            {
                ViewBag.ServerName = connection.DataSource;
                ViewBag.DataBaseName = connection.Database;
            }

            //set IpAddress and Version
            ViewBag.IpAddress = Helper.GetCurrentIpAddress();
            ViewBag.BuildVersion = Convert.ToString(ConfigurationManager.AppSettings["BuildVersion"]);
            ViewBag.FinaldVersion = Convert.ToString(ConfigurationManager.AppSettings["FinalVersion"]);

            string url = Request.Url.Authority;
            sfac = new SettingFacade(General.masterDatabaseConnectionString);
            DataTable dt = sfac.GetLicenseSetting(url);
            if (dt != null)
            {
                //set all License elements
                ViewBag.LicenseSKU = Convert.ToString(dt.Rows[0]["LicenseSKU"].ToString());
                ViewBag.LicenseNumberOfUsers = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseNumberOfUsers"])) ? Convert.ToInt32(dt.Rows[0]["LicenseNumberOfUsers"]) : 0;
                ViewBag.LicenseNumberOfTransactions = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseNumberOfTransactions"])) ? Convert.ToInt32(dt.Rows[0]["LicenseNumberOfTransactions"]) : 0;
                ViewBag.LicenseEnableLiveAPI = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseEnableLiveAPI"])) ? Convert.ToBoolean(dt.Rows[0]["LicenseEnableLiveAPI"]) : false;
                ViewBag.LicenseEnableTags = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseEnableTags"])) ? Convert.ToBoolean(dt.Rows[0]["LicenseEnableTags"]) : false;
                ViewBag.LicenseEndDate = !string.IsNullOrEmpty(dt.Rows[0]["LicenseEndDate"].ToString()) ? Convert.ToDateTime(dt.Rows[0]["LicenseEndDate"]).ToString("MM-dd-yyyy") : string.Empty;
                ViewBag.MonitorProfile = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseEnableMonitoring"])) ? Convert.ToBoolean(dt.Rows[0]["LicenseEnableMonitoring"]) : false;
                ViewBag.Investigation = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseEnableInvestigations"])) ? Convert.ToBoolean(dt.Rows[0]["LicenseEnableInvestigations"]) : false;
            }
            else
            {
                //set all License elements empty or default when data is null in database
                ViewBag.LicenseSKU = "";
                ViewBag.LicenseNumberOfUsers = "0";
                ViewBag.LicenseNumberOfTransactions = "0";
                ViewBag.LicenseEnableLiveAPI = false;
                ViewBag.LicenseEnableTags = false;
                ViewBag.LicenseEndDate = "";
                ViewBag.MonitorProfile = false;
                ViewBag.Investigation = false;
            }
            return View();
        }
    }
}