using Amazon.S3;
using Amazon.S3.Model;
using ExcelDataReader;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Exchange.WebServices.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using Renci.SshNet;
using SBISCCMWeb.Models;
using SBISCCMWeb.Models.BeneficialOwnership;
using SBISCCMWeb.Models.PreviewMatchData.Main;
using SBISCCMWeb.Utility.IdentityResolution;
using SBISCCMWeb.Utility.SearchByDomain;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Utility
{
    public class CommonMethod
    {
        public List<SettingEntity> LoadCleanseMatchSettings(string ConnectionString)
        {
            try
            {
                SettingFacade sfac = new SettingFacade(ConnectionString);
                return sfac.GetCleanseMatchSettings();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CountryEntity> LoadCountries(string ConnectionString)
        {
            SettingFacade sfac = new SettingFacade(ConnectionString);
            return sfac.GetCountries();
        }
        public void InsertAPILogs(string ConnectionString, string ServiceTransactionID, DateTime? TransactionTimestamp, string SeverityText, string ResultID, string ResultText = null, string MatchDataCriteriaText = null, int MatchedQuantity = 0, string DnBDUNSNumber = null)
        {
            CompanyFacade cFac = new CompanyFacade(ConnectionString, Helper.UserName);
            cFac.InsertAPILogs(ServiceTransactionID, TransactionTimestamp, SeverityText, ResultID, ResultText, MatchDataCriteriaText, MatchedQuantity, DnBDUNSNumber);
        }
        public void InsertAPILogs(TransactionResponseDetail objResponse, string ConnectionString)
        {
            CompanyFacade cFac = new CompanyFacade(ConnectionString, Helper.UserName);
            if (objResponse != null)
                cFac.InsertAPILogs(objResponse.ServiceTransactionID, objResponse.TransactionTimestamp, objResponse.SeverityText, objResponse.ResultID, objResponse.ResultText, objResponse.MatchDataCriteriaText, objResponse.MatchedQuantity, objResponse.DnBDUNSNumber);
        }
        public static Dictionary<string, string> GetOrderByColumns()
        {
            Dictionary<string, string> lst = new Dictionary<string, string>();
            lst.Add("SrcRecordId", "SrcRecordId");
            lst.Add("CompanyName", "CompanyName");
            lst.Add("Address", "Address");
            lst.Add("City", "City");
            lst.Add("State", "State");
            lst.Add("PostalCode", "PostalCode");
            lst.Add("Country", "Country");
            lst.Add("PhoneNbr", "PhoneNbr");
            return lst;
        }
        public string GetSettingIDs(List<SettingEntity> settings, string settingName)
        {
            return settings.FirstOrDefault(c => c.SettingName == settingName)?.SettingValue;
        }

        public static bool CheckFileType(string Type, string FileName)
        {

            return !string.IsNullOrWhiteSpace(FileName) && Array.IndexOf(Type.Split(','), System.IO.Path.GetExtension(FileName)) > -1 ? true : false;
        }
        public static int GenerateToken()
        {
            Random rnd = new Random();
            return Helper.RequestToken = rnd.Next(100, 9999);
        }


        public static void SetHelperSession(int UserId, string ConnectionString)
        {
            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.UserName);
            SettingFacade sfac = new SettingFacade(ConnectionString);

            UsersEntity oNewUser = sfac.GetUserDetailsById(UserId);
            var user = fac.GetUserByLoginId(oNewUser.EmailAddress);
            Helper.Enable2StepUpdate = user.Enable2StepUpdate;
            Helper.IsApprover = user.IsApprover;
            Helper.UserName = Convert.ToString(user.UserFullName);
            Helper.UserType = Convert.ToString(user.UserType);
            //Helper.oUser.UserId = user.UserId;
            Helper.IsUserLoginFirstTime = user.IsUserLoginFirstTime;
        }


        public static void GetThirdPartyAPICredentials(string ConnectionString)
        {
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(ConnectionString);
            fac.RefreshThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            //Set third party apis key in session
            Helper.lstThirdPartyAPIs = fac.GetThirdPartyAPICredentialsForUser(Helper.oUser.UserId);
        }

        public static void GetUXDefaultUXEnrichments(string ConnectionString)
        {
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(ConnectionString);
            //Set enrichment apis key in session
            Helper.lstEnrichCreds = fac.GetUXDefaultUXEnrichments();
        }

        public static void GetAPIToken(string ConnectionString)
        {
            CommonMethod objCommon = new CommonMethod();
            var objResult = objCommon.LoadCleanseMatchSettings(ConnectionString);//get Cleanse Match Settings
            string IsEnableDataReset = objCommon.GetSettingIDs(objResult, "ENABLE_DATA_RESET");
            string IsEnableCorporateTreeEnrichment = objCommon.GetSettingIDs(objResult, "ORB_ENABLE_CORPORATE_TREE_ENRICHMENT");


            //set Helpers
            Helper.IsEnableDataReset = !string.IsNullOrEmpty(IsEnableDataReset) ? IsEnableDataReset : "false";
            Helper.IsEnableCorporateTreeEnrichment = !string.IsNullOrEmpty(IsEnableCorporateTreeEnrichment) ? Convert.ToBoolean(IsEnableCorporateTreeEnrichment) : false;

            //set Default google api Key
            GoogleAPIFacade fac = new GoogleAPIFacade(ConnectionString);
            string GoogleAPIKey = fac.GetDefaultAPIKey();
            Helper.GoogleAPIKey = !string.IsNullOrEmpty(GoogleAPIKey) ? GoogleAPIKey : "";

            //set Default Orb API Key
            List<ThirdPartyAPICredentialsEntity> lstThirdPartyAPICredentials = new List<ThirdPartyAPICredentialsEntity>();
            ThirdPartyAPICredentialsFacade ThirdPartyAPICredentialsfac = new ThirdPartyAPICredentialsFacade(ConnectionString);
            lstThirdPartyAPICredentials = ThirdPartyAPICredentialsfac.GetThirdPartyAPICredentials(ThirdPartyProvider.ORB.ToString());
            if (lstThirdPartyAPICredentials != null && lstThirdPartyAPICredentials.Any())
            {
                try { Helper.OIAPIKey = lstThirdPartyAPICredentials.FirstOrDefault().APICredential; }
                catch (Exception ex) { Helper.OIAPIKey = ""; }
            }
            else { Helper.OIAPIKey = ""; }
        }
        public static void LicenseSetting(string Url, string ConnectionString)
        {
            SettingFacade sfac = new SettingFacade(ConnectionString);
            DataTable dt = sfac.GetLicenseSetting(Url);
            if (dt != null && dt.Rows.Count > 0)
            {
                Helper.LicenseSKU = Convert.ToString(dt.Rows[0]["LicenseSKU"].ToString());
                Helper.LicenseNumberOfUsers = Convert.ToInt32(!string.IsNullOrEmpty(dt.Rows[0]["LicenseNumberOfUsers"].ToString()) ? dt.Rows[0]["LicenseNumberOfUsers"].ToString() : "0");
                Helper.LicenseNumberOfTransactions = Convert.ToInt32(!string.IsNullOrEmpty(dt.Rows[0]["LicenseNumberOfTransactions"].ToString()) ? dt.Rows[0]["LicenseNumberOfTransactions"].ToString() : "0");
                Helper.LicenseEnableMonitoring = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableMonitoring"].ToString()) ? dt.Rows[0]["LicenseEnableMonitoring"].ToString() : "false");
                Helper.LicenseEnableTags = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableTags"].ToString()) ? dt.Rows[0]["LicenseEnableTags"].ToString() : "false");
                Helper.LicenseEnableLiveAPI = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableLiveAPI"].ToString()) ? dt.Rows[0]["LicenseEnableLiveAPI"].ToString() : "false");
                Helper.LicenseEnableInvestigations = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableInvestigations"].ToString()) ? dt.Rows[0]["LicenseEnableInvestigations"].ToString() : "false");
                Helper.LicenseEnableBingSearch = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableBingSearch"].ToString()) ? dt.Rows[0]["LicenseEnableBingSearch"].ToString() : "false");
                Helper.LicenseBuildAList = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseBuildAList"].ToString()) ? dt.Rows[0]["LicenseBuildAList"].ToString() : "false");
                Helper.LicenseEnableGoogleMap = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableGoogleMap"].ToString()) ? dt.Rows[0]["LicenseEnableGoogleMap"].ToString() : "false");
                Helper.LicenseEnableCommandLine = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableCommandLine"].ToString()) ? dt.Rows[0]["LicenseEnableCommandLine"].ToString() : "false");

                //Match Grade component based on licensing(MP-379)
                Helper.LicenseEnableAdvancedMatch = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableAdvancedMatch"].ToString()) ? dt.Rows[0]["LicenseEnableAdvancedMatch"].ToString() : "false");

                //Create settings UI for OI MP-500
                Helper.LicenseEnabledDNB = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnabledDNB"].ToString()) ? dt.Rows[0]["LicenseEnabledDNB"].ToString() : "false");
                Helper.LicenseEnabledOrb = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnabledOrb"].ToString()) ? dt.Rows[0]["LicenseEnabledOrb"].ToString() : "false");

                Helper.LicenseEnableDPM = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableDPM"].ToString()) ? dt.Rows[0]["LicenseEnableDPM"].ToString() : "false");
                Helper.LicenseEnableFamilyTree = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableFamilyTree"].ToString()) ? dt.Rows[0]["LicenseEnableFamilyTree"].ToString() : "false");
                Helper.LicenseEnableDataStewardship = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableDataStewardship"].ToString()) ? dt.Rows[0]["LicenseEnableDataStewardship"].ToString() : "false");
                Helper.Branding = dt.Rows[0]["Branding"] == DBNull.Value ? Branding.Matchbook.ToString() : !string.IsNullOrEmpty(dt.Rows[0]["Branding"].ToString()) ? dt.Rows[0]["Branding"].ToString() : Branding.Matchbook.ToString();
                //Brandings.SetBranding(Helper.Branding);
                Helper.LicenseEnableStubData = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableStubData"].ToString()) ? dt.Rows[0]["LicenseEnableStubData"].ToString() : "false");
                Helper.LicenseEnableCompliance = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableCompliance"].ToString()) ? dt.Rows[0]["LicenseEnableCompliance"].ToString() : "false");
                Helper.LicenseEnableMultiPassMatching = Convert.ToBoolean(!string.IsNullOrEmpty(dt.Rows[0]["LicenseEnableMultiPassMatching"].ToString()) ? dt.Rows[0]["LicenseEnableMultiPassMatching"].ToString() : "false");
                if (Helper.LicenseEnabledDNB)
                {
                    Helper.CurrentProvider = Convert.ToString(ProviderType.DandB);
                }
                else if (Helper.LicenseEnabledOrb)
                {
                    Helper.CurrentProvider = Convert.ToString(ProviderType.OI);
                }
            }



        }
        #region Get Help Content
        public static string GetHelpContent(int sectionMasterId, string ConnectionString)
        {
            // Get Help Content from the admin panel Database and set.
            string helpContent = string.Empty;
            if (Helper.HelpDataContent == null)
            {
                MasterHelpDataFacade MHFacade = new MasterHelpDataFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase), Helper.UserName);
                Helper.HelpDataContent = MHFacade.GetActiveHelp();
            }
            helpContent = Helper.HelpDataContent.Where(x => x.HelpDataId == sectionMasterId).Select(x => x.Helpdata).FirstOrDefault();
            return helpContent;
        }
        #endregion
        #region Mail Exchange Api
        public static void EmailExchange()
        {
            try
            {
                GetUsersEwsUrl("support@matchbookservices.com", "P@ss4_You01");
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
        }

        static void GetUsersEwsUrl(string userEmailAddress, string userPassword)
        {
            ExchangeService service = new ExchangeService();
            // Set specific credentials.
            service.Credentials = new NetworkCredential(userEmailAddress, userPassword);
            // Look up the user's EWS endpoint by using Auto discover.
            service.AutodiscoverUrl(userEmailAddress, RedirectionCallback);
            EmailMessage message = new EmailMessage(service);
            message.Subject = "Test Subject";
            message.Body = "This is test body.";
            message.ToRecipients.Add("pl9@estatic-infotech.in");
            message.Save();
            message.SendAndSaveCopy();
        }

        static bool RedirectionCallback(string url)
        {
            // Return true if the URL is an HTTPS URL.
            return url.ToLower().StartsWith("https://");
        }
        #endregion

        public static string EncodeSrting(string value, string ConnectionString)
        {
            DataTable dt = new DataTable();
            //SettingFacade sfac = new SettingFacade(ConnectionString);
            //if (Helper.UrlEncode == null)
            //{
            //    dt = sfac.GetURLEncode();
            //    Helper.UrlEncode = dt;
            //}
            //else {
            //    dt = Helper.UrlEncode;
            //}

            foreach (DataRow d in dt.Rows)
            {
                value = value.Replace(d["Character"].ToString(), d["URLEncode"].ToString());
            }
            //MatchFacade mfObject = new MatchFacade(ConnectionString);
            //mfObject.EncodeURL(value);
            return value;
        }



        public static SelectList GetUserCommentType()
        {
            List<SelectListItem> lstUserCommentType = new List<SelectListItem>();
            lstUserCommentType.Add(new SelectListItem { Value = "AUTOACCEPT_DELETE", Text = "AUTOACCEPT_DELETE" });
            return new SelectList(lstUserCommentType, "Value", "Text");
        }

        //fill the Dropdown


        #region Reject All
        public SelectList GetStewTags(string ConnectionString, bool IsMatchData)
        {
            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.oUser.UserName);
            DataTable dt = fac.GetStewTags(IsMatchData, Helper.oUser != null ? Helper.oUser.LOBTag : "");
            List<SelectListItem> lstTags = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTags.Add(new SelectListItem { Value = dt.Rows[i]["Tag"].ToString(), Text = dt.Rows[i]["TagName"].ToString() });
            }
            return new SelectList(lstTags, "Value", "Text");
        }
        public SelectList GetStewImportProcess(string ConnectionString, string Queue, bool IsMatchData)
        {
            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.oUser.UserName);
            DataTable dt = fac.GetImportProcessesByQueue(Queue, IsMatchData);
            List<SelectListItem> lstImportProcess = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstImportProcess.Add(new SelectListItem { Value = dt.Rows[i]["ImportProcess"].ToString(), Text = dt.Rows[i]["ImportProcess"].ToString() });
            }
            return new SelectList(lstImportProcess, "Value", "Text");
        }

        #endregion
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length - 1; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        //public static SelectList GetCDSEnvironment(string ConnectionString)
        //{
        //    SettingFacade fac = new SettingFacade(ConnectionString);
        //    DataTable dt = fac.GetAllCDSEnvironment();
        //    List<SelectListItem> lstAllEnvironment = new List<SelectListItem>();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        lstAllEnvironment.Add(new SelectListItem { Value = dt.Rows[i]["EnvironmentId"].ToString(), Text = dt.Rows[i]["EnvironmentName"].ToString() });
        //    }
        //    //lstAllEnvironment.Add(new SelectListItem { Value = "6812a77c-c28e-49aa-9b48-9a3006c5f7e0", Text = "Matchbook Dev Enviorment" });
        //    return new SelectList(lstAllEnvironment, "Value", "Text");
        //}
        public static SelectList GetCDSEntity(string ConnectionString)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            DataTable dt = fac.GetAllCDSEntity();

            List<SelectListItem> lstAllEntity = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAllEntity.Add(new SelectListItem { Value = dt.Rows[i]["Entity"].ToString(), Text = dt.Rows[i]["Entity"].ToString() });
            }
            return new SelectList(lstAllEntity, "Value", "Text");
            //lstAllEntity.Add(new SelectListItem { Value = "Organization", Text = "Organization" });

        }

        public struct DateTimeSpan
        {
            private readonly int years;
            private readonly int months;
            private readonly int days;
            private readonly int hours;
            private readonly int minutes;
            private readonly int seconds;
            private readonly int milliseconds;

            public DateTimeSpan(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
            {
                this.years = years;
                this.months = months;
                this.days = days;
                this.hours = hours;
                this.minutes = minutes;
                this.seconds = seconds;
                this.milliseconds = milliseconds;
            }

            public int Years { get { return years; } }
            public int Months { get { return months; } }
            public int Days { get { return days; } }
            public int Hours { get { return hours; } }
            public int Minutes { get { return minutes; } }
            public int Seconds { get { return seconds; } }
            public int Milliseconds { get { return milliseconds; } }

            enum Phase { Years, Months, Days, Done }

            public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
            {
                if (date2 < date1)
                {
                    var sub = date1;
                    date1 = date2;
                    date2 = sub;
                }

                DateTime current = date1;
                int years = 0;
                int months = 0;
                int days = 0;

                Phase phase = Phase.Years;
                DateTimeSpan span = new DateTimeSpan();
                int officialDay = current.Day;

                while (phase != Phase.Done)
                {
                    switch (phase)
                    {
                        case Phase.Years:
                            if (current.AddYears(years + 1) > date2)
                            {
                                phase = Phase.Months;
                                current = current.AddYears(years);
                            }
                            else
                            {
                                years++;
                            }
                            break;
                        case Phase.Months:
                            if (current.AddMonths(months + 1) > date2)
                            {
                                phase = Phase.Days;
                                current = current.AddMonths(months);
                                if (current.Day < officialDay && officialDay <= DateTime.DaysInMonth(current.Year, current.Month))
                                    current = current.AddDays(officialDay - current.Day);
                            }
                            else
                            {
                                months++;
                            }
                            break;
                        case Phase.Days:
                            if (current.AddDays(days + 1) > date2)
                            {
                                current = current.AddDays(days);
                                var timespan = date2 - current;
                                span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                                phase = Phase.Done;
                            }
                            else
                            {
                                days++;
                            }
                            break;
                    }
                }

                return span;
            }
        }

        #region "Display Sort Count like  1000 to 1k"
        public static string FormatNumber(string Num)
        {

            // Round off the Statistics count if count is too large 
            string val = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Num))
                {
                    decimal number = Convert.ToDecimal(Num);
                    if (number > 1000000000)
                        val = (number / 1000000000).ToString("#0.0") + "B";
                    else if (number > 1000000)
                        val = (number / 1000000).ToString("#0.0") + "M";
                    else if (number > 1000)
                        val = (number / 1000).ToString("#0.0") + "K";
                    else
                        val = number.ToString();
                }
                else { val = "0"; }
            }
            catch (Exception)
            {
                val = "0";
            }
            return val;
        }
        #endregion


        public static bool IsDigitsOnly(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            else
            {
                foreach (char c in str)
                {
                    if (c < '0' || c > '9')
                        return false;
                }

                return true;
            }
        }


        public static void GetSettingIDs(CleanseMatchSettingsModel CleanseMatchSettingsModel)
        {
            //Get Cleanse Match Settings
            for (int i = 0; i < CleanseMatchSettingsModel.Settings.Count; i++)
            {
                string settingname = CleanseMatchSettingsModel.Settings[i].SettingName;
                switch (settingname)
                {
                    case "AUTO_CORRECTION_THRESHOLD":
                        CleanseMatchSettingsModel.AUTO_CORRECTION_THRESHOLD = i; break;
                    case "MAX_PARALLEL_THREAD":
                        CleanseMatchSettingsModel.MAX_PARALLEL_THREAD = i; break;
                    case "MATCH_GRADE_NAME_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_NAME_THRESHOLD = i; break;
                    case "MATCH_GRADE_STREET_NO_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_STREET_NO_THRESHOLD = i; break;
                    case "MATCH_GRADE_STREET_NAME_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_STREET_NAME_THRESHOLD = i; break;
                    case "MATCH_GRADE_CITY_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_CITY_THRESHOLD = i; break;
                    case "MATCH_GRADE_STATE_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_STATE_THRESHOLD = i; break;
                    case "MATCH_GRADE_TELEPHONE_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_TELEPHONE_THRESHOLD = i; break;
                    case "MATCH_GRADE_POBOX_THRESHOLD":  //changing the SettingName in the ProcessSettings table to MATCH_GRADE_POBOX_THRESHOLD from MATCH_GRADE_ZIPCODE_THRESHOLD(MP-338)
                        CleanseMatchSettingsModel.MATCH_GRADE_POBOX_THRESHOLD = i; break;
                    case "APPLY_MATCH_GRADE_TO_LCM":
                        CleanseMatchSettingsModel.APPLY_MATCH_GRADE_TO_LCM = i; break;
                    case "BATCH_SIZE":
                        CleanseMatchSettingsModel.BATCH_SIZE = i; break;
                    case "WAIT_TIME_BETWEEN_BATCHES_SECS":
                        CleanseMatchSettingsModel.WAIT_TIME_BETWEEN_BATCHES_SECS = i; break;
                    case "ORB_API_KEY":
                        CleanseMatchSettingsModel.ORB_API_KEY = i; break;
                    case "ORB_BATCH_SIZE":
                        CleanseMatchSettingsModel.ORB_BATCH_SIZE = i; break;
                    case "ORB_BATCH_WAITTIME_SECS":
                        CleanseMatchSettingsModel.ORB_BATCH_WAITTIME_SECS = i; break;
                    case "ORB_MAX_PARALLEL_THREADS":
                        CleanseMatchSettingsModel.ORB_MAX_PARALLEL_THREADS = i; break;
                    case "PAUSE_ORB_BATCHMATCH_ETL":
                        CleanseMatchSettingsModel.PAUSE_ORB_BATCHMATCH_ETL = i; break;
                    case "ORB_DATA_IMPORT_DUPLICATE_RESOLUTION":
                        CleanseMatchSettingsModel.ORB_DATA_IMPORT_DUPLICATE_RESOLUTION = i; break;
                    case "ORB_DATA_IMPORT_DUPLICATE_RESOLUTION_TAGS":
                        CleanseMatchSettingsModel.ORB_DATA_IMPORT_DUPLICATE_RESOLUTION_TAGS = i; break;
                    case "ORB_ENABLE_CORPORATE_TREE_ENRICHMENT":
                        CleanseMatchSettingsModel.ORB_ENABLE_CORPORATE_TREE_ENRICHMENT = i; break;
                    case "DATA_STUB_CLIENT_CODE":
                        CleanseMatchSettingsModel.DATA_STUB_CLIENT_CODE = i; break;
                    case "USE_DATA_STUB":
                        CleanseMatchSettingsModel.USE_DATA_STUB = i; break;
                    case "USE_DATA_STUB_FOR_ENRICHMENT":
                        CleanseMatchSettingsModel.USE_DATA_STUB_FOR_ENRICHMENT = i; break;
                    case "PAUSE_FILE_IMPORT_PROCESS_ETL":
                        CleanseMatchSettingsModel.PAUSE_FILE_IMPORT_PROCESS_ETL = i; break;
                    default:
                        break;
                }
            }
        }

        public string ValidateDnBLogin(string DnBUserName, string DnBPassword)
        {
            string AuthenticationUrl = "https://direct.dnb.com/rest/Authentication";
            string result = "";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (WebClient client = new WebClient())
            {
                try
                {
                    WebHeaderCollection myWebHeaderCollection = client.ResponseHeaders;
                    client.Headers.Add("x-dnb-user", DnBUserName);
                    client.Headers.Add("x-dnb-pwd", DnBPassword);
                    byte[] arr = client.DownloadData(AuthenticationUrl);
                    result = "Credential are valid.";
                }
                catch
                {
                    result = "Invalid Credentials";
                    return result;
                }

                return result;
            }
        }

        public static string GetMatchGrade(string MG)
        {
            string GradeName = string.Empty;
            switch (MG)
            {
                case "A":
                    GradeName = "Same";
                    break;
                case "B":
                    GradeName = "Similar";
                    break;
                case "F":
                    GradeName = "Different";
                    break;
                case "X":
                    GradeName = "No Data";
                    break;
                case "Z":
                    GradeName = "Missing Input";
                    break;
            }
            return GradeName;
        }


        //Convert Datatable to json
        public static string DataSetToJSON(DataTable dt)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            object[] arr = new object[dt.Rows.Count];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                arr[i] = dt.Rows[i].ItemArray;
            }
            dict.Add(dt.TableName, arr);
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(dict);
        }

        #region "Bing Search"
        public async static Task<IEnumerable<dynamic>> MakeRequest(string searchValue)
        {
            List<dynamic> sres = new List<dynamic>();
            string BingSearchKey = ConfigurationManager.AppSettings["BingSearchKey"];
            string BingSearchUrl = ConfigurationManager.AppSettings["BingSearchUrl"];
            var client = new HttpClient();
            // Request headers  
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", BingSearchKey);
            // Request parameters  
            int count = 20; //set count for get records 
            string offset = "0";
            string mkt = "en-us";
            try
            {
                //call bing search request 
                var result = await client.GetAsync(string.Format("{0}q={1}&count={2}&offset={3}&mkt={4}", BingSearchUrl, WebUtility.UrlEncode(searchValue), count, offset, mkt));
                result.EnsureSuccessStatusCode();
                var json = await result.Content.ReadAsStringAsync();
                dynamic data = JObject.Parse(json);

                for (int i = 0; i < count; i++)
                {
                    sres.Add(new BingSearchModel
                    {
                        Id = data.webPages.value[i].id,
                        Name = data.webPages.value[i].name,
                        Url = data.webPages.value[i].url,
                        Snippet = data.webPages.value[i].snippet,
                        DisplayUrl = data.webPages.value[i].displayUrl
                    });
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (message.IndexOf("Index was out of range") == 0)
                {
                    return sres;
                }
                throw;
            }
            return sres;
        }
        #endregion

        #region  "Convert JSON To DataTable"
        public static DataTable ConvertJSONToDataTable(string json)
        {
            var jsonLinq = JObject.Parse(json);
            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }
            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }
        #endregion

        #region Remove Special Characters
        public static string RemoveSpecialChars(string removeSpecalCharacter)
        {
            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (removeSpecalCharacter.Contains(chars[i]))
                {
                    removeSpecalCharacter = removeSpecalCharacter.Replace(chars[i], "");
                }
            }
            return removeSpecalCharacter;
        }
        #endregion

        #region Password Validation for Reset Password
        public static bool ValidatePassword(string password)
        {
            int cnt = 0;
            if (password.Length >= 8 && password.Length <= 51)
            {
                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                if (hasNumber.IsMatch(password))
                {
                    cnt++;
                }
                if (hasUpperChar.IsMatch(password))
                {
                    cnt++;
                }
                if (hasLowerChar.IsMatch(password))
                {
                    cnt++;
                }
                if (hasSymbols.IsMatch(password))
                {
                    cnt++;
                }

                if (cnt >= 3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region MP-796 While creating Tag Name , allow only alpha numeric and hyphen symbol .
        public static bool isValidTagName(string inputTag)
        {
            var strRegex = new Regex(@"^([a-zA-Z0-9-_]+)+$");
            if (strRegex.IsMatch(inputTag))
                return true;
            else
                return false;
        }
        #endregion

        public static bool IsLanguageAllow()
        {
            bool isAllow = false;
            try
            {
                string APIType = Helper.lstThirdPartyAPIs.Where(x => x.Code == "DNB_SINGLE_ENTITY_SEARCH").Select(x => x.APIType).FirstOrDefault();
                if (APIType.ToLower() == ApiLayerType.Directplus.ToString().ToLower())
                {
                    isAllow = true;
                }
            }
            catch (Exception ex)
            {
                isAllow = false;
            }
            return isAllow;
        }
        public static string GetThirdPartyProperty(string code, string GetValue)
        {
            try
            {
                return Helper.lstThirdPartyAPIs.Where(x => x.Code == code).Select(x => { return x.GetType().GetProperty(GetValue).GetValue(x, null); }).FirstOrDefault().ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #region Preview Enrichment Data
        public static DunsInfo EnrichmentDataPreview(DunsInfo model, DataSet ds)
        {
            if (ds != null)
            {

                DataTable dumsInfoBaseTb = ds.Tables[0];
                model.Base = new SFI_CMPELK_Baseclass();
                if (dumsInfoBaseTb != null && dumsInfoBaseTb.Rows != null && dumsInfoBaseTb.Rows.Count > 0)
                {
                    model.Base.DnBDUNSNumber = dumsInfoBaseTb.Rows[0]["DnBDUNSNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["DnBDUNSNumber"]);
                    model.Base.startDate = dumsInfoBaseTb.Rows[0]["startDate"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["startDate"]);
                    model.Base.OutOfBusinessIndicator = Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["OutOfBusinessIndicator"]);
                    model.Base.operatingStatus = dumsInfoBaseTb.Rows[0]["operatingStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["operatingStatus"]);
                    model.Base.operatingStatusCode = dumsInfoBaseTb.Rows[0]["operatingStatusCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["operatingStatusCode"]);
                    model.Base.isMarketable = dumsInfoBaseTb.Rows[0]["isMarketable"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["isMarketable"]);
                    model.Base.isMailUndeliverable = dumsInfoBaseTb.Rows[0]["isMailUndeliverable"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["isMailUndeliverable"]);
                    model.Base.isTelephoneDisconnected = dumsInfoBaseTb.Rows[0]["isTelephoneDisconnected"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["isTelephoneDisconnected"]);
                    model.Base.isDelisted = dumsInfoBaseTb.Rows[0]["isDelisted"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["isDelisted"]);
                    model.Base.dunsControlStatusDescription = dumsInfoBaseTb.Rows[0]["dunsControlStatusDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["dunsControlStatusDescription"]);
                    model.Base.dunsControlStatusCode = dumsInfoBaseTb.Rows[0]["dunsControlStatusCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["dunsControlStatusCode"]);
                    model.Base.dunsControlStatusLastUpdateDate = dumsInfoBaseTb.Rows[0]["dunsControlStatusLastUpdateDate"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["dunsControlStatusLastUpdateDate"]);
                    model.Base.dunsControlStatusFullReportDate = dumsInfoBaseTb.Rows[0]["dunsControlStatusFullReportDate"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["dunsControlStatusFullReportDate"]);
                    model.Base.primaryName = dumsInfoBaseTb.Rows[0]["primaryName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryName"]);
                    model.Base.tradeStyleName0 = dumsInfoBaseTb.Rows[0]["tradeStyleName0"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["tradeStyleName0"]);
                    model.Base.tradeStylepriority0 = dumsInfoBaseTb.Rows[0]["tradeStylepriority0"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["tradeStylepriority0"]);
                    model.Base.tradeStyleName1 = dumsInfoBaseTb.Rows[0]["tradeStyleName1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["tradeStyleName1"]);
                    model.Base.tradeStylepriority1 = dumsInfoBaseTb.Rows[0]["tradeStylepriority1_CMPELK"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["tradeStylepriority1_CMPELK"]);
                    model.Base.tradeStyleName2 = dumsInfoBaseTb.Rows[0]["tradeStyleName2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["tradeStyleName2"]);
                    model.Base.tradeStylepriority2 = dumsInfoBaseTb.Rows[0]["tradeStylepriority2"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["tradeStylepriority2"]);
                    model.Base.tradeStyleName3 = dumsInfoBaseTb.Rows[0]["tradeStyleName3"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["tradeStyleName3"]);
                    model.Base.tradeStylepriority3 = dumsInfoBaseTb.Rows[0]["tradeStylepriority3"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["tradeStylepriority3"]);
                    model.Base.tradeStyleName4 = dumsInfoBaseTb.Rows[0]["tradeStyleName4"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["tradeStyleName4"]);
                    model.Base.tradeStylepriority4 = dumsInfoBaseTb.Rows[0]["tradeStylepriority4"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["tradeStylepriority4"]);
                    model.Base.websiteAddressUrl0 = dumsInfoBaseTb.Rows[0]["websiteAddressUrl0"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["websiteAddressUrl0"]);
                    model.Base.websiteAddressDomainName0 = dumsInfoBaseTb.Rows[0]["websiteAddressDomainName0"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["websiteAddressDomainName0"]);
                    model.Base.websiteAddressUrl1 = dumsInfoBaseTb.Rows[0]["websiteAddressUrl1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["websiteAddressUrl1"]);
                    model.Base.websiteAddressDomainName1 = dumsInfoBaseTb.Rows[0]["websiteAddressDomainName1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["websiteAddressDomainName1"]);
                    model.Base.websiteAddressUrl2 = dumsInfoBaseTb.Rows[0]["websiteAddressUrl2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["websiteAddressUrl2"]);
                    model.Base.websiteAddressDomainName2 = dumsInfoBaseTb.Rows[0]["websiteAddressDomainName2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["websiteAddressDomainName2"]);
                    model.Base.websiteAddressUrl3 = dumsInfoBaseTb.Rows[0]["websiteAddressUrl3"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["websiteAddressUrl3"]);
                    model.Base.websiteAddressDomainName3 = dumsInfoBaseTb.Rows[0]["websiteAddressDomainName3"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["websiteAddressDomainName3"]);
                    model.Base.telephoneNumber = dumsInfoBaseTb.Rows[0]["telephoneNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["telephoneNumber"]);
                    model.Base.telephoneIsdCode = dumsInfoBaseTb.Rows[0]["telephoneIsdCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["telephoneIsdCode"]);
                    model.Base.isUnreachable = dumsInfoBaseTb.Rows[0]["isUnreachable"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["isUnreachable"]);
                    model.Base.faxNumber = dumsInfoBaseTb.Rows[0]["faxNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["faxNumber"]);
                    model.Base.isdCode = dumsInfoBaseTb.Rows[0]["isdCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["isdCode"]);
                    model.Base.primaryAddressLanguageDescription = dumsInfoBaseTb.Rows[0]["primaryAddressLanguageDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressLanguageDescription"]);
                    model.Base.primaryAddressLanguageDnbCode = dumsInfoBaseTb.Rows[0]["primaryAddressLanguageDnbCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["primaryAddressLanguageDnbCode"]);
                    model.Base.primaryAddressCountry = dumsInfoBaseTb.Rows[0]["primaryAddressCountry"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressCountry"]);
                    model.Base.primaryAddressCountryIsoAlpha2Code = dumsInfoBaseTb.Rows[0]["primaryAddressCountryIsoAlpha2Code"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressCountryIsoAlpha2Code"]);
                    model.Base.primaryAddressCountryFipsCode = dumsInfoBaseTb.Rows[0]["primaryAddressCountryFipsCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressCountryFipsCode"]);
                    model.Base.primaryAddressContinentalState = dumsInfoBaseTb.Rows[0]["primaryAddressContinentalState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressContinentalState"]);
                    model.Base.primaryAddressCity = dumsInfoBaseTb.Rows[0]["primaryAddressCity"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressCity"]);
                    model.Base.primaryAddressMinorTownName = dumsInfoBaseTb.Rows[0]["primaryAddressMinorTownName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressMinorTownName"]);
                    model.Base.primaryAddressState = dumsInfoBaseTb.Rows[0]["primaryAddressState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressState"]);
                    model.Base.primaryAddressStateAbbreviatedName = dumsInfoBaseTb.Rows[0]["primaryAddressStateAbbreviatedName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStateAbbreviatedName"]);
                    model.Base.primaryAddressStateFipsCode = dumsInfoBaseTb.Rows[0]["primaryAddressStateFipsCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStateFipsCode"]);
                    model.Base.primaryAddressCounty = dumsInfoBaseTb.Rows[0]["primaryAddressCounty"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressCounty"]);
                    model.Base.primaryAddressCountyFipsCode = dumsInfoBaseTb.Rows[0]["primaryAddressCountyFipsCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressCountyFipsCode"]);
                    model.Base.primaryAddressPostalCode = dumsInfoBaseTb.Rows[0]["primaryAddressPostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressPostalCode"]);
                    model.Base.primaryAddressPostalCodePosition = dumsInfoBaseTb.Rows[0]["primaryAddressPostalCodePosition"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressPostalCodePosition"]);
                    model.Base.primaryAddressPostalCodePositionCode = dumsInfoBaseTb.Rows[0]["primaryAddressPostalCodePositionCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["primaryAddressPostalCodePositionCode"]);
                    model.Base.primaryAddressStreetNumber = dumsInfoBaseTb.Rows[0]["primaryAddressStreetNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStreetNumber"]);
                    model.Base.primaryAddressStreetName = dumsInfoBaseTb.Rows[0]["primaryAddressStreetName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStreetName"]);
                    model.Base.primaryAddressStreetLine1 = dumsInfoBaseTb.Rows[0]["primaryAddressStreetLine1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStreetLine1"]);
                    model.Base.primaryAddressStreetLine2 = dumsInfoBaseTb.Rows[0]["primaryAddressStreetLine2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStreetLine2"]);
                    model.Base.primaryAddressPostOfficeBoxNumber = dumsInfoBaseTb.Rows[0]["primaryAddressPostOfficeBoxNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressPostOfficeBoxNumber"]);
                    model.Base.primaryAddressPostOfficeBoxTypeDescription = dumsInfoBaseTb.Rows[0]["primaryAddressPostOfficeBoxTypeDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressPostOfficeBoxTypeDescription"]);
                    model.Base.primaryAddressPostOfficeBoxTypeCode = dumsInfoBaseTb.Rows[0]["primaryAddressPostOfficeBoxTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["primaryAddressPostOfficeBoxTypeCode"]);
                    model.Base.primaryAddressLatitude = dumsInfoBaseTb.Rows[0]["primaryAddressLatitude"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressLatitude"]);
                    model.Base.primaryAddressLongitude = dumsInfoBaseTb.Rows[0]["primaryAddressLongitude"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressLongitude"]);
                    model.Base.primaryAddressGeographicalPrecisionDescription = dumsInfoBaseTb.Rows[0]["primaryAddressGeographicalPrecisionDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressGeographicalPrecisionDescription"]);
                    model.Base.primaryAddressGeographicalPrecisionCode = dumsInfoBaseTb.Rows[0]["primaryAddressGeographicalPrecisionCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["primaryAddressGeographicalPrecisionCode"]);
                    model.Base.primaryAddressIsRegistered = dumsInfoBaseTb.Rows[0]["primaryAddressIsRegistered"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["primaryAddressIsRegistered"]);
                    model.Base.primaryAddressStatisticalAreaCbsaName = dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaCbsaName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaCbsaName"]);
                    model.Base.primaryAddressStatisticalAreaCbsaCode = dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaCbsaCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaCbsaCode"]);
                    model.Base.primaryAddressStatisticalAreaEconomicAreaOfInfluenceCode = dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaEconomicAreaOfInfluenceCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaEconomicAreaOfInfluenceCode"]);
                    model.Base.primaryAddressStatisticalAreaPopulationRankNumber = dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaPopulationRankNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaPopulationRankNumber"]);
                    model.Base.primaryAddressStatisticalAreaPopulationRankCode = dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaPopulationRankCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaPopulationRankCode"]);
                    model.Base.primaryAddressStatisticalAreaPopulationRank = dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaPopulationRank"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressStatisticalAreaPopulationRank"]);
                    model.Base.primaryAddressLocationOwnershipDescription = dumsInfoBaseTb.Rows[0]["primaryAddressLocationOwnershipDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressLocationOwnershipDescription"]);
                    model.Base.primaryAddressLocationOwnershipCode = dumsInfoBaseTb.Rows[0]["primaryAddressLocationOwnershipCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressLocationOwnershipCode"]);
                    model.Base.primaryAddressPremisesAreaMeasurement = dumsInfoBaseTb.Rows[0]["primaryAddressPremisesAreaMeasurement"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressPremisesAreaMeasurement"]);
                    model.Base.primaryAddressPremisesAreaUnitDescription = dumsInfoBaseTb.Rows[0]["primaryAddressPremisesAreaUnitDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressPremisesAreaUnitDescription"]);
                    model.Base.primaryAddressPremisesAreaUnitCode = dumsInfoBaseTb.Rows[0]["primaryAddressPremisesAreaUnitCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["primaryAddressPremisesAreaUnitCode"]);
                    model.Base.primaryAddressPremisesAreaReliabilityDescription = dumsInfoBaseTb.Rows[0]["primaryAddressPremisesAreaReliabilityDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryAddressPremisesAreaReliabilityDescription"]);
                    model.Base.primaryAddressPremisesAreaReliabilityCode = dumsInfoBaseTb.Rows[0]["primaryAddressPremisesAreaReliabilityCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["primaryAddressPremisesAreaReliabilityCode"]);
                    model.Base.primaryAddressIsManufacturingLocation = dumsInfoBaseTb.Rows[0]["primaryAddressIsManufacturingLocation"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["primaryAddressIsManufacturingLocation"]);
                    model.Base.registeredAddressLanguage = dumsInfoBaseTb.Rows[0]["registeredAddressLanguage"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressLanguage"]);
                    model.Base.registeredAddressLanguageDnbCode = dumsInfoBaseTb.Rows[0]["registeredAddressLanguageDnbCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["registeredAddressLanguageDnbCode"]);
                    model.Base.registeredAddressCountry = dumsInfoBaseTb.Rows[0]["registeredAddressCountry"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressCountry"]);
                    model.Base.registeredAddressIsoAlpha2Code = dumsInfoBaseTb.Rows[0]["registeredAddressIsoAlpha2Code"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressCountry"]);
                    model.Base.registeredAddressCity = dumsInfoBaseTb.Rows[0]["registeredAddressCity"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressCity"]);
                    model.Base.registeredAddressMinorTownName = dumsInfoBaseTb.Rows[0]["registeredAddressMinorTownName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressMinorTownName"]);
                    model.Base.registeredAddressState = dumsInfoBaseTb.Rows[0]["registeredAddressState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressState"]);
                    model.Base.registeredAddressStateAbbreviatedName = dumsInfoBaseTb.Rows[0]["registeredAddressStateAbbreviatedName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressStateAbbreviatedName"]);
                    model.Base.registeredAddressCounty = dumsInfoBaseTb.Rows[0]["registeredAddressCounty"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressCounty"]);
                    model.Base.registeredAddressPostalCode = dumsInfoBaseTb.Rows[0]["registeredAddressPostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressPostalCode"]);
                    model.Base.registeredAddressPostalCodePositionDescription = dumsInfoBaseTb.Rows[0]["registeredAddressPostalCodePositionDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressPostalCodePositionDescription"]);
                    model.Base.registeredAddressPostalCodePositionCode = dumsInfoBaseTb.Rows[0]["registeredAddressPostalCodePositionCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["registeredAddressPostalCodePositionCode"]);
                    model.Base.registeredAddressStreetNumber = dumsInfoBaseTb.Rows[0]["registeredAddressStreetNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressStreetNumber"]);
                    model.Base.registeredAddressStreetName = dumsInfoBaseTb.Rows[0]["registeredAddressStreetName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressStreetName"]);
                    model.Base.registeredAddressStreetLine1 = dumsInfoBaseTb.Rows[0]["registeredAddressStreetLine1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressStreetLine1"]);
                    model.Base.registeredAddressStreetLine2 = dumsInfoBaseTb.Rows[0]["registeredAddressStreetLine2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressStreetLine2"]);
                    model.Base.registeredAddressStreetLine3 = dumsInfoBaseTb.Rows[0]["registeredAddressStreetLine3"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressStreetLine3"]);
                    model.Base.registeredAddressStreetLine4 = dumsInfoBaseTb.Rows[0]["registeredAddressStreetLine4"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressStreetLine4"]);
                    model.Base.registeredAddressPostOfficeBoxNumber = dumsInfoBaseTb.Rows[0]["registeredAddressPostOfficeBoxNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressPostOfficeBoxNumber"]);
                    model.Base.registeredAddressPostOfficeBoxType = dumsInfoBaseTb.Rows[0]["registeredAddressPostOfficeBoxType"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["registeredAddressPostOfficeBoxType"]);
                    model.Base.registeredAddressPostOfficeBoxTypeCode = dumsInfoBaseTb.Rows[0]["registeredAddressPostOfficeBoxTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["registeredAddressPostOfficeBoxTypeCode"]);
                    model.Base.mailingAddressLanguageDescription = dumsInfoBaseTb.Rows[0]["mailingAddressLanguageDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressLanguageDescription"]);
                    model.Base.mailingAddressLanguageCode = dumsInfoBaseTb.Rows[0]["mailingAddressLanguageCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["mailingAddressLanguageCode"]);
                    model.Base.mailingAddressCountry = dumsInfoBaseTb.Rows[0]["mailingAddressCountry"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressCountry"]);
                    model.Base.mailingAddressCountryIsoAlpha2Code = dumsInfoBaseTb.Rows[0]["mailingAddressCountryIsoAlpha2Code"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressCountryIsoAlpha2Code"]);
                    model.Base.mailingAddressContinentalState = dumsInfoBaseTb.Rows[0]["mailingAddressContinentalState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressContinentalState"]);
                    model.Base.mailingAddressCity = dumsInfoBaseTb.Rows[0]["mailingAddressCity"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressCity"]);
                    model.Base.mailingAddressMinorTownName = dumsInfoBaseTb.Rows[0]["mailingAddressMinorTownName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressMinorTownName"]);
                    model.Base.mailingAddressState = dumsInfoBaseTb.Rows[0]["mailingAddressState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressState"]);
                    model.Base.mailingAddressStateAbbreviatedName = dumsInfoBaseTb.Rows[0]["mailingAddressStateAbbreviatedName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressStateAbbreviatedName"]);
                    model.Base.mailingAddressCounty = dumsInfoBaseTb.Rows[0]["mailingAddressCounty"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressCounty"]);
                    model.Base.mailingAddressPostalCode = dumsInfoBaseTb.Rows[0]["mailingAddressPostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressPostalCode"]);
                    model.Base.mailingAddressPostalCodePositionDescription = dumsInfoBaseTb.Rows[0]["mailingAddressPostalCodePositionDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressPostalCodePositionDescription"]);
                    model.Base.mailingAddressPostalCodePositionCode = dumsInfoBaseTb.Rows[0]["mailingAddressPostalCodePositionCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["mailingAddressPostalCodePositionCode"]);
                    model.Base.mailingAddressPostalRoute = dumsInfoBaseTb.Rows[0]["mailingAddressPostalRoute"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressPostalRoute"]);
                    model.Base.mailingAddressStreetNumber = dumsInfoBaseTb.Rows[0]["mailingAddressStreetNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressStreetNumber"]);
                    model.Base.mailingAddressStreetName = dumsInfoBaseTb.Rows[0]["mailingAddressStreetName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressStreetName"]);
                    model.Base.mailingAddressStreetAddressLine1 = dumsInfoBaseTb.Rows[0]["mailingAddressStreetAddressLine1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressStreetAddressLine1"]);
                    model.Base.mailingAddressStreetAddressLine2 = dumsInfoBaseTb.Rows[0]["mailingAddressStreetAddressLine2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressStreetAddressLine2"]);
                    model.Base.mailingAddressPostOfficeBoxNumber = dumsInfoBaseTb.Rows[0]["mailingAddressPostOfficeBoxNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressPostOfficeBoxNumber"]);
                    model.Base.mailingAddressPostOfficeBoxTypeDescription = dumsInfoBaseTb.Rows[0]["mailingAddressPostOfficeBoxTypeDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mailingAddressPostOfficeBoxTypeDescription"]);
                    model.Base.thirdPartyAssessmentDescription = dumsInfoBaseTb.Rows[0]["thirdPartyAssessmentDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["thirdPartyAssessmentDescription"]);
                    model.Base.thirdPartyAssessmentCode = dumsInfoBaseTb.Rows[0]["thirdPartyAssessmentCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["thirdPartyAssessmentCode"]);
                    model.Base.thirdPartyAssessmentDate = dumsInfoBaseTb.Rows[0]["thirdPartyAssessmentDate"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["thirdPartyAssessmentDate"]);
                    model.Base.thirdPartyAssessmentValue = dumsInfoBaseTb.Rows[0]["thirdPartyAssessmentValue"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["thirdPartyAssessmentValue"]);
                    model.Base.businessEntityTypeDescription = dumsInfoBaseTb.Rows[0]["businessEntityTypeDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["businessEntityTypeDescription"]);
                    model.Base.businessEntityTypeCode = dumsInfoBaseTb.Rows[0]["businessEntityTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["businessEntityTypeCode"]);
                    model.Base.controlOwnershipDate = dumsInfoBaseTb.Rows[0]["controlOwnershipDate"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["controlOwnershipDate"]);
                    model.Base.controlOwnershipTypeDescription = dumsInfoBaseTb.Rows[0]["controlOwnershipTypeDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["controlOwnershipTypeDescription"]);
                    model.Base.controlOwnershipTypeCode = dumsInfoBaseTb.Rows[0]["controlOwnershipTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["controlOwnershipTypeCode"]);
                    model.Base.isAgent = dumsInfoBaseTb.Rows[0]["isAgent"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["isAgent"]);
                    model.Base.isImporter = dumsInfoBaseTb.Rows[0]["isImporter"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["isImporter"]);
                    model.Base.isExporter = dumsInfoBaseTb.Rows[0]["isExporter"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["isExporter"]);
                    model.Base.financialStatementToDate = dumsInfoBaseTb.Rows[0]["financialStatementToDate"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialStatementToDate"]);
                    model.Base.financialStatementDuration = dumsInfoBaseTb.Rows[0]["financialStatementDuration"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialStatementDuration"]);
                    model.Base.financialinformationScopeDescription = dumsInfoBaseTb.Rows[0]["financialinformationScopeDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialinformationScopeDescription"]);
                    model.Base.financialInformationScopeDnBCode = dumsInfoBaseTb.Rows[0]["financialInformationScopeDnBCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["financialInformationScopeDnBCode"]);
                    model.Base.financialsReliabilityDescription = dumsInfoBaseTb.Rows[0]["financialsReliabilityDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsReliabilityDescription"]);
                    model.Base.financialsReliabilityDnbCode = dumsInfoBaseTb.Rows[0]["financialsReliabilityDnbCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["financialsReliabilityDnbCode"]);
                    model.Base.financialsUnitCode = dumsInfoBaseTb.Rows[0]["financialsUnitCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsUnitCode"]);
                    model.Base.financialsAccountantName = dumsInfoBaseTb.Rows[0]["financialsAccountantName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsAccountantName"]);
                    model.Base.financialsYearlyRevenueValue1 = dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueValue1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueValue1"]);
                    model.Base.financialsYearlyRevenueCurrency1 = dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueCurrency1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueCurrency1"]);
                    model.Base.financialsYearlyRevenueValue2 = dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueValue2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueValue2"]);
                    model.Base.financialsYearlyRevenueCurrency2 = dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueCurrency2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueCurrency2"]);
                    model.Base.financialsYearlyRevenueTrendTimePeriodDescription0 = dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrendTimePeriodDescription0"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrendTimePeriodDescription0"]);
                    model.Base.financialsYearlyRevenueTrendTimePeriodCode0 = dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrendTimePeriodCode0"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrendTimePeriodCode0"]);
                    model.Base.financialsYearlyRevenueTrenGrowthRate0 = dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrenGrowthRate0"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrenGrowthRate0"]);
                    model.Base.financialsYearlyRevenueTrendTimePeriodDescription1 = dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrendTimePeriodDescription1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrendTimePeriodDescription1"]);
                    model.Base.financialsYearlyRevenueTrendTimePeriodCode1 = dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrendTimePeriodCode1"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrendTimePeriodCode1"]);
                    model.Base.financialsYearlyRevenueTrenGrowthRate1 = dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrenGrowthRate1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["financialsYearlyRevenueTrenGrowthRate1"]);
                    model.Base.mostSeniorPrincipalGivenName = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalGivenName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalGivenName"]);
                    model.Base.mostSeniorPrincipalFamilyName = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalFamilyName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalFamilyName"]);
                    model.Base.mostSeniorPrincipalFullName = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalFullName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalFullName"]);
                    model.Base.mostSeniorPrincipalNamePreFix = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalNamePreFix"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalNamePreFix"]);
                    model.Base.mostSeniorPrincipalNameSuffix = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalNameSuffix"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalNameSuffix"]);
                    model.Base.mostSeniorPrincipalGender = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalGender"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalGender"]);
                    model.Base.mostSeniorPrincipalJobTitle = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalJobTitle"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalJobTitle"]);
                    model.Base.mostSeniorPrincipalManagementResponsibilitiesDescription0 = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesDescription0"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesDescription0"]);
                    model.Base.mostSeniorPrincipalManagementResponsibilitiesMrcCode0 = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesMrcCode0"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesMrcCode0"]);
                    model.Base.mostSeniorPrincipalManagementResponsibilitiesDescription1 = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesDescription1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesDescription1"]);
                    model.Base.mostSeniorPrincipalManagementResponsibilitiesMrcCode1 = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesMrcCode1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesMrcCode1"]);
                    model.Base.mostSeniorPrincipalManagementResponsibilitiesDescription2 = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesDescription2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesDescription2"]);
                    model.Base.mostSeniorPrincipalManagementResponsibilitiesMrcCode2 = dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesMrcCode2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["mostSeniorPrincipalManagementResponsibilitiesMrcCode2"]);
                    model.Base.socioEconomicInformationIsMinorityOwned = dumsInfoBaseTb.Rows[0]["socioEconomicInformationIsMinorityOwned"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["socioEconomicInformationIsMinorityOwned"]);
                    model.Base.socioEconomicInformationIsSmallBusiness = dumsInfoBaseTb.Rows[0]["socioEconomicInformationIsSmallBusiness"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["socioEconomicInformationIsSmallBusiness"]);

                    model.Base.isStandalone = dumsInfoBaseTb.Rows[0]["isStandalone"] == DBNull.Value ? false : Convert.ToBoolean(dumsInfoBaseTb.Rows[0]["isStandalone"]);
                    model.Base.corporateLinkageFamilytreeRolesPlayedDescription1 = dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDescription1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDescription1"]);
                    model.Base.corporateLinkageFamilytreeRolesPlayedDnbCode1 = dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDnbCode1"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDnbCode1"]);
                    model.Base.corporateLinkageFamilytreeRolesPlayedDescription2 = dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDescription2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDescription2"]);
                    model.Base.corporateLinkageFamilytreeRolesPlayedDnbCode2 = dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDnbCode2"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDnbCode2"]);
                    model.Base.corporateLinkageFamilytreeRolesPlayedDescription3 = dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDescription3"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDescription3"]);
                    model.Base.corporateLinkageFamilytreeRolesPlayedDnbCode3 = dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDnbCode3"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["corporateLinkageFamilytreeRolesPlayedDnbCode3"]);
                    model.Base.corporateLinkageHierarchyLevel = dumsInfoBaseTb.Rows[0]["corporateLinkageHierarchyLevel"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["corporateLinkageHierarchyLevel"]);
                    model.Base.corporateLinkageGlobalUltimateFamilyTreeMembersCount = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateFamilyTreeMembersCount"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateFamilyTreeMembersCount"]);
                    model.Base.corporateLinkageGlobalUltimateDuns = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateDuns"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateDuns"]);
                    model.Base.corporateLinkageGlobalUltimatePrimaryName = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimatePrimaryName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimatePrimaryName"]);
                    model.Base.corporateLinkageGlobalUltimateCountry = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateCountry"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateCountry"]);
                    model.Base.corporateLinkageGlobalUltimateCountryIsoAlpha2Code = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateCountryIsoAlpha2Code"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateCountryIsoAlpha2Code"]);
                    model.Base.corporateLinkageGlobalUltimateCountryFipsCode = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateCountryFipsCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateCountryFipsCode"]);
                    model.Base.corporateLinkaglobalUltimateContinentalState = dumsInfoBaseTb.Rows[0]["corporateLinkaglobalUltimateContinentalState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkaglobalUltimateContinentalState"]);
                    model.Base.corporateLinkageGlobalUltimateCity = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateCity"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateCity"]);
                    model.Base.corporateLinkageGlobalUltimateState = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateState"]);
                    model.Base.corporateLinkageGlobalUltimateStateAbbreviatedName = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateStateAbbreviatedName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateStateAbbreviatedName"]);
                    model.Base.corporateLinkageGlobalUltimatePostalCode = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimatePostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimatePostalCode"]);
                    model.Base.corporateLinkageGlobalUltimateStreetAddressLine1 = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateStreetAddressLine1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateStreetAddressLine1"]);
                    model.Base.corporateLinkageGlobalUltimateStreetAddressLine2 = dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateStreetAddressLine2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageGlobalUltimateStreetAddressLine2"]);
                    model.Base.corporateLinkageDomesticUltimateDuns = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateDuns"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateDuns"]);
                    model.Base.corporateLinkageDomesticUltimatePrimaryName = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimatePrimaryName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimatePrimaryName"]);
                    model.Base.corporateLinkageDomesticUltimateCountry = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateCountry"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateCountry"]);
                    model.Base.corporateLinkageDomesticUltimateIsoAlpha2Code = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateIsoAlpha2Code"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateIsoAlpha2Code"]);
                    model.Base.corporateLinkageDomesticUltimateCountryFipsCode = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateCountryFipsCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateCountryFipsCode"]);
                    model.Base.corporateLinkageDomesticUltimateContinentalState = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateContinentalState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateContinentalState"]);
                    model.Base.corporateLinkageDomesticUltimateCity = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateCity"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateCity"]);
                    model.Base.corporateLinkageDomesticUltimateState = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateState"]);
                    model.Base.corporateLinkageDomesticUltimateStateAbbreviatedName = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateStateAbbreviatedName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateStateAbbreviatedName"]);
                    model.Base.corporateLinkageDomesticUltimatePostalCode = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimatePostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimatePostalCode"]);
                    model.Base.corporateLinkageDomesticUltimateStreetAddressLine1 = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateStreetAddressLine1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateStreetAddressLine1"]);
                    model.Base.corporateLinkageDomesticUltimateStreetAddressLine2 = dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateStreetAddressLine2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageDomesticUltimateStreetAddressLine2"]);
                    model.Base.corporateLinkageParentDuns = dumsInfoBaseTb.Rows[0]["corporateLinkageParentDuns"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentDuns"]);
                    model.Base.corporateLinkageParentName = dumsInfoBaseTb.Rows[0]["corporateLinkageParentName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentName"]);
                    model.Base.corporateLinkageParentCountry = dumsInfoBaseTb.Rows[0]["corporateLinkageParentCountry"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentCountry"]);
                    model.Base.corporateLinkageParentIsoAlpha2Code = dumsInfoBaseTb.Rows[0]["corporateLinkageParentIsoAlpha2Code"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentIsoAlpha2Code"]);
                    model.Base.corporateLinkageParentCounrtyFipsCode = dumsInfoBaseTb.Rows[0]["corporateLinkageParentCounrtyFipsCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentCounrtyFipsCode"]);
                    model.Base.corporateLinkageParentContinentalState = dumsInfoBaseTb.Rows[0]["corporateLinkageParentContinentalState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentContinentalState"]);
                    model.Base.corporateLinkageParentCity = dumsInfoBaseTb.Rows[0]["corporateLinkageParentCity"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentCity"]);
                    model.Base.corporateLinkageParentState = dumsInfoBaseTb.Rows[0]["corporateLinkageParentState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentState"]);
                    model.Base.corporateLinkageParentStateAbbreviatedName = dumsInfoBaseTb.Rows[0]["corporateLinkageParentStateAbbreviatedName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentStateAbbreviatedName"]);
                    model.Base.corporateLinkageParentPostalCode = dumsInfoBaseTb.Rows[0]["corporateLinkageParentPostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentPostalCode"]);
                    model.Base.corporateLinkageParentStreetAddressLine1 = dumsInfoBaseTb.Rows[0]["corporateLinkageParentStreetAddressLine1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentStreetAddressLine1"]);
                    model.Base.corporateLinkageParentStreetAddressLine2 = dumsInfoBaseTb.Rows[0]["corporateLinkageParentStreetAddressLine2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageParentStreetAddressLine2"]);
                    model.Base.corporateLinkageHeadQuarterDuns = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterDuns"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterDuns"]);
                    model.Base.corporateLinkageHeadQuarter = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarter"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarter"]);
                    model.Base.corporateLinkageHeadQuarterCountry = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterCountry"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterCountry"]);
                    model.Base.corporateLinkageHeadQuarterIsoAlpha2Code = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterIsoAlpha2Code"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterIsoAlpha2Code"]);
                    model.Base.corporateLinkageHeadQuarterCountryFipscode = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterCountryFipscode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterCountryFipscode"]);
                    model.Base.corporateLinkageHeadQuarterContinentalState = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterContinentalState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterContinentalState"]);
                    model.Base.corporateLinkageHeadQuarterCity = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterCity"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterCity"]);
                    model.Base.corporateLinkageHeadQuarterState = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterState"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterState"]);
                    model.Base.corporateLinkageHeadQuarterStateAbbreviatedName = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterStateAbbreviatedName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterStateAbbreviatedName"]);
                    model.Base.corporateLinkageHeadQuarterPostalCode = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterPostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterPostalCode"]);
                    model.Base.corporateLinkageHeadQuarterStreetAddressLine1 = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterStreetAddressLine1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterStreetAddressLine1"]);
                    model.Base.corporateLinkageHeadQuarterStreetAddressLine2 = dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterStreetAddressLine2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["corporateLinkageHeadQuarterStreetAddressLine2"]);

                    model.Base.PrimarySic = dumsInfoBaseTb.Rows[0]["PrimarySic"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["PrimarySic"]);
                    model.Base.PrimarySicDesc = dumsInfoBaseTb.Rows[0]["PrimarySicDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["PrimarySicDesc"]);
                    model.Base.SecondSic = dumsInfoBaseTb.Rows[0]["SecondSic"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SecondSic"]);
                    model.Base.SecondSicDesc = dumsInfoBaseTb.Rows[0]["SecondSicDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SecondSicDesc"]);
                    model.Base.ThirdSic = dumsInfoBaseTb.Rows[0]["ThirdSic"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ThirdSic"]);
                    model.Base.ThirdSicDesc = dumsInfoBaseTb.Rows[0]["ThirdSicDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ThirdSicDesc"]);
                    model.Base.FourthSic = dumsInfoBaseTb.Rows[0]["FourthSic"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FourthSic"]);
                    model.Base.FourthSicDesc = dumsInfoBaseTb.Rows[0]["FourthSicDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FourthSicDesc"]);
                    model.Base.FifthSic = dumsInfoBaseTb.Rows[0]["FifthSic"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FifthSic"]);
                    model.Base.FifthSicDesc = dumsInfoBaseTb.Rows[0]["FifthSicDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FifthSicDesc"]);
                    model.Base.SixthSic = dumsInfoBaseTb.Rows[0]["SixthSic"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SixthSic"]);
                    model.Base.SixthSicDesc = dumsInfoBaseTb.Rows[0]["SixthSicDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SixthSicDesc"]);
                    model.Base.PrimaryNaics = dumsInfoBaseTb.Rows[0]["PrimaryNaics"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["PrimaryNaics"]);
                    model.Base.PrimaryNaicsDesc = dumsInfoBaseTb.Rows[0]["PrimaryNaicsDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["PrimaryNaicsDesc"]);
                    model.Base.SecondNaics = dumsInfoBaseTb.Rows[0]["SecondNaics"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SecondNaics"]);
                    model.Base.SecondNaicsDesc = dumsInfoBaseTb.Rows[0]["SecondNaicsDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SecondNaicsDesc"]);
                    model.Base.ThirdNaics = dumsInfoBaseTb.Rows[0]["ThirdNaics"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ThirdNaics"]);
                    model.Base.ThirdNaicsDesc = dumsInfoBaseTb.Rows[0]["ThirdNaicsDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ThirdNaicsDesc"]);
                    model.Base.FourthNaics = dumsInfoBaseTb.Rows[0]["FourthNaics"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FourthNaics"]);
                    model.Base.FourthNaicsDesc = dumsInfoBaseTb.Rows[0]["FourthNaicsDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FourthNaicsDesc"]);
                    model.Base.FifthNaics = dumsInfoBaseTb.Rows[0]["FifthNaics"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FifthNaics"]);
                    model.Base.FifthNaicsDesc = dumsInfoBaseTb.Rows[0]["FifthNaicsDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FifthNaicsDesc"]);
                    model.Base.SixthNaics = dumsInfoBaseTb.Rows[0]["SixthNaics"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SixthNaics"]);
                    model.Base.SixthNaicsDesc = dumsInfoBaseTb.Rows[0]["SixthNaicsDesc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SixthNaicsDesc"]);
                    model.Base.PrimarySic8 = dumsInfoBaseTb.Rows[0]["PrimarySic8"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["PrimarySic8"]);
                    model.Base.PrimarySic8Desc = dumsInfoBaseTb.Rows[0]["PrimarySic8Desc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["PrimarySic8Desc"]);
                    model.Base.SecondSic8 = dumsInfoBaseTb.Rows[0]["SecondSic8"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SecondSic8"]);
                    model.Base.SecondSic8Desc = dumsInfoBaseTb.Rows[0]["SecondSic8Desc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SecondSic8Desc"]);
                    model.Base.ThirdSic8 = dumsInfoBaseTb.Rows[0]["ThirdSic8"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ThirdSic8"]);
                    model.Base.ThirdSic8Desc = dumsInfoBaseTb.Rows[0]["ThirdSic8Desc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ThirdSic8Desc"]);
                    model.Base.FourthSic8 = dumsInfoBaseTb.Rows[0]["FourthSic8"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FourthSic8"]);
                    model.Base.FourthSic8Desc = dumsInfoBaseTb.Rows[0]["FourthSic8Desc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FourthSic8Desc"]);
                    model.Base.FifthSic8 = dumsInfoBaseTb.Rows[0]["FifthSic8"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FifthSic8"]);
                    model.Base.FifthSic8Desc = dumsInfoBaseTb.Rows[0]["FifthSic8Desc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["FifthSic8Desc"]);
                    model.Base.SixthSic8 = dumsInfoBaseTb.Rows[0]["SixthSic8"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SixthSic8"]);
                    model.Base.SixthSic8Desc = dumsInfoBaseTb.Rows[0]["SixthSic8Desc"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["SixthSic8Desc"]);
                    model.Base.UsTaxId = dumsInfoBaseTb.Rows[0]["UsTaxId"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["UsTaxId"]);
                    model.Base.ConsolidatedNumberOfEmployees = dumsInfoBaseTb.Rows[0]["ConsolidatedNumberOfEmployees"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ConsolidatedNumberOfEmployees"]);
                    model.Base.ConsolidatedReliabilityDescription = dumsInfoBaseTb.Rows[0]["ConsolidatedReliabilityDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ConsolidatedReliabilityDescription"]);
                    model.Base.ConsolidatedReliabilityDnBCode = dumsInfoBaseTb.Rows[0]["ConsolidatedReliabilityDnBCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ConsolidatedReliabilityDnBCode"]);
                    model.Base.ConsolidatedGrowthRate = dumsInfoBaseTb.Rows[0]["ConsolidatedGrowthRate"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ConsolidatedGrowthRate"]);
                    model.Base.ConsolidatedTimePeriod = dumsInfoBaseTb.Rows[0]["ConsolidatedTimePeriod"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["ConsolidatedTimePeriod"]);
                    model.Base.IndividualNumberOfEmployees = dumsInfoBaseTb.Rows[0]["IndividualNumberOfEmployees"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["IndividualNumberOfEmployees"]);
                    model.Base.IndividualReliabilityDescription = dumsInfoBaseTb.Rows[0]["IndividualReliabilityDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["IndividualReliabilityDescription"]);
                    model.Base.IndividualReliabilityDnBCode = dumsInfoBaseTb.Rows[0]["IndividualReliabilityDnBCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["IndividualReliabilityDnBCode"]);
                    model.Base.IndividualGrowthRate = dumsInfoBaseTb.Rows[0]["IndividualGrowthRate"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["IndividualGrowthRate"]);
                    model.Base.IndividualTimePeriod = dumsInfoBaseTb.Rows[0]["IndividualTimePeriod"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["IndividualTimePeriod"]);
                    model.Base.primaryStockSymbol = dumsInfoBaseTb.Rows[0]["primaryStockSymbol"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryStockSymbol"]);
                    model.Base.primaryStockExchange = dumsInfoBaseTb.Rows[0]["primaryStockExchange"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryStockExchange"]);
                    model.Base.primaryStockExchangeCountryIsoAlpha2Code = dumsInfoBaseTb.Rows[0]["primaryStockExchangeCountryIsoAlpha2Code"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoBaseTb.Rows[0]["primaryStockExchangeCountryIsoAlpha2Code"]);
                }

                DataTable dumsInfoPrincipalsTb = ds.Tables[2];
                model.lstCurrentPrincipals = new List<SFI_CMPELK_CurrentPrincipalsclass>();
                SFI_CMPELK_CurrentPrincipalsclass objPrincipal = new SFI_CMPELK_CurrentPrincipalsclass();
                if (dumsInfoPrincipalsTb != null && dumsInfoPrincipalsTb.Rows != null && dumsInfoPrincipalsTb.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < dumsInfoPrincipalsTb.Rows.Count; cnt++)
                    {
                        objPrincipal = new SFI_CMPELK_CurrentPrincipalsclass();

                        objPrincipal.DnbDUNSNumber = dumsInfoPrincipalsTb.Rows[cnt]["DnbDUNSNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["DnbDUNSNumber"]);
                        objPrincipal.APIType = dumsInfoPrincipalsTb.Rows[cnt]["APIType"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["APIType"]);
                        objPrincipal.TransactionTimestamp = dumsInfoPrincipalsTb.Rows[cnt]["TransactionTimestamp"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["TransactionTimestamp"]);
                        objPrincipal.givenName = dumsInfoPrincipalsTb.Rows[cnt]["givenName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["givenName"]);
                        objPrincipal.familyName = dumsInfoPrincipalsTb.Rows[cnt]["familyName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["familyName"]);
                        objPrincipal.fullName = dumsInfoPrincipalsTb.Rows[cnt]["fullName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["fullName"]);
                        objPrincipal.namePrefix = dumsInfoPrincipalsTb.Rows[cnt]["namePrefix"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["namePrefix"]);
                        objPrincipal.nameSuffix = dumsInfoPrincipalsTb.Rows[cnt]["nameSuffix"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["nameSuffix"]);
                        objPrincipal.gender = dumsInfoPrincipalsTb.Rows[cnt]["gender"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["gender"]);
                        objPrincipal.jobTitle = dumsInfoPrincipalsTb.Rows[cnt]["jobTitle"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["jobTitle"]);
                        objPrincipal.managementResponsibilitieCode0 = dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitieCode0"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitieCode0"]);
                        objPrincipal.managementResponsibilitie0 = dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitie0"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitie0"]);
                        objPrincipal.managementResponsibilitieCode1 = dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitieCode1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitieCode1"]);
                        objPrincipal.managementResponsibilitie1 = dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitie1"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitie1"]);
                        objPrincipal.managementResponsibilitieCode2 = dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitieCode2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitieCode2"]);
                        objPrincipal.managementResponsibilitie2 = dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitie2"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["managementResponsibilitie2"]);
                        objPrincipal.RecordIdent = dumsInfoPrincipalsTb.Rows[cnt]["DnbDUNSNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoPrincipalsTb.Rows[cnt]["DnbDUNSNumber"]) + cnt;
                        model.lstCurrentPrincipals.Add(objPrincipal);
                    }
                }

                model.lstIndustryCodes = new List<SFI_CMPELK_IndustryCodesclass>();
                SFI_CMPELK_IndustryCodesclass objIndustryCodes = new SFI_CMPELK_IndustryCodesclass();
                DataTable dumsInfoIndustrydt = ds.Tables[1];
                if (dumsInfoIndustrydt != null && dumsInfoIndustrydt.Rows != null && dumsInfoIndustrydt.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < dumsInfoIndustrydt.Rows.Count; cnt++)
                    {
                        objIndustryCodes = new SFI_CMPELK_IndustryCodesclass();
                        objIndustryCodes.DnbDUNSNumber = dumsInfoIndustrydt.Rows[cnt]["DnbDUNSNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoIndustrydt.Rows[cnt]["DnbDUNSNumber"]);
                        objIndustryCodes.APIType = dumsInfoIndustrydt.Rows[cnt]["APIType"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoIndustrydt.Rows[cnt]["APIType"]);
                        objIndustryCodes.TransactionTimestamp = dumsInfoIndustrydt.Rows[cnt]["TransactionTimestamp"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoIndustrydt.Rows[cnt]["TransactionTimestamp"]);
                        objIndustryCodes.priority = dumsInfoIndustrydt.Rows[cnt]["priority"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoIndustrydt.Rows[cnt]["priority"]);
                        objIndustryCodes.industryCode = dumsInfoIndustrydt.Rows[cnt]["industryCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoIndustrydt.Rows[cnt]["industryCode"]);
                        objIndustryCodes.industryDescription = dumsInfoIndustrydt.Rows[cnt]["industryDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoIndustrydt.Rows[cnt]["industryDescription"]);
                        objIndustryCodes.typeCode = dumsInfoIndustrydt.Rows[cnt]["typeCode"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoIndustrydt.Rows[cnt]["typeCode"]);
                        objIndustryCodes.typeDescription = dumsInfoIndustrydt.Rows[cnt]["typeDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoIndustrydt.Rows[cnt]["typeDescription"]);
                        objIndustryCodes.RowId = Convert.ToString(cnt);
                        objIndustryCodes.RecordIdent = dumsInfoIndustrydt.Rows[cnt]["DnbDUNSNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoIndustrydt.Rows[cnt]["DnbDUNSNumber"]) + cnt;
                        model.lstIndustryCodes.Add(objIndustryCodes);
                    }
                }




                model.lstRegistrationNumbers = new List<SFI_CMPELK_RegistrationNumbersclass>();
                SFI_CMPELK_RegistrationNumbersclass objRegistrationNumbers = new SFI_CMPELK_RegistrationNumbersclass();
                DataTable dumsInfoRegistrationNondt = ds.Tables[3];
                if (dumsInfoRegistrationNondt != null && dumsInfoRegistrationNondt.Rows != null && dumsInfoRegistrationNondt.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < dumsInfoRegistrationNondt.Rows.Count; cnt++)
                    {
                        objRegistrationNumbers = new SFI_CMPELK_RegistrationNumbersclass();
                        objRegistrationNumbers.DnbDUNSNumber = dumsInfoRegistrationNondt.Rows[cnt]["DnbDUNSNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoRegistrationNondt.Rows[cnt]["DnbDUNSNumber"]);
                        objRegistrationNumbers.APIType = dumsInfoRegistrationNondt.Rows[cnt]["APIType"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoRegistrationNondt.Rows[cnt]["APIType"]);
                        objRegistrationNumbers.TransactionTimestamp = dumsInfoRegistrationNondt.Rows[cnt]["TransactionTimestamp"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoRegistrationNondt.Rows[cnt]["TransactionTimestamp"]);
                        objRegistrationNumbers.registrationNumber = dumsInfoRegistrationNondt.Rows[cnt]["registrationNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoRegistrationNondt.Rows[cnt]["registrationNumber"]);
                        objRegistrationNumbers.registrationNumbersTypeDescription = dumsInfoRegistrationNondt.Rows[cnt]["registrationNumbersTypeDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoRegistrationNondt.Rows[cnt]["registrationNumbersTypeDescription"]);
                        objRegistrationNumbers.registrationNumbersTypeCode = dumsInfoRegistrationNondt.Rows[cnt]["registrationNumbersTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(dumsInfoRegistrationNondt.Rows[cnt]["registrationNumbersTypeCode"]);
                        objRegistrationNumbers.RecordIdent = "114315195 - 1";
                        model.lstRegistrationNumbers.Add(objRegistrationNumbers);
                    }
                }




                model.lstStockExchanges = new List<SFI_CMPELK_StockExchangesclass>();
                SFI_CMPELK_StockExchangesclass objStockExchanges = new SFI_CMPELK_StockExchangesclass();
                DataTable dumsInfoStockdt = ds.Tables[4];
                if (dumsInfoStockdt != null && dumsInfoStockdt.Rows != null && dumsInfoStockdt.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < dumsInfoStockdt.Rows.Count; cnt++)
                    {
                        objStockExchanges = new SFI_CMPELK_StockExchangesclass();
                        objStockExchanges.DnbDUNSNumber = dumsInfoStockdt.Rows[cnt]["DnbDUNSNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoStockdt.Rows[cnt]["DnbDUNSNumber"]);
                        objStockExchanges.APIType = dumsInfoStockdt.Rows[cnt]["APIType"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoStockdt.Rows[cnt]["APIType"]);
                        objStockExchanges.TransactionTimestamp = dumsInfoStockdt.Rows[cnt]["TransactionTimestamp"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoStockdt.Rows[cnt]["TransactionTimestamp"]);
                        objStockExchanges.tickerName = dumsInfoStockdt.Rows[cnt]["tickerName"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoStockdt.Rows[cnt]["tickerName"]);
                        objStockExchanges.description = dumsInfoStockdt.Rows[cnt]["description"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoStockdt.Rows[cnt]["description"]);
                        objStockExchanges.countryIsoAlpha2Code = dumsInfoStockdt.Rows[cnt]["countryIsoAlpha2Code"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoStockdt.Rows[cnt]["countryIsoAlpha2Code"]);
                        objStockExchanges.isPrimary = dumsInfoStockdt.Rows[cnt]["isPrimary"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoStockdt.Rows[cnt]["isPrimary"]);
                        objStockExchanges.RecordIdent = dumsInfoStockdt.Rows[cnt]["DnbDUNSNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dumsInfoStockdt.Rows[cnt]["DnbDUNSNumber"]) + cnt;
                        model.lstStockExchanges.Add(objStockExchanges);
                    }
                }
            }
            return model;
        }
        #endregion

        #region "PreviewBenificialOwnershipData"
        public static BeneficialOwnership_Main BenificialOwnershipDataPreview(BeneficialOwnership_Main model, DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    model.Base = CommonConvertMethods.GetItem<CMPBOSV1_Base>(item);
                }

                if (ds.Tables.Count > 1)
                {
                    model.lstBeneficialOwnerRelationships = CommonConvertMethods.ConvertDataTable<CMPBOSV1_BeneficialOwnerRelationships>(ds.Tables[1]);
                }

                if (ds.Tables.Count > 2)
                {
                    model.lstBeneficialOwners = CommonConvertMethods.ConvertDataTable<CMPBOSV1_BeneficialOwners>(ds.Tables[2]);
                }

                if (ds.Tables.Count > 3)
                {
                    model.lstBeneficialOwnershipCountryWiseSummary = CommonConvertMethods.ConvertDataTable<CMPBOSV1_BeneficialOwnershipCountryWiseSummary>(ds.Tables[3]);
                }

                if (ds.Tables.Count > 4)
                {
                    model.lstCombinedData = CommonConvertMethods.ConvertDataTable<CMPBOSV1_CombinedData>(ds.Tables[4]);
                }

                if (ds.Tables.Count > 5)
                {
                    if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[5].Rows)
                        {
                            model.graphJson = CommonConvertMethods.GetItem<GraphJson>(dr);
                            model.graphJson.ResultJSON = Regex.Unescape(model.graphJson.ResultJSON);
                        }
                    }
                }

                if (ds.Tables.Count > 6)
                {
                    model.lstBeneficialOwnershipCountryWisePSCSummary = CommonConvertMethods.ConvertDataTable<CMPBOSV1_BeneficialOwnershipCountryWisePSCSummary>(ds.Tables[6]);
                }

                if (ds.Tables.Count > 7)
                {
                    model.lstBeneficialOwnershipNationalityWisePSCSummary = CommonConvertMethods.ConvertDataTable<CMPBOSV1_BeneficialOwnershipNationalityWisePSCSummary>(ds.Tables[7]);
                }

                if (ds.Tables.Count > 8)
                {
                    model.lstBeneficialOwnershipTypeWisePSCSummary = CommonConvertMethods.ConvertDataTable<CMPBOSV1_BeneficialOwnershipTypeWisePSCSummary>(ds.Tables[8]);
                }
            }
            return model;
        }
        #endregion

        #region Common function for displaying formatted address in Preview Enrichment Data
        public static string DisplayFormattedAddress(string primaryAddressStreetLine1, string primaryAddressCity, string primaryAddressState, string primaryAddressPostalCode, string primaryAddressCountryIsoAlpha2Code)
        {
            string Address = string.Empty;
            if (!string.IsNullOrEmpty(primaryAddressStreetLine1))
            {
                Address = primaryAddressStreetLine1;
            }
            if (!string.IsNullOrEmpty(primaryAddressCity))
            {
                Address = Address + ", " + primaryAddressCity;
            }
            if (!string.IsNullOrEmpty(primaryAddressState))
            {
                Address = Address + ", " + primaryAddressState;
            }
            if (!string.IsNullOrEmpty(primaryAddressPostalCode))
            {
                Address = Address + ", " + primaryAddressPostalCode;
            }
            if (!string.IsNullOrEmpty(primaryAddressCountryIsoAlpha2Code))
            {
                Address = Address + ", " + primaryAddressCountryIsoAlpha2Code;
            }
            if (string.IsNullOrEmpty(Address))
            {
                return Address;
            }
            else
            {
                return Address.Trim().TrimStart(',');
            }
        }
        #endregion

        public static bool isValidEmail(string inputEmail)
        {
            if (string.IsNullOrEmpty(inputEmail))
            {
                return true;
            }
            try
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                      @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                      @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);

                if (re.IsMatch(inputEmail) || string.IsNullOrEmpty(inputEmail))
                    return (true);
                else
                    return (false);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string SecondToMinute(int secs)
        {
            string convertedString = string.Empty;
            if (secs > 60)
            {
                secs = secs / 60;
                convertedString = "In " + secs + " minutes";
            }
            else
            {
                convertedString = "In " + secs + " seconds";
            }
            return convertedString;
        }

        public static string SecondToMinuteAccurate(int secs)
        {
            string convertedString = string.Empty;
            TimeSpan t = TimeSpan.FromSeconds(secs);
            if (t.Hours > 0)
                convertedString = string.Format("{0:D2} hours {1:D2} minutes {2:D2} seconds", t.Hours, t.Minutes, t.Seconds);
            else if (t.Minutes > 0)
                convertedString = string.Format("{0:D2} minutes {1:D2} seconds", t.Minutes, t.Seconds);
            else
                convertedString = string.Format("{0:D2} seconds", t.Seconds);
            return convertedString;
        }

        public static string GetETLTypeColorClass(int i)
        {
            string color = string.Empty;
            switch (i)
            {
                case 1:
                    color = "ColorA";
                    break;
                case 2:
                    color = "ColorB";
                    break;
                case 3:
                    color = "ColorC";
                    break;
                case 4:
                    color = "ColorD";
                    break;
                case 5:
                    color = "ColorF";
                    break;
                case 6:
                    color = "ColorZ";
                    break;
            }
            return color;
        }

        public static DataTable ExcelToDataTable(HttpPostedFileBase fileBase)
        {
            DataTable dt = new DataTable();
            IExcelDataReader reader = null;

            if (System.IO.Path.GetExtension(fileBase.FileName).Equals(".xls"))
                reader = ExcelReaderFactory.CreateBinaryReader(fileBase.InputStream);
            else if (System.IO.Path.GetExtension(fileBase.FileName).Equals(".xlsx"))
                reader = ExcelReaderFactory.CreateOpenXmlReader(fileBase.InputStream);
            if (reader != null)
            {
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true, EmptyColumnNamePrefix = "Column" }
                };
                //Fill DataSet
                DataSet content = reader.AsDataSet(conf);
                dt = content.Tables[0].AsEnumerable().Skip(0).CopyToDataTable();
            }
            return dt;
        }

        #region "Logs"
        public static void logMessage(string logtxt)
        {
            string dir = HttpContext.Current.Server.MapPath("~/LanguageLogs/");
            string path = dir + "/" + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (File.Exists(path))
            {
                using (FileStream aFile = new FileStream(path, FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(aFile))
                {
                    sw.WriteLine(logtxt);
                    sw.WriteLine("********************************************************************************************************************************************");
                }
            }
            else
            {
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine(logtxt);
                    tw.WriteLine("********************************************************************************************************************************************");
                }
            }
        }
        #endregion

        #region ResearchSubtype List
        public static List<DropDownReturn> GetiResearchMarketApplicability(string MarketApplicability, string ConnectionString, int fulllist = 0)
        {
            List<DropDownReturn> lstResearchInvestigation = new List<DropDownReturn>();
            iResearchFacade fac = new iResearchFacade(ConnectionString);
            DataTable dt = fac.GetiResearchMarketApplicability(MarketApplicability, fulllist);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstResearchInvestigation.Add(new DropDownReturn { Value = dt.Rows[i]["SubTypeCode"].ToString(), Text = dt.Rows[i]["SubTypeCode"].ToString() + "-" + dt.Rows[i]["ResearchSubType"].ToString() });
            }
            return lstResearchInvestigation;
        }
        #endregion

        #region "Benificiary Graph Json"
        public static string CreatejsonForGraph()
        {
            BeneficialOwnership_Main modal = new BeneficialOwnership_Main();
            modal = JsonConvert.DeserializeObject<BeneficialOwnership_Main>(SessionHelper.BeneficialOwnershipData);
            List<int> ownerIds = modal.lstBeneficialOwnerRelationships.Select(x => x.sourceMemberID).Distinct().ToList();
            BenificiaryGraphModel graphModel = new BenificiaryGraphModel();
            graphModel.nodes = new List<GraphNodes>();
            graphModel.edges = new List<GraphEdges>();

            foreach (var item in ownerIds)
            {
                var objOwner = modal.lstBeneficialOwners.Where(x => x.memberID == item).FirstOrDefault();
                GraphNodes nodes = new GraphNodes();
                nodes.id = item.ToString();
                nodes.attributes = new NodeAttributes()
                {
                    color = "#67328E",
                    radius = 10,
                    text = objOwner.duns + " - " + objOwner.name
                };
                nodes.data = new NodeData()
                {
                    area = "owner"
                };
                graphModel.nodes.Add(nodes);
            }
            foreach (var item in ownerIds)
            {
                foreach (var relation in modal.lstBeneficialOwnerRelationships.Where(x => x.sourceMemberID == item))
                {
                    var objRelative = modal.lstBeneficialOwners.Where(x => x.memberID == relation.targetMemberID).FirstOrDefault();
                    GraphNodes relnode = new GraphNodes();
                    relnode.id = relation.targetMemberID.ToString();
                    relnode.attributes = new NodeAttributes()
                    {
                        color = "#328E5B",
                        radius = 8,
                        text = objRelative.memberID + " - " + objRelative.name
                    };
                    relnode.data = new NodeData()
                    {
                        area = "relative"
                    };
                    graphModel.nodes.Add(relnode);

                    GraphEdges edges = new GraphEdges();
                    edges.id = item.ToString() + "-" + relation.targetMemberID.ToString();
                    edges.source = item.ToString();
                    edges.target = relation.targetMemberID.ToString();
                    graphModel.edges.Add(edges);
                }
            }
            return JsonConvert.SerializeObject(graphModel);
        }
        #endregion

        #region Add configuration settings to UI - Client Portal

        #region Common Method for Azure Connection
        public static async Task<bool> CheckAzureConnection(DataSourceConfigurationEntity objDataSourceCconfiguration)
        {
            var storageCredentials = new StorageCredentials(objDataSourceCconfiguration.azure.AccountName, objDataSourceCconfiguration.azure.AccountKey);
            var cloudStorageAccount = new CloudStorageAccount(storageCredentials, objDataSourceCconfiguration.azure.EndpointSuffix, useHttps: true);
            var blob = cloudStorageAccount.CreateCloudBlobClient();
            var container = blob.GetContainerReference("container");
            try
            {
                await container.ExistsAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Common Method for Amazon Connection
        public static async Task<bool> CheckAmazonConnection(DataSourceConfigurationEntity objDataSourceCconfiguration)
        {
            try
            {
                AmazonS3Config conf = new AmazonS3Config();
                conf.ServiceURL = objDataSourceCconfiguration.amazon.ServiceURL;
                using (var client = new AmazonS3Client(objDataSourceCconfiguration.amazon.AccessKey, objDataSourceCconfiguration.amazon.SecurityKey, conf))
                {
                    ListBucketsResponse response = await client.ListBucketsAsync();
                    if (response.Buckets != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred: " + ex.Message);
            }
        }
        #endregion

        #region Common Method for FTP Connection
        public static bool CheckFTPConnection(DPMFTPConfigurationEntity objFTPConfiguration)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(objFTPConfiguration.Host);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                request.Credentials = new NetworkCredential(objFTPConfiguration.UserName, objFTPConfiguration.Password);
                request.KeepAlive = false;
                request.UseBinary = true;
                request.UsePassive = true;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    try
                    {
                        response.GetResponseStream();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Common Method for SFTP Connection
        public static string CreateAndUploadBlobFile(string ApplicationSubDomain, HttpPostedFileBase file, string fileName, string containerName)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            if (!container.Exists())
                container.Create(BlobContainerPublicAccessType.Blob);
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            CloudBlockBlob blob = container.GetBlockBlobReference(ApplicationSubDomain + "/" + fileName);
            blob.UploadFromStreamAsync(file.InputStream);
            return blob.Uri.AbsoluteUri;
        }
        public static Stream DownloadBlobFile(string ApplicationSubDomain, string fileName)
        {
            CloudBlockBlob objBlob;
            using (MemoryStream objMemoryStream = new MemoryStream())
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(ExternalDataSources.SFTP.ToString().ToLower());
                objBlob = container.GetBlockBlobReference(ApplicationSubDomain + "/" + fileName);
                objBlob.DownloadToStream(objMemoryStream);
            }
            Stream blobStream = objBlob.OpenReadAsync().Result;
            return blobStream;
        }
        public static void DeleteBlobFile(string ApplicationSubDomain, string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(ExternalDataSources.SFTP.ToString().ToLower());
                if (!container.Exists())
                    container.Create(BlobContainerPublicAccessType.Blob);
                CloudBlockBlob blob = container.GetBlockBlobReference(filename);
                blob.DeleteIfExists();
            }
        }
        public static bool CheckSFTPConnection(DataSourceConfigurationEntity objDataSourceCconfiguration, Stream file)
        {
            try
            {
                PrivateKeyFile keyFile = new PrivateKeyFile(file);
                var keyFiles = new[] { keyFile };
                var checkSFTP = new List<AuthenticationMethod>();
                checkSFTP.Add(new PrivateKeyAuthenticationMethod(objDataSourceCconfiguration.sftp.SFTPUserName, keyFiles));
                Renci.SshNet.ConnectionInfo objConnection = new Renci.SshNet.ConnectionInfo(objDataSourceCconfiguration.sftp.SFTPHost, Convert.ToInt32(objDataSourceCconfiguration.sftp.SFTPPort), objDataSourceCconfiguration.sftp.SFTPUserName, checkSFTP.ToArray());
                using (var client = new SftpClient(objConnection))
                {
                    client.Connect();
                    client.Disconnect();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion
        //User Story 105-File Import Configuration settings page - Client Portal
        public static SelectList GetAllImportFileTemplates(string ConnectionString)
        {
            ImportJobDataEntity importJobData = new ImportJobDataEntity();
            ImportJobDataFacade efac = new ImportJobDataFacade(ConnectionString);
            importJobData.lstTemplates = new List<ImportFileTemplates>();
            importJobData.lstTemplates = efac.GetAllImportFileTemplates().ToList();
            SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
            List<SelectListItem> lstTemplate = new List<SelectListItem>();
            foreach (var item in importJobData.lstTemplates)
            {
                lstTemplate.Add(new SelectListItem { Value = Convert.ToString(item.TemplateId), Text = item.TemplateName });
            }
            return new SelectList(lstTemplate, "Value", "Text");
        }
        public static SelectList GetExternalDataStore(string ConnectionString)
        {
            ExternalSourceConfigurationFacade efac = new ExternalSourceConfigurationFacade(ConnectionString);
            List<DataSourceConfigurationEntity> lstExternalSourceConfiguration = new List<DataSourceConfigurationEntity>();
            lstExternalSourceConfiguration = efac.GetExternalDataStore(null);

            List<SelectListItem> lstExternalDataStore = new List<SelectListItem>();
            foreach (var item in lstExternalSourceConfiguration)
            {
                lstExternalDataStore.Add(new SelectListItem { Value = Convert.ToString(item.Id), Text = item.ExternalDataStoreName });
            }
            return new SelectList(lstExternalDataStore, "Value", "Text");
        }
    }
}