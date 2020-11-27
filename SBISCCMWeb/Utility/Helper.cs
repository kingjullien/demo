using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;
using SBISCCMWeb.Controllers;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public static class Helper
    {
        public static string UpdateMessage = "Record(s) updated successfully.";
        public static string ErrorMessage = CommonMessagesLang.msgSomethingWrong;
        public const string DATETIME_FULL = "MM/dd/yyyy HH:mm:ss";
        public const string DATETIME_SHORT = "MM/dd/yyyy";
        public static bool[] _lookup;
        public static string GetMatchColor(string MG)
        {
            string color;
            switch (MG)
            {
                case "A":
                    color = "ColorA";
                    break;
                case "B":
                    color = "ColorB";
                    break;
                case "F":
                    color = "ColorF";
                    break;
                default:
                    color = "ColorZ";
                    break;
            }
            return color;
        }
        //public static enum  ReportType:int {
        //    InputAndOutput = 1,
        //    CompanyProcessAudit = 2,
        //    StewardshipStatistics = 3,
        //    APIUsage = 4,
        //    TopMatchGrades = 5
        //        };
        public static void StewUserActivityCloseWindow(string ConnectionString)
        {
            // Set window close event and manage changes discard for page.

            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.UserName);
            fac.StewUserActivityCloseWindow(Helper.oUser.UserId);
        }
        public static string GetMasterConnctionstring()
        {
            return ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ToString();
        }
        public static string GetMatchGradeValue(string matchGrade, string dbConnectionString)
        {

            SettingFacade fac = new SettingFacade(dbConnectionString);
            List<MatchGradeEntity> objMatchList = new List<MatchGradeEntity>();
            string result = string.Empty;
            objMatchList = fac.GetMatchGrades();

            if (objMatchList.Any())
            {
                var objMatchGrade = objMatchList.FirstOrDefault(c => c.MatchGradeCode == matchGrade);
                if (objMatchGrade != null)
                {
                    result = objMatchGrade.MatchGradeValue;
                }
            }
            return result;
        }

        public static string GetMDPValue(string mdpcode, string type, string ConnectionString)
        {
            List<MatchCodeEntity> objMatchCodeList = new List<MatchCodeEntity>();
            MatchFacade mf = new MatchFacade(ConnectionString);
            string result = string.Empty;
            objMatchCodeList = mf.GetMDPValues();

            if (objMatchCodeList.Any())
            {
                var objMatchGrade = objMatchCodeList.FirstOrDefault(c => c.MDPCode == mdpcode && c.MDPType == type);
                if (objMatchGrade != null)
                {
                    result = objMatchGrade.MDPValue;
                }
                else
                    result = mdpcode;
            }
            return result;
        }
        public static int UserId
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["UserId"])
                    return (Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["UserId"] = value;
            }
        }
        public static string SrcRecordId
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SrcRecordId"])
                    return (Convert.ToString(HttpContext.Current.Session["SrcRecordId"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SrcRecordId"] = value;
            }
        }
        public static string EmailAddress
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["EmailAddress"])
                    return (Convert.ToString(HttpContext.Current.Session["EmailAddress"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["EmailAddress"] = value;
            }
        }
        public static string TempEmailAddress
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["TempEmailAddress"])
                    return (Convert.ToString(HttpContext.Current.Session["TempEmailAddress"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["TempEmailAddress"] = value;
            }
        }
        public static string UserName
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["UserName"])
                    return (Convert.ToString(HttpContext.Current.Session["UserName"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["UserName"] = value;
            }
        }
        public static string UserType
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["UserType"])
                    return (Convert.ToString(HttpContext.Current.Session["UserType"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["UserType"] = value;
            }
        }
        public static bool IsDirty
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsDirty"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsDirty"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsDirty"] = value;
            }
        }
        public static bool RememberMe
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["RememberMe"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["RememberMe"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["RememberMe"] = value;
            }
        }
        public static bool IsApprovalScreen
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsApprovalScreen"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsApprovalScreen"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsApprovalScreen"] = value;
            }
        }
        public static string CompanyName
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["CompanyName"])
                    return (Convert.ToString(HttpContext.Current.Session["CompanyName"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["CompanyName"] = value;
            }
        }
        public static string Address
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Address"])
                    return (Convert.ToString(HttpContext.Current.Session["Address"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Address"] = value;
            }
        }
        public static string Address1
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Address1"])
                    return (Convert.ToString(HttpContext.Current.Session["Address1"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Address1"] = value;
            }
        }
        public static string City
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["City"])
                    return (Convert.ToString(HttpContext.Current.Session["City"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["City"] = value;
            }
        }
        public static string State
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["State"])
                    return (Convert.ToString(HttpContext.Current.Session["State"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["State"] = value;
            }
        }
        public static string PhoneNbr
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["PhoneNbr"])
                    return (Convert.ToString(HttpContext.Current.Session["PhoneNbr"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["PhoneNbr"] = value;
            }
        }
        public static string Zip
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Zip"])
                    return (Convert.ToString(HttpContext.Current.Session["Zip"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Zip"] = value;
            }
        }
        public static bool ExcludeNonHeadQuarters
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ExcludeNonHeadQuarters"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["ExcludeNonHeadQuarters"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ExcludeNonHeadQuarters"] = value;
            }
        }
        public static bool ExcludeNonMarketable
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ExcludeNonMarketable"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["ExcludeNonMarketable"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ExcludeNonMarketable"] = value;
            }
        }
        public static bool ExcludeOutofBusiness
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ExcludeOutofBusiness"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["ExcludeOutofBusiness"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ExcludeOutofBusiness"] = value;
            }
        }
        public static bool ExcludeUndeliverable
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ExcludeUndeliverable"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["ExcludeUndeliverable"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ExcludeUndeliverable"] = value;
            }
        }
        public static bool ExcludeUnreachable
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ExcludeUnreachable"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["ExcludeUnreachable"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ExcludeUnreachable"] = value;
            }
        }
        public static bool IsMatchDataDirty
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsMatchDataDirty"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsMatchDataDirty"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsMatchDataDirty"] = value;
            }
        }
        public static bool IsCleanDataDirty
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsCleanDataDirty"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsCleanDataDirty"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsCleanDataDirty"] = value;
            }
        }
        public static bool Enable2StepUpdate
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Enable2StepUpdate"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["Enable2StepUpdate"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Enable2StepUpdate"] = value;
            }
        }
        public static bool IsApprover
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsApprover"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsApprover"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsApprover"] = value;
            }
        }
        public static bool Approve
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Approve"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["Approve"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Approve"] = value;
            }
        }
        public static bool IsUserLoginFirstTime
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsUserLoginFirstTime"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsUserLoginFirstTime"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsUserLoginFirstTime"] = value;
            }
        }
        public static string ClientLogo
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ClientLogo"])
                    return (Convert.ToString(HttpContext.Current.Session["ClientLogo"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ClientLogo"] = value;
            }
        }

        public static bool SAMLSSO
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SAMLSSO"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["SAMLSSO"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SAMLSSO"] = value;
            }
        }
        public static string PartnerIdP
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["PartnerIdP"])
                    return (Convert.ToString(HttpContext.Current.Session["PartnerIdP"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["PartnerIdP"] = value;
            }
        }
        public static string FeedbackPath
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["FeedbackPath"])
                    return (Convert.ToString(HttpContext.Current.Session["FeedbackPath"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["FeedbackPath"] = value;
            }
        }
        public static DateTime PasswordResetDate
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["PasswordResetDate"])
                    return (Convert.ToDateTime(HttpContext.Current.Session["PasswordResetDate"]));
                else
                    return DateTime.Now;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["PasswordResetDate"] = value;
            }
        }

        public static bool IsUserAllreadyLogin
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsUserAllreadyLogin"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsUserAllreadyLogin"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsUserAllreadyLogin"] = value;
            }
        }
        public static ObservableCollection<CompanyIdentificationModel> listCompanyIdentity
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["listCompanyIdentity"])
                    return ((ObservableCollection<CompanyIdentificationModel>)HttpContext.Current.Session["listCompanyIdentity"]);
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["listCompanyIdentity"] = value;
            }
        }
        public static bool IsTowStepVerification
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsTowStepVerification"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsTowStepVerification"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsTowStepVerification"] = value;
            }
        }
        public static bool EnableInvestigations
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["EnableInvestigations"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["EnableInvestigations"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["EnableInvestigations"] = value;
            }
        }
        public static bool EnableSearchByDUNS
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["EnableSearchByDUNS"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["EnableSearchByDUNS"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["EnableSearchByDUNS"] = value;
            }
        }
        public static bool EnableCreateAutoAcceptRules
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["EnableCreateAutoAcceptRules"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["EnableCreateAutoAcceptRules"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["EnableCreateAutoAcceptRules"] = value;
            }
        }
        public static string TowWayVerificationData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["TowWayVerificationData"])
                    return (Convert.ToString(HttpContext.Current.Session["TowWayVerificationData"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["TowWayVerificationData"] = value;
            }
        }
        public static string GetCurrentIpAddress()
        {
            var _httpContext = HttpContext.Current.Request;
            if (_httpContext == null)
                return string.Empty;

            var result = "";
            if (_httpContext.Headers != null)
            {
                //look for the X-Forwarded-For (XFF) HTTP header field
                //it's used for identifying the originating IP address of a client connecting to a web server through an HTTP proxy or load balancer. 
                string xff = _httpContext.Headers.AllKeys
                    .Where(x => "X-FORWARDED-FOR".Equals(x, StringComparison.InvariantCultureIgnoreCase))
                    .Select(k => _httpContext.Headers[k])
                    .FirstOrDefault();

                //if you want to exclude private IP addresses, then see http://stackoverflow.com/questions/2577496/how-can-i-get-the-clients-ip-address-in-asp-net-mvc

                if (!String.IsNullOrEmpty(xff))
                {
                    string lastIp = xff.Split(new char[] { ',' }).FirstOrDefault();
                    result = lastIp;
                }
            }

            if (String.IsNullOrEmpty(result) && _httpContext.UserHostAddress != null)
            {
                result = _httpContext.UserHostAddress;
            }

            //some validation
            if (result == "::1")
                result = "127.0.0.1";
            //remove port
            if (!String.IsNullOrEmpty(result))
            {
                int index = result.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                    result = result.Substring(0, index);
            }
            return result;

        }

        public static bool SendMail(string emailTo, string emailSubj, string emailBody, string BCCEmail = null)
        {
            string emailhost = GetAppSettingAsString("emailhost");
            int emailport = Convert.ToInt16(GetAppSettingAsString("emailport"));
            string emailFrom = GetAppSettingAsString("emailFrom");
            string emailuserName = GetAppSettingAsString("emailuserName");
            string emailpassword = GetAppSettingAsString("emailpassword");
            bool emailenableSsl = Convert.ToBoolean(GetAppSettingAsString("emailenableSsl"));
            try
            {
                MailAddress toAddress = new MailAddress(emailTo);
                MailAddress fromAddress = new MailAddress(emailFrom);
                using (SmtpClient smtp = new SmtpClient()
                {
                    Host = emailhost,
                    Port = (int)emailport,
                    EnableSsl = emailenableSsl,
                    Credentials = new System.Net.NetworkCredential(emailuserName, emailpassword)
                })
                {
                    MailMessage message = new MailMessage();
                    message.From = fromAddress;
                    if (!string.IsNullOrEmpty(BCCEmail))
                    {
                        message.Bcc.Add(BCCEmail);
                    }
                    message.To.Add(toAddress);
                    message.Subject = emailSubj;
                    message.Body = emailBody;
                    message.Priority = MailPriority.High;
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsSearchBYDUNS
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsSearchBYDUNS"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsSearchBYDUNS"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsSearchBYDUNS"] = value;
            }
        }
        public static string AuthURL
        {
            get { return "https://direct.dnb.com/Authentication/V2.0/"; }
        }
        public static string JsonMediaType
        {
            get { return "application/json"; }
        }
        public static string XMLMediaType
        {
            get { return "application/xml"; }
        }
        public static string GetAppSettingAsString(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
        public static string SalesForceAccessToken
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SalesForceAccessToken"])
                    return (Convert.ToString(HttpContext.Current.Session["SalesForceAccessToken"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SalesForceAccessToken"] = value;
            }
        }
        public static string SalesForceInstanceUrl
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SalesForceInstanceUrl"])
                    return (Convert.ToString(HttpContext.Current.Session["SalesForceInstanceUrl"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SalesForceInstanceUrl"] = value;
            }
        }
        public static string SalesForceVersion
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SalesForceVersion"])
                    return (Convert.ToString(HttpContext.Current.Session["SalesForceVersion"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SalesForceVersion"] = value;
            }
        }
        public static DataTable SalesForcedtTable
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["SalesForceTableName"])
                    return (DataTable)HttpContext.Current.Session["SalesForceTableName"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["SalesForceTableName"] = value;
            }
        }
        public static DataTable UrlEncode
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["UrlEncode"])
                    return (DataTable)HttpContext.Current.Session["UrlEncode"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["UrlEncode"] = value;
            }
        }

        public static DataTable DatabaseName
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DatabaseName"])
                    return (DataTable)HttpContext.Current.Session["DatabaseName"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DatabaseName"] = value;
            }
        }
        public static DataTable TableName
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["TableName"])
                    return (DataTable)HttpContext.Current.Session["TableName"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["TableName"] = value;
            }
        }
        public static string ServerName
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ServerName"])
                    return (Convert.ToString(HttpContext.Current.Session["ServerName"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ServerName"] = value;
            }
        }
        public static string DbUserName
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DbUserName"])
                    return (Convert.ToString(HttpContext.Current.Session["DbUserName"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DbUserName"] = value;
            }
        }
        public static string DbPassword
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DbPassword"])
                    return (Convert.ToString(HttpContext.Current.Session["DbPassword"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DbPassword"] = value;
            }
        }
        public static string hostName
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["hostName"])
                    return (Convert.ToString(HttpContext.Current.Session["hostName"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null == HttpContext.Current.Session["hostName"])
                    HttpContext.Current.Session["hostName"] = value;
            }
        }
        public static int RequestToken
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["RequestToken"])
                    return (Convert.ToInt32(HttpContext.Current.Session["RequestToken"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["RequestToken"] = value;
            }
        }
        public static int ProfileId
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ProfileId"])
                    return (Convert.ToInt32(HttpContext.Current.Session["ProfileId"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ProfileId"] = value;
            }
        }

        public static int ProductId
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ProductId"])
                    return (Convert.ToInt32(HttpContext.Current.Session["ProductId"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ProductId"] = value;
            }
        }
        public static string ProductCode
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ProductCode"])
                    return (Convert.ToString(HttpContext.Current.Session["ProductCode"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null == HttpContext.Current.Session["ProductCode"])
                    HttpContext.Current.Session["ProductCode"] = value;
            }
        }
        public static string MonitoringLevel
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["MonitoringLevel"])
                    return (Convert.ToString(HttpContext.Current.Session["MonitoringLevel"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null == HttpContext.Current.Session["MonitoringLevel"])
                    HttpContext.Current.Session["MonitoringLevel"] = value;
            }
        }
        public static int ProductElementID
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ProductElementID"])
                    return (Convert.ToInt32(HttpContext.Current.Session["ProductElementID"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null == HttpContext.Current.Session["ProductElementID"])
                    HttpContext.Current.Session["ProductElementID"] = value;
            }
        }
        public static string LicenseSKU
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseSKU"])
                    return (Convert.ToString(HttpContext.Current.Session["LicenseSKU"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseSKU"] = value;
            }
        }
        public static int LicenseNumberOfUsers
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseNumberOfUsers"])
                    return (Convert.ToInt32(HttpContext.Current.Session["LicenseNumberOfUsers"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseNumberOfUsers"] = value;
            }
        }
        public static int LicenseNumberOfTransactions
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseNumberOfTransactions"])
                    return (Convert.ToInt32(HttpContext.Current.Session["LicenseNumberOfTransactions"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseNumberOfTransactions"] = value;
            }
        }
        public static bool LicenseEnableMonitoring
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableMonitoring"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableMonitoring"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableMonitoring"] = value;
            }
        }
        public static bool LicenseEnableDPM
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableDPM"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableDPM"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableDPM"] = value;
            }
        }
        public static bool LicenseEnableFamilyTree
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableFamilyTree"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableFamilyTree"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableFamilyTree"] = value;
            }
        }
        public static bool LicenseEnableTags
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableTags"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableTags"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableTags"] = value;
            }
        }
        public static bool LicenseBuildAList
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseBuildAList"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseBuildAList"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseBuildAList"] = value;
            }
        }
        public static bool LicenseEnableGoogleMap
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableGoogleMap"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableGoogleMap"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableGoogleMap"] = value;
            }
        }
        public static bool LicenseEnableLiveAPI
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableLiveAPI"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableLiveAPI"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableLiveAPI"] = value;
            }
        }
        public static bool LicenseEnableInvestigations
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableInvestigations"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableInvestigations"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableInvestigations"] = value;
            }
        }
        public static bool LicenseEnableBingSearch
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableBingSearch"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableBingSearch"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableBingSearch"] = value;
            }
        }
        public static bool LicenseEnableCommandLine
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableCommandLine"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableCommandLine"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableCommandLine"] = value;
            }
        }
        //Create settings UI for OI MP-500
        public static bool LicenseEnabledDNB
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnabledDNB"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnabledDNB"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnabledDNB"] = value;
            }
        }
        //Create settings UI for OI MP-500
        public static bool LicenseEnabledOrb
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnabledOrb"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnabledOrb"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnabledOrb"] = value;
            }
        }
        // Create LicenseEnableDataStewardship in admin portal for enable/disable Data Stewardship and Prospecting
        public static bool LicenseEnableDataStewardship
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableDataStewardship"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableDataStewardship"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableDataStewardship"] = value;
            }
        }
        // Create Branding in admin portal which includes Matchbook and DandB
        public static string Branding
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["Branding"])
                    return (Convert.ToString(HttpContext.Current.Session["Branding"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["Branding"] = value;
            }
        }

        // Create LicenseEnableStubData in admin portal for enable/disable Cached Data Settings
        public static bool LicenseEnableStubData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableStubData"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableStubData"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableStubData"] = value;
            }
        }
        public static bool LicenseEnableCompliance
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableCompliance"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableCompliance"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableCompliance"] = value;
            }
        }

        public static bool LicenseEnableMultiPassMatching
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableMultiPassMatching"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableMultiPassMatching"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableMultiPassMatching"] = value;
            }
        }
        #region OI ExportData-Company Tree
        public static bool IsEnableCorporateTreeEnrichment
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsEnableCorporateTreeEnrichment "])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsEnableCorporateTreeEnrichment "]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsEnableCorporateTreeEnrichment "] = value;
            }
        }
        #endregion

        public static bool IsExpand
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsExpand"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsExpand"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsExpand"] = value;
            }
        }
        public static bool IsExpandCleanData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsExpandCleanData"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["IsExpandCleanData"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsExpandCleanData"] = value;
            }
        }
        public static string ApiToken
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ApiToken"])
                    return (Convert.ToString(HttpContext.Current.Session["ApiToken"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ApiToken"] = value;
            }
        }
        public static string strCondition
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["strCondition"])
                    return (Convert.ToString(HttpContext.Current.Session["strCondition"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["strCondition"] = value;
            }
        }
        public static string ElementType
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ElementType"])
                    return (Convert.ToString(HttpContext.Current.Session["ElementType"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ElementType"] = value;
            }
        }
        public static List<MasterHelpDataEntity> HelpDataContent
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["HelpDataContent"])
                    return (List<MasterHelpDataEntity>)HttpContext.Current.Session["HelpDataContent"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["HelpDataContent"] = value;
            }
        }
        public static UsersEntity oUser
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["oUser"])
                    return (UsersEntity)HttpContext.Current.Session["oUser"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["oUser"] = value;
            }
        }
        public static string IsEnableDataReset
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsEnableDataReset"])
                    return (Convert.ToString(HttpContext.Current.Session["IsEnableDataReset"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsEnableDataReset"] = value;
            }
        }
        public static string IsEableChat
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["IsEableChat"])
                    return (Convert.ToString(HttpContext.Current.Session["IsEableChat"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["IsEableChat"] = value;
            }
        }
        public static string ResponseErroeMessage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ResponseErroeMessage"])
                    return (Convert.ToString(HttpContext.Current.Session["ResponseErroeMessage"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ResponseErroeMessage"] = value;
            }
        }
        public static string DomainErrorMessage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DomainErrorMessage"])
                    return (Convert.ToString(HttpContext.Current.Session["DomainErrorMessage"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DomainErrorMessage"] = value;
            }
        }
        public static string DunsResponseErroeMessage
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DunsResponseErroeMessage"])
                    return (Convert.ToString(HttpContext.Current.Session["DunsResponseErroeMessage"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DunsResponseErroeMessage"] = value;
            }
        }
        public static string AuthError
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["AuthError"])
                    return (Convert.ToString(HttpContext.Current.Session["AuthError"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["AuthError"] = value;
            }
        }

        public static int ArchivalQueueCount
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ArchivalQueueCount"])
                    return (Convert.ToInt32(HttpContext.Current.Session["ArchivalQueueCount"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ArchivalQueueCount"] = value;
            }
        }

        public static ClientApplicationDataEntity ApplicationData
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["ApplicationData"])
                    return (ClientApplicationDataEntity)HttpContext.Current.Session["ApplicationData"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["ApplicationData"] = value;
            }
        }
        //Match Grade component based on licensing (MP-379)
        public static bool LicenseEnableAdvancedMatch
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableAdvancedMatch"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableAdvancedMatch"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableAdvancedMatch"] = value;
            }
        }
        public static bool EnableApplyMatchFilter
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["EnableApplyMatchFilter"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["EnableApplyMatchFilter"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["EnableApplyMatchFilter"] = value;
            }
        }

        public static PreviewMatchDataModel oPreviewMatchDataModel
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["oPreviewMatchDataModel"])
                    return (PreviewMatchDataModel)HttpContext.Current.Session["oPreviewMatchDataModel"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["oPreviewMatchDataModel"] = value;
            }
        }

        public static DataTable dtlastLoginDetail
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["dtlastLoginDetail"])
                    return (DataTable)HttpContext.Current.Session["dtlastLoginDetail"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["dtlastLoginDetail"] = value;
            }
        }

        //Create New Settings for Salesforce Integration (MP-486)
        public static bool LicenseEnableSalesforce
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseEnableSalesforce"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseEnableSalesforce"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseEnableSalesforce"] = value;
            }
        }
        public static string CurrentProvider
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["CurrentProvider"])
                    return (Convert.ToString(HttpContext.Current.Session["CurrentProvider"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["CurrentProvider"] = value;
            }
        }
        public static string OIAPIKey
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["OIAPIKey"])
                    return (Convert.ToString(HttpContext.Current.Session["OIAPIKey"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["OIAPIKey"] = value;
            }
        }
        public static string GoogleAPIKey
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["GoogleAPIKey"])
                    return (Convert.ToString(HttpContext.Current.Session["GoogleAPIKey"]));
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["GoogleAPIKey"] = value;
            }
        }
        public static int DeletedMatchId
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DeletedMatchId"])
                    return (Convert.ToInt32(HttpContext.Current.Session["DeletedMatchId"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DeletedMatchId"] = value;
            }
        }


        public static List<GlobalThirdPartyAPICredentialsEntity> lstThirdPartyAPIs
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstThirdPartyAPIs"])
                    return (List<GlobalThirdPartyAPICredentialsEntity>)HttpContext.Current.Session["lstThirdPartyAPIs"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstThirdPartyAPIs"] = value;
            }
        }

        public static List<ThirdPartyAPIForEnrichmentEntity> lstEnrichCreds
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["lstEnrichCreds"])
                    return (List<ThirdPartyAPIForEnrichmentEntity>)HttpContext.Current.Session["lstEnrichCreds"];
                else
                    return null;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["lstEnrichCreds"] = value;
            }
        }

        public static int DefaultMornitoring20Credential
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["DefaultMornitoring20Credential"])
                    return (Convert.ToInt32(HttpContext.Current.Session["DefaultMornitoring20Credential"]));
                else
                    return 0;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["DefaultMornitoring20Credential"] = value;
            }
        }
        public static bool LicenseADACompliance
        {
            get
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session && null != HttpContext.Current.Session["LicenseADACompliance"])
                    return (Convert.ToBoolean(HttpContext.Current.Session["LicenseADACompliance"]));
                else
                    return false;
            }
            set
            {
                if (null != HttpContext.Current && null != HttpContext.Current.Session)
                    HttpContext.Current.Session["LicenseADACompliance"] = value;
            }
        }

        public static void InitLookups()
        {
            _lookup = new bool[65536];
            for (char c = '0'; c <= '9'; c++) _lookup[c] = true;
            for (char c = 'A'; c <= 'Z'; c++) _lookup[c] = true;
            for (char c = 'a'; c <= 'z'; c++) _lookup[c] = true;
            _lookup['_'] = true;
            _lookup['-'] = true;
            _lookup['('] = true;
            _lookup[')'] = true;
            _lookup[' '] = true;            
        }

        public static bool IsSpecialCharactersExist(string str)
        {
            bool result = false;
            char[] buffer = new char[str.Length];
            int index = 0;
            foreach (char c in str)
            {
                if (!_lookup[c])
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        #region "Common Date time methods"
        public static string ToDatetimeFull(this DateTime? datetime)
        {
            if (datetime.HasValue)
            {
                return datetime.Value.ToDatetimeFull();
            }
            else
            {
                return string.Empty;
            }
        }
        public static string ToDatetimeFull(this DateTime datetime)
        {
            return datetime.ToString(DATETIME_FULL);
        }
        public static string ToDatetimeShort(this DateTime? datetime)
        {
            if (datetime.HasValue)
            {
                return datetime.Value.ToDatetimeShort();
            }
            else
            {
                return string.Empty;
            }
        }
        public static string ToDatetimeShort(this DateTime datetime)
        {
            return datetime.ToString(DATETIME_SHORT);
        }
        #endregion


    }
}
namespace BarChart
{
    public class Dataset<T>
    {
        public string label { get; set; }
        public string fillColor { get; set; }
        public string strokeColor { get; set; }
        public string highlightFill { get; set; }
        public string highlightStroke { get; set; }
        public List<T> data { get; set; }
    }

    public class Chart<T>
    {
        public List<string> labels { get; set; }
        public List<Dataset<T>> datasets { get; set; }
    }
}
public class ReviewDataChartValue
{
    public string labels;
    public int datasets;
}

public class MatchUserChart
{
    public string name;
    public decimal y;
}
public class MatchConfidenceCodeChart
{
    public string x;
    public decimal y;
}
public class UserDetails
{
    public string UserName;
    public string Image_path;
}
//Investigation
public class TransactionDetail
{
    public object ApplicationTransactionID { get; set; }
    public string ServiceTransactionID { get; set; }
    public string TransactionTimestamp { get; set; }
    public string SubmittingOfficeID { get; set; }
}

public class TransactionResult
{
    public string SeverityText { get; set; }
    public string ResultID { get; set; }
    public string ResultText { get; set; }
}
public class InquiryReferenceDetail
{
    public List<string> CustomerReferenceText { get; set; }
    public string CustomerBillingEndorsementText { get; set; }
}

public class StreetAddressLine
{
    public string LineText { get; set; }
}

public class Address
{
    public StreetAddressLine[] StreetAddressLine { get; set; }
    public string PrimaryTownName { get; set; }
    public string CountryISOAlpha2Code { get; set; }
    public string TerritoryName { get; set; }
    public string FullPostalCode { get; set; }
}
public class OrganizationIdentificationNumberDetail
{
    public string OrganizationIdentificationNumber { get; set; }
    public string OrganizationIdentificationNumberTypeCode { get; set; }
}
public class InquiryDetail
{
    public string DUNSNumber { get; set; }
    public string OrganizationName { get; set; }
    public Address Address { get; set; }
    public List<OrganizationIdentificationNumberDetail> OrganizationIdentificationNumberDetail { get; set; }
    public TelephoneNumber TelePhoneNumber { get; set; }

}
public class TelephoneNumber
{
    public string TelecommunicationNumber { get; set; }
    public string InternationalDialingCode { get; set; }
    public bool MobileIndicator { get; internal set; }
}
public class ContactName
{
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
}
public class InvestigationSubjectContact
{
    public TelephoneNumber TelephoneNumber { get; set; }
    public ContactName ContactName { get; set; }
}
public class InvestigationSpecification
{
    public string CharacterSetPreferenceCode { get; internal set; }
    public string DNBProductID { get; set; }
    public string InvestigationPriorityCode { get; internal set; }
    public string InvestigationRemarksText { get; internal set; }
    public InvestigationSubjectContact InvestigationSubjectContact { get; set; }
    public string LanguagePreferenceCode { get; internal set; }
    public string OrderReasonCode { get; internal set; }
    public string ProductFormatPreferenceCode { get; internal set; }
}
public class EmailAddress
{
    public string TelecommunicationAddress { get; set; }
}
public class RequestorDetail
{
    public string RequestorName { get; set; }
    public EmailAddress EmailAddress { get; set; }
    public Address Address { get; set; }
    public TelephoneNumber TelephoneNumber { get; set; }
}
public class EmailAddress2
{
    public string TelecommunicationAddress { get; set; }
}
public class DeliveryDetail
{
    public string DeliveryMethodCode { get; set; }
    public List<EmailAddress2> EmailAddress { get; set; }
}
public class EmailAddress3
{
    public string TelecommunicationAddress { get; set; }
}
public class NotificationDetail
{
    public string NotificationMethodCode { get; set; }
    public List<EmailAddress3> EmailAddress { get; set; }
}
public class PlaceInvestigationRequestDetail
{
    public InquiryDetail InquiryDetail { get; set; }
    public InvestigationSpecification InvestigationSpecification { get; set; }
    public RequestorDetail RequestorDetail { get; set; }
    public DeliveryDetail DeliveryDetail { get; set; }
    public NotificationDetail NotificationDetail { get; set; }
}
public class PlaceInvestigationRequest
{
    public TransactionDetail TransactionDetail { get; set; }
    public PlaceInvestigationRequestDetail PlaceInvestigationRequestDetail { get; set; }
}
public class RootObject
{
    public PlaceInvestigationRequest PlaceInvestigationRequest { get; set; }
}
public class ArchiveDetail
{
    public int PortfolioAssetID { get; set; }
}
public class PlaceInvestigationResponseDetail
{
    public InquiryDetail InquiryDetail { get; set; }
    public string InvestigationTrackingID { get; set; }
    public ArchiveDetail ArchiveDetail { get; set; }
    public InquiryReferenceDetail InquiryReferenceDetail { get; set; }
}

public class PlaceInvestigationResponse
{
    public string ServiceVersionNumber { get; set; }
    public TransactionDetail TransactionDetail { get; set; }
    public TransactionResult TransactionResult { get; set; }
    public PlaceInvestigationResponseDetail PlaceInvestigationResponseDetail { get; set; }
}

public class PlaceInvestigationResponseRoot
{
    public PlaceInvestigationResponse PlaceInvestigationResponse { get; set; }
}


//Monitoring Profile

public class PlaceMonitoringProfile
{
    public TransactionDetail TransactionDetail { get; set; }
    public TransactionResult TransactionResult { get; set; }
    public InquiryReferenceDetail InquiryReferenceDetail { get; set; }
}

public class PlaceMonitoringProfileRoot
{
    public PlaceMonitoringProfile PlaceMonitoringProfile { get; set; }
}
public class ChartCount
{
    public List<MatchUserChart> lstMatchUser { get; set; }
    public List<MatchConfidenceCodeChart> lstMatchConfidenceCode { get; set; }
}
public class JSONAttributes
{
    public bool result { get; set; }
    public string message { get; set; }
}

