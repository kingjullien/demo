using Newtonsoft.Json;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Utility.Monitoring;
using SBISCCMWeb.Utility.OI;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Utility
{
    public class CommonDropdown
    {

        #region Get Tags and Tags Type Code

        public static SelectList GetTagTypeCode(string ConnectionString)
        {
            // Get All tags type code from the database and fill the dropdown 
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            TagFacade fac = new TagFacade(ConnectionString);
            DataTable dt = fac.GetTagTypeCode();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAllFilter.Add(new SelectListItem { Value = dt.Rows[i]["TagTypeCode"].ToString() + "@#$" + dt.Rows[i]["Value"].ToString(), Text = dt.Rows[i]["Description"].ToString() });
            }

            return new SelectList(lstAllFilter, "Value", "Text");
        }



        public static List<TagsEntity> GetAllTags(string ConnectionString/*,string LOBTag*/)
        {
            // Get All tags from the database and fill the dropdown 
            List<TagsEntity> model = new List<TagsEntity>();
            TagFacade fac = new TagFacade(ConnectionString);
            model = fac.GetAllTags(Helper.oUser.LOBTag);
            return model;
        }
        public static List<TagsEntity> GetTagByTypeCode(string ConnectionString)
        {
            TagFacade fac = new TagFacade(ConnectionString);
            return fac.GetTagByTypeCode(ConstantValues.LOBTagCode);
        }


        //Session Filter - Tags [Task] MP-361
        //GetAllTags - Remove LOB Tags (MP-376)
        public static List<TagsEntity> GetAllTagsForUser(string ConnectionString, bool FilterNoTag)
        {
            // Get All tags from the database and fill the dropdown 
            List<TagsEntity> model = new List<TagsEntity>();
            TagFacade fac = new TagFacade(ConnectionString);
            model = fac.GetAllTagsForUser(Helper.oUser.LOBTag, Helper.oUser.UserId, FilterNoTag);
            return model;
        }

        public static SelectList GetAllOrbTags(string ConnectionString, string LobTag = "", string SecurityTag = "")
        {
            OISettingFacade fac = new OISettingFacade(ConnectionString);
            DataTable dt = fac.GetAllOrbTags(LobTag, SecurityTag, Helper.oUser != null ? Convert.ToString(Helper.oUser.UserId) : "");
            List<SelectListItem> lstTags = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTags.Add(new SelectListItem { Value = dt.Rows[i]["Tag"].ToString(), Text = dt.Rows[i]["TagName"].ToString() });
            }
            return new SelectList(lstTags, "Value", "Text");
        }

        #endregion

        public static List<string> GetFileType()
        {
            List<string> lstFileType = new List<string>();
            lstFileType.Add(CommonMessagesLang.lblMyFiles);
            //if (Helper.oUser.UserRole == UserRole.GLOBAL.ToString() && Helper.oUser.UserType == "admin")
            //{
            lstFileType.Add(CommonMessagesLang.lblAllFiles);
            //}
            return lstFileType;
        }
        public List<CountryGroupEntity> LoadCountryGroupEntity(string ConnectionString)
        {
            // Load Country Group Entity
            SettingFacade fac = new SettingFacade(ConnectionString);
            return fac.GetCountryGroup();
        }

        #region  DropDown List for Number of Records Per Page
        public static List<int> GetPageSize()
        {
            List<int> lstsize = new List<int>();
            lstsize.Add(10);
            lstsize.Add(20);
            lstsize.Add(30);
            return lstsize;
        }

        public static List<int> GetPageSizeRevireData()
        {
            List<int> lstsize = new List<int>();
            lstsize.Add(50);
            lstsize.Add(100);
            lstsize.Add(200);
            return lstsize;
        }
        public static List<int> GetDataStewardshipPageSize()
        {
            List<int> lstsize = new List<int>();
            lstsize.Add(5);
            lstsize.Add(10);
            lstsize.Add(20);
            lstsize.Add(30);
            return lstsize;
        }
        public static List<int> GetPageSizeImportDataFile()
        {
            List<int> lstsize = new List<int>();
            lstsize.Add(5);
            lstsize.Add(10);
            return lstsize;
        }
        // For getting the OI Build List page size
        public static List<int> GetOIBuildListPageSize()
        {
            List<int> lstsize = new List<int>();
            lstsize.Add(30);
            lstsize.Add(50);
            lstsize.Add(100);
            return lstsize;
        }
        #endregion


        public static SelectList GetAllCountry(string ConnectionString)
        {
            UserSessionFacade fac = new UserSessionFacade(ConnectionString);
            List<CountryEntity> lstCountry = fac.GetCountries();
            List<SelectListItem> lstAllCountry = new List<SelectListItem>();
            foreach (var item in lstCountry)
            {
                lstAllCountry.Add(new SelectListItem { Value = item.ISOAlpha2Code, Text = item.CountryWithISOCode });
            }
            return new SelectList(lstAllCountry, "Value", "Text");
        }

        public List<MatchGradeEntity> LoadMatchGrades(string ConnectionString)
        {
            // Load Match Entity
            SettingFacade fac = new SettingFacade(ConnectionString);
            return fac.GetMatchGrades();
        }
        public List<MatchCodeEntity> LoadMatchGradesEntities(string ddlMatchGrade, string ConnectionString)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            switch (ddlMatchGrade)
            {
                case "Company":
                    return fac.GetMatchMDPCodes("Company");
                case "StreetName":
                    return fac.GetMatchMDPCodes("StreetName");
                case "StreetNo":
                    return fac.GetMatchMDPCodes("StreetNo");
                case "City":
                    return fac.GetMatchMDPCodes("City");
                case "State":
                    return fac.GetMatchMDPCodes("State");
                case "PoBox":
                    return fac.GetMatchMDPCodes("PoBox");
                case "Phone":
                    return fac.GetMatchMDPCodes("Phone");
                case "PostalCode":
                    return fac.GetMatchMDPCodes("PostalCode");
                case "Density":
                    return fac.GetMatchMDPCodes("Density");
                case "Uniqueness":
                    return fac.GetMatchMDPCodes("Uniqueness");
                case "SIC":
                    return fac.GetMatchMDPCodes("SIC");
                case "DUNS":
                    return fac.GetMatchMDPCodes("DUNS");
                case "NationalID":
                    return fac.GetMatchMDPCodes("NationalID");
                case "URL":
                    return fac.GetMatchMDPCodes("URL");
                default:
                    return fac.GetMatchMDPCodes("");
            }
        }

        //public List<string> AddComboboxItems(string ComboboxItemsName)
        //{
        //    List<string> ComboboxItems = new List<string>();
        //    switch (ComboboxItemsName)
        //    {
        //    }
        //    return ComboboxItems;
        //}

        public SelectList ComboboxItems(string ComboboxItemsName)
        {
            List<SelectListItem> lstComboboxItems = new List<SelectListItem>();
            switch (ComboboxItemsName)
            {
                case "MaximumCandidateResults":
                    //fill the Dropdown for Maximum Candidate Results
                    for (int i = 1; i <= 50; i++)
                    {
                        lstComboboxItems.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                    }
                    break;
                case "MaximumParallelThread":
                    //fill the Dropdown for Maximum Parallel Thread
                    for (int i = 1; i <= 10; i++)
                    {
                        lstComboboxItems.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                    }
                    break;
                case "AutoSettingConfidenceCode":
                    //fill the Dropdown for Auto Setting Confidence Code
                    //change in Sprint 7 (start Confidence Code with 3 inseted of 4)
                    for (int i = 3; i <= 10; i++)
                    {
                        lstComboboxItems.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                    }
                    break;
                case "CountryGroup":
                    //fill the Dropdown for CountryGroup
                    for (int i = 1; i <= 10; i++)
                    {

                    }
                    break;
                case "MinCCoverride":
                    //fill the Dropdown for CountryGroup
                    for (int i = 0; i <= 10; i++)
                    {
                        lstComboboxItems.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                    }
                    break;
                case "CompanyScore":
                    for (int i = 0; i <= 100; i++)
                    {
                        lstComboboxItems.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                    }
                    break;
                case "MinConfidenceScore":
                    for (int i = 1; i <= 100; i++)
                    {
                        lstComboboxItems.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                    }
                    break;
            }
            return new SelectList(lstComboboxItems, "Value", "Text");
        }

        #region Language
        public static SelectList GetLanguageCodes(string ConnectionString)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            DataTable dt = fac.GetLanguageCodes();
            List<SelectListItem> lstAllLanguage = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAllLanguage.Add(new SelectListItem { Value = dt.Rows[i]["inLanguage"].ToString(), Text = dt.Rows[i]["Language"].ToString() });
            }
            return new SelectList(lstAllLanguage, "Value", "Text");
        }
        #endregion
        //get  User Comment and fill dropdown list
        public List<UserCommentsEntity> LoadUserComment(string ConnectionString)
        {
            // Load User Comment
            UserCommentsFacade fac = new UserCommentsFacade(ConnectionString);
            return fac.GetUserCommentsByType("AUTOACCEPT_DELETE");
        }


        public static SelectList GetAllEnvironmentName(string ConnectionString)
        {
            List<SelectListItem> lstAllEnvironmentName = new List<SelectListItem>();
            SettingFacade fac = new SettingFacade(ConnectionString);
            DataTable dt = fac.GetCDSEnvironmentName(false);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAllEnvironmentName.Add(new SelectListItem { Value = dt.Rows[i]["OrganizationUrl"].ToString(), Text = dt.Rows[i]["EnvironmentName"].ToString() });
            }

            return new SelectList(lstAllEnvironmentName, "Value", "Text");
        }

        public static SelectList StewGetDataImportProcess(string ConnectionString, string Queue, bool IsMatchdata)
        {
            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.oUser.UserName);
            DataTable dt = fac.GetImportProcessesByQueue(Queue, IsMatchdata);
            List<SelectListItem> lstImportProcess = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstImportProcess.Add(new SelectListItem { Value = dt.Rows[i]["ImportProcess"].ToString(), Text = dt.Rows[i]["ImportProcess"].ToString() });
            }
            return new SelectList(lstImportProcess, "Value", "Text");
        }
        public static SelectList GetDataImportProcess(string ConnectionString)
        {
            OIImportDataFacade fac = new OIImportDataFacade(ConnectionString);
            DataTable dt = fac.GetDataImportProcess();
            List<SelectListItem> lstImportProcess = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstImportProcess.Add(new SelectListItem { Value = dt.Rows[i]["ImportProcess"].ToString(), Text = dt.Rows[i]["ImportProcess"].ToString() });
            }
            return new SelectList(lstImportProcess, "Value", "Text");
        }
        //DropDown List for searching OrderBy
        public static List<string> GetOrderBy()
        {
            List<string> lstsize = new List<string>();
            lstsize.Add(CommonMessagesLang.lblSrcRecordId);
            lstsize.Add(CommonMessagesLang.lblCompany);
            lstsize.Add(CommonMessagesLang.lblState);
            return lstsize;
        }


        #region "Export Data"
        public static List<TagsEntity> GetExportDataTags(string ConnectionString)
        {
            // Get All tags from the database and fill the dropdown 
            List<TagsEntity> model = new List<TagsEntity>();
            TagFacade fac = new TagFacade(ConnectionString);
            model = fac.GetExportDataTags(Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", Helper.oUser != null ? (Helper.oUser.Tags != null ? Helper.oUser.Tags : "") : "", Helper.oUser.UserId);
            return model;
        }
        public static SelectList GetExportDelimeter()
        {
            string[] DelimeterValues = ConfigurationManager.AppSettings["ExportDelimeter"].ToString().Split(',');
            List<SelectListItem> lstAllEnvironmentName = new List<SelectListItem>();
            foreach (var item in DelimeterValues)
            {
                lstAllEnvironmentName.Add(new SelectListItem { Value = item, Text = item });
            }
            return new SelectList(lstAllEnvironmentName, "Value", "Text");
        }
        public static SelectList GetImportDelimiter()
        {
            string[] DelimeterValues = ConfigurationManager.AppSettings["ImportDelimiter"].ToString().Split(',');
            List<SelectListItem> lstAllEnvironmentName = new List<SelectListItem>();
            foreach (var item in DelimeterValues)
            {
                lstAllEnvironmentName.Add(new SelectListItem { Value = item, Text = item });
            }
            return new SelectList(lstAllEnvironmentName, "Value", "Text");
        }
        public static SelectList GetImportType()
        {
            List<SelectListItem> lstImportType = new List<SelectListItem>();
            if (Helper.LicenseEnabledDNB)
            {
                lstImportType.Add(new SelectListItem { Value = "Data Import", Text = CommonMessagesLang.lblCleanseMatch });
                lstImportType.Add(new SelectListItem { Value = "Match Refresh", Text = CommonMessagesLang.lblEnrichment });
            }
            if (Helper.LicenseEnabledOrb)
            {
                lstImportType.Add(new SelectListItem { Value = "Orb Data Import", Text = CommonMessagesLang.lblOrbCleanseMatch });
                lstImportType.Add(new SelectListItem { Value = "Orb Match Refresh", Text = CommonMessagesLang.lblOrbEnrichment });//added orb enrichment only dropdown in upload configuration
            }
            return new SelectList(lstImportType, "Value", "Text");
        }

        #endregion

        #region OI Export Data
        public static SelectList GetCompanyTree()
        {
            List<SelectListItem> lstCompanyTree = new List<SelectListItem>();

            lstCompanyTree.Add(new SelectListItem { Value = "MatchOutput", Text = CommonMessagesLang.lblMatchOutput });
            lstCompanyTree.Add(new SelectListItem { Value = "Enrichment", Text = CommonMessagesLang.lblFirmographics });

            //If IsEnableCorporateTreeEnrichment is true then only display CompanyTree in dropdown
            if (Helper.IsEnableCorporateTreeEnrichment)
            {
                lstCompanyTree.Add(new SelectListItem { Value = "CompanyTree", Text = CommonMessagesLang.lblCompanyTree });
            }
            lstCompanyTree.Add(new SelectListItem { Value = "ActiveDataQueue", Text = CommonMessagesLang.lblActiveDataQueue });

            return new SelectList(lstCompanyTree, "Value", "Text");
        }
        #endregion

        #region OI Build A List
        // Get employees details
        public static SelectList GetEmployees()
        {
            List<SelectListItem> lstEmployees = new List<SelectListItem>();

            lstEmployees.Add(new SelectListItem { Value = "any", Text = "any" });
            lstEmployees.Add(new SelectListItem { Value = "1-10", Text = "1-10" });
            lstEmployees.Add(new SelectListItem { Value = "10-50", Text = "10-50" });
            lstEmployees.Add(new SelectListItem { Value = "50-200", Text = "50-200" });
            lstEmployees.Add(new SelectListItem { Value = "200-500", Text = "200-500" });
            lstEmployees.Add(new SelectListItem { Value = "500-1k", Text = "500-1k" });
            lstEmployees.Add(new SelectListItem { Value = "1k-5k", Text = "1k-5k" });
            lstEmployees.Add(new SelectListItem { Value = "5k-10k", Text = "5k-10k" });
            lstEmployees.Add(new SelectListItem { Value = "10k", Text = "10k" });
            return new SelectList(lstEmployees, "Value", "Text");
        }
        // Get revenue details
        public static SelectList GetRevenue()
        {
            List<SelectListItem> lstRevenue = new List<SelectListItem>();

            lstRevenue.Add(new SelectListItem { Value = "any", Text = "any" });
            lstRevenue.Add(new SelectListItem { Value = "0-1m", Text = "0-1m" });
            lstRevenue.Add(new SelectListItem { Value = "1m-10m", Text = "1m-10m" });
            lstRevenue.Add(new SelectListItem { Value = "10m-50m", Text = "10m-50m" });
            lstRevenue.Add(new SelectListItem { Value = "50m-100m", Text = "50m-100m" });
            lstRevenue.Add(new SelectListItem { Value = "100m-200m", Text = "100m-200m" });
            lstRevenue.Add(new SelectListItem { Value = "200m-1b", Text = "200m-1b" });
            lstRevenue.Add(new SelectListItem { Value = "1b", Text = "1b" });
            return new SelectList(lstRevenue, "Value", "Text");
        }
        // Fetching the response from API
        public static SelectList GetBuildListData(string fieldsType)
        {
            string APIKey = Helper.OIAPIKey;
            APIKey = APIKey.Replace("Bearer ", "");
            string endpoint = ConfigurationManager.AppSettings["OIBuildAListDictionariesUrl"] + fieldsType + "?api_key=" + APIKey;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var result = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                var serializer = new JavaScriptSerializer();
            }
            List<SelectListItem> lstResponse = new List<SelectListItem>();

            if (fieldsType == "industries" || fieldsType == "rankings")
            {
                var objResponse = JsonConvert.DeserializeObject<List<Namelst>>(result);
                foreach (var item in objResponse)
                {
                    lstResponse.Add(new SelectListItem { Value = item.name.ToString(), Text = item.name.ToString() });
                }
            }
            else if (fieldsType == "stock_exchanges")
            {
                var objResponse = JsonConvert.DeserializeObject<List<StockExchange>>(result);
                foreach (var item in objResponse)
                {
                    lstResponse.Add(new SelectListItem { Value = item.exchange.ToString(), Text = item.exchange.ToString() + " " + item.description.ToString() });
                }
            }
            else if (fieldsType == "technologies")
            {
                var objResponse = JsonConvert.DeserializeObject<List<Technologies>>(result);
                foreach (var item in objResponse)
                {
                    lstResponse.Add(new SelectListItem { Value = item.name.ToString(), Text = item.name.ToString() + " " + item.category.ToString() });
                }
            }
            else if (fieldsType == "technologies/categories")
            {
                var objResponse = JsonConvert.DeserializeObject<List<Tech_Categories>>(result);
                foreach (var item in objResponse)
                {
                    lstResponse.Add(new SelectListItem { Value = item.name.ToString(), Text = item.name.ToString() });
                }
            }
            else if (fieldsType == "naics_codes" || fieldsType == "sic_codes")
            {
                var objResponse = JsonConvert.DeserializeObject<List<Codes>>(result);
                foreach (var item in objResponse)
                {
                    lstResponse.Add(new SelectListItem { Value = item.code.ToString(), Text = item.code.ToString() + " " + item.description.ToString() });
                }
            }

            else if (fieldsType == "categories")
            {
                var objResponse = JsonConvert.DeserializeObject<List<Categories>>(result);
                foreach (var item in objResponse)
                {
                    lstResponse.Add(new SelectListItem { Value = item.canonical.ToString(), Text = item.canonical.ToString() });
                }
            }

            return new SelectList(lstResponse, "Value", "Text");
        }
        #endregion
        #region "Monitoring Export Data"
        public static SelectList GetExportApi(string ConncetionString, string ExportCategory)
        {
            // Get All tags type code from the database and fill the dropdown 
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            SettingFacade fac = new SettingFacade(ConncetionString);
            DataTable dt = fac.GetExportTablesApiName(ExportCategory);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAllFilter.Add(new SelectListItem { Value = dt.Rows[i]["APIName"].ToString(), Text = dt.Rows[i]["APIName"].ToString() });
            }
            return new SelectList(lstAllFilter, "Value", "Text");
        }
        #endregion

        public static SelectList GetAcceptedBy(string ConnectionString)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            DataTable dt = fac.GetAcceptedBy();
            List<SelectListItem> lstAcceptedBy = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAcceptedBy.Add(new SelectListItem { Value = dt.Rows[i]["AcceptedBy"].ToString(), Text = dt.Rows[i]["AcceptedBy"].ToString() });
            }
            return new SelectList(lstAcceptedBy, "Value", "Text");
        }

        public static SelectList GetTrueFalse()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "", Text = "Select Value" });
            lstAllFilter.Add(new SelectListItem { Value = "True", Text = "True" });
            lstAllFilter.Add(new SelectListItem { Value = "False", Text = "False" });
            return new SelectList(lstAllFilter, "Value", "Text");
        }
        public static SelectList GetORBDataImportDuplicateResolution()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "NONE", Text = "NONE" });
            lstAllFilter.Add(new SelectListItem { Value = "PURGE_IN_ACTIVE_QUEUE", Text = "PURGE_IN_ACTIVE_QUEUE" });
            lstAllFilter.Add(new SelectListItem { Value = "PURGE_IN_ACTIVE_AND_EXPORT_QUEUE", Text = "PURGE_IN_ACTIVE_AND_EXPORT_QUEUE" });
            lstAllFilter.Add(new SelectListItem { Value = "DONT_IMPORT_DUPLICATES", Text = "DONT_IMPORT_DUPLICATES" });
            return new SelectList(lstAllFilter, "Value", "Text");
        }

        public static SelectList GetOIMatchGrade()
        {
            List<SelectListItem> lstMatchGrade = new List<SelectListItem>();
            lstMatchGrade.Add(new SelectListItem { Value = "#", Text = CommonMessagesLang.lblAny });
            lstMatchGrade.Add(new SelectListItem { Value = "A", Text = CommonMessagesLang.lblSame });
            lstMatchGrade.Add(new SelectListItem { Value = "B", Text = CommonMessagesLang.lblSimilar });
            lstMatchGrade.Add(new SelectListItem { Value = "F", Text = CommonMessagesLang.lblDifferent }); ;
            lstMatchGrade.Add(new SelectListItem { Value = "X", Text = CommonMessagesLang.lblNoData });
            lstMatchGrade.Add(new SelectListItem { Value = "Z", Text = CommonMessagesLang.lblMissingInput });
            return new SelectList(lstMatchGrade, "Value", "Text");
        }

        public static SelectList GetOIlimitedMatchGrade()
        {
            List<SelectListItem> lstMatchGrade = new List<SelectListItem>();
            lstMatchGrade.Add(new SelectListItem { Value = "#", Text = CommonMessagesLang.lblAny });
            lstMatchGrade.Add(new SelectListItem { Value = "A", Text = CommonMessagesLang.lblSame });
            lstMatchGrade.Add(new SelectListItem { Value = "F", Text = CommonMessagesLang.lblDifferent });
            lstMatchGrade.Add(new SelectListItem { Value = "X", Text = CommonMessagesLang.lblNoData });
            lstMatchGrade.Add(new SelectListItem { Value = "Z", Text = CommonMessagesLang.lblMissingInput });
            return new SelectList(lstMatchGrade, "Value", "Text");
        }
        public static SelectList GetOIMatchCode()
        {
            List<SelectListItem> lstMatchGrade = new List<SelectListItem>();
            lstMatchGrade.Add(new SelectListItem { Value = "00", Text = CommonMessagesLang.lblBusinessName });
            lstMatchGrade.Add(new SelectListItem { Value = "01", Text = CommonMessagesLang.lblOtherName });
            return new SelectList(lstMatchGrade, "Value", "Text");
        }
        public static SelectList GetOIWebDomainMatchCode()
        {
            List<SelectListItem> lstWebDomainMatchCode = new List<SelectListItem>();
            lstWebDomainMatchCode.Add(new SelectListItem { Value = "00", Text = "00" });
            lstWebDomainMatchCode.Add(new SelectListItem { Value = "01", Text = "01" });
            return new SelectList(lstWebDomainMatchCode, "Value", "Text");
        }
        public static SelectList GetTypesOfProviders()
        {
            List<SelectListItem> lstWebDomainMatchCode = new List<SelectListItem>();
            lstWebDomainMatchCode.Add(new SelectListItem { Value = "DandB", Text = CommonMessagesLang.lblDAndB });
            lstWebDomainMatchCode.Add(new SelectListItem { Value = "Orb", Text = CommonMessagesLang.lblOrbIntelligence });
            return new SelectList(lstWebDomainMatchCode, "Value", "Text");
        }



        public static SelectList GetDnBCommandOutputFormat()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "DownloadMatchOutput", Text = "Match Output" });
            lstAllFilter.Add(new SelectListItem { Value = "DownloadEnrichmentOutput", Text = "Enrichment Output" });
            lstAllFilter.Add(new SelectListItem { Value = "DownloadTransferDUNS", Text = "Transferred Duns" });
            lstAllFilter.Add(new SelectListItem { Value = "DownloadActiveDataQueue", Text = "Active Data Queue" });
            lstAllFilter.Add(new SelectListItem { Value = "DownloadMonitoringUpdates", Text = "Monitoring Notification" });
            return new SelectList(lstAllFilter, "Value", "Text");
        }
        public static SelectList GetOrBCommandOutputFormat()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "DownloadMatchOutput", Text = "Match Output" });
            lstAllFilter.Add(new SelectListItem { Value = "DownloadEnrichmentOutput", Text = "Firmop Output" });
            lstAllFilter.Add(new SelectListItem { Value = "DownloadCompanyTree", Text = "Company Tree" });
            return new SelectList(lstAllFilter, "Value", "Text");
        }
        // Tune import process to handle bad data import(MP-681)
        public static SelectList getDataImportErrorResolution()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "FAIL_IMPORT_ON_ERROR", Text = DandBSettingLang.lblFailImportOnError });
            lstAllFilter.Add(new SelectListItem { Value = "REJECT_ERROR_ROWS", Text = DandBSettingLang.lblRejectErrorRows });
            return new SelectList(lstAllFilter, "Value", "Text");
        }

        public static SelectList GetThirdPartyAPICredentials(string ConnectionString, string thirdPartyProvider)
        {
            List<ThirdPartyAPICredentialsEntity> lstThirdPartyAPICredentials = new List<ThirdPartyAPICredentialsEntity>();
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(ConnectionString);
            lstThirdPartyAPICredentials = fac.GetThirdPartyAPICredentials(thirdPartyProvider);
            List<SelectListItem> lstAPICredentials = new List<SelectListItem>();
            foreach (var item in lstThirdPartyAPICredentials)
            {
                lstAPICredentials.Add(new SelectListItem { Value = item.CredentialId.ToString(), Text = item.CredentialName.ToString() });
            }
            return new SelectList(lstAPICredentials, "Value", "Text");
        }
        public static SelectList GetThirdPartyAPICredentialsForEnrichment(string ConnectionString, string thirdPartyProvider)
        {
            List<ThirdPartyAPIForEnrichmentEntity> lstThirdPartyAPICredentials = new List<ThirdPartyAPIForEnrichmentEntity>();
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(ConnectionString);
            lstThirdPartyAPICredentials = fac.GetThirdPartyAPICredentialsForEhrichment(thirdPartyProvider);
            List<SelectListItem> lstAPICredentials = new List<SelectListItem>();
            foreach (var item in lstThirdPartyAPICredentials)
            {
                lstAPICredentials.Add(new SelectListItem { Value = item.CredentialId.ToString() + "@#$" + item.APIType, Text = item.CredentialName.ToString() });
            }
            return new SelectList(lstAPICredentials, "Value", "Text");
        }
        public static SelectList GetAPITypeForThirdPartyAPICredentialsForEnrichment(string ConnectionString, string APIFamily)
        {
            List<ThirdPartyAPIForEnrichmentEntity> lstThirdPartyAPICredentials = new List<ThirdPartyAPIForEnrichmentEntity>();
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(ConnectionString);
            lstThirdPartyAPICredentials = fac.GetAPITypeForUXDefaultUXEnrichment();
            List<SelectListItem> lstAPICredentials = new List<SelectListItem>();
            if (APIFamily == "DirectPlus")
            {
                lstThirdPartyAPICredentials = lstThirdPartyAPICredentials.Where(x => x.APIFamily == "DirectPlus").GroupBy(x => x.DnBAPIId).Select(g => g.First()).ToList();
            }
            else if (APIFamily == "Direct20")
            {
                lstThirdPartyAPICredentials = lstThirdPartyAPICredentials.Where(x => x.APIFamily == "Direct20").GroupBy(x => x.DnBAPIId).Select(g => g.First()).ToList();
            }
            else if (APIFamily == "")
            {
                lstThirdPartyAPICredentials = lstThirdPartyAPICredentials.Where(x => x.APIFamily == "").GroupBy(x => x.DnBAPIId).Select(g => g.First()).ToList();
            }
            foreach (var item in lstThirdPartyAPICredentials)
            {
                lstAPICredentials.Add(new SelectListItem { Value = item.DnBAPIId.ToString(), Text = item.APIType.ToString() + "-" + item.DnBAPIName.ToString() });
            }
            return new SelectList(lstAPICredentials, "Value", "Text");
        }
        #region "OI Match data"
        public static SelectList GetFiltersMatchvsNoCandidate()
        {
            List<SelectListItem> lstFilterMatch = new List<SelectListItem>();
            lstFilterMatch.Add(new SelectListItem { Value = "Both", Text = "Both" });
            lstFilterMatch.Add(new SelectListItem { Value = "IncludeWithCandidates", Text = "Matches Candidates" });
            lstFilterMatch.Add(new SelectListItem { Value = "IncludeWithoutCandidates", Text = "No Candidate" });
            return new SelectList(lstFilterMatch, "Value", "Text");
        }
        #endregion

        public static List<CVRefEntity> GetCVRefTypeByTypeCode(int typeCode, string ConnectionString)
        {
            List<CVRefEntity> refEntities = new List<CVRefEntity>();
            CVRefFacade fac = new CVRefFacade(ConnectionString);
            refEntities = fac.GetAPItype(typeCode);
            if (typeCode == (int)CVRefTypeCode.THIRD_PARTY_PROVIDER)
                refEntities.RemoveAll(x => x.Value == "DNB" || x.Value == "ORB");
            return refEntities;
        }

        public static List<CVRefEntity> GetThirdPartyProviders(int fullList, string ConnectionString)
        {
            List<CVRefEntity> refEntities = new List<CVRefEntity>();
            CVRefFacade fac = new CVRefFacade(ConnectionString);
            refEntities = fac.GetThirdPartyProviders(fullList);
            refEntities.RemoveAll(x => x.Value == "DNB" || x.Value == "ORB");
            refEntities.ForEach(x => x.DDValue = x.Value + "::" + x.Code);
            return refEntities;
        }

        public List<CVRefEntity> GetCVRefTypeByTypeCodeAutoAcceptance(int typeCode, string ConnectionString)
        {
            List<CVRefEntity> refEntities = new List<CVRefEntity>();
            CVRefFacade fac = new CVRefFacade(ConnectionString);
            refEntities = fac.GetAPItype(typeCode);
            if (typeCode == (int)CVRefTypeCode.THIRD_PARTY_PROVIDER)
                refEntities.RemoveAll(x => x.Value == "DNB" || x.Value == "ORB");
            return refEntities;
        }


        public static List<DnbAPIEntity> GetDnBAPIList(string ConnectionString, string APIType = null)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            return fac.GetDnBAPIList(APIType);
        }
        public static SelectList GetCleanseMatchAPItype()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "", Text = "--select--" });
            lstAllFilter.Add(new SelectListItem { Value = "DP", Text = "DirectPlus" });
            lstAllFilter.Add(new SelectListItem { Value = "D2", Text = "Direct20" });
            return new SelectList(lstAllFilter, "Value", "Text");
        }

        public static SelectList GetLicensedAPIType(string ConnectionString)
        {
            List<SelectListItem> lstAPITypes = new List<SelectListItem>();
            SettingFacade fac = new SettingFacade(ConnectionString);
            DataTable dt = fac.GetLicensedAPIType();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAPITypes.Add(new SelectListItem { Value = dt.Rows[i]["APIType"].ToString(), Text = dt.Rows[i]["APIType"].ToString() });
            }
            return new SelectList(lstAPITypes, "Value", "Text");
        }


        public static List<ThirdPartyAPICredentialsEntity> GetCredentials(string connectionString, string thirdPartyProvider, string APIType)
        {
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(connectionString);
            List<ThirdPartyAPICredentialsEntity> lst = fac.GetThirdPartyAPICredentials(thirdPartyProvider);
            if (APIType == "DirectPlus")
            {
                lst = lst.Where(x => x.APIType == "DirectPlus").GroupBy(x => x.CredentialName).Select(g => g.First()).ToList();
            }
            else if (APIType == "Direct20")
            {
                lst = lst.Where(x => x.APIType == "Direct20").GroupBy(x => x.CredentialName).Select(g => g.First()).ToList();
            }
            lst.Insert(0, new ThirdPartyAPICredentialsEntity
            {
                CredentialName = "--Select--"
            });
            return lst;
        }

        #region User Delete
        public static SelectList GetAllActiveUsers(string ConnectionString, int UserId)
        {
            List<UsersEntity> lstUsers = new List<UsersEntity>();
            SettingFacade fac = new SettingFacade(ConnectionString);
            lstUsers = fac.GetUsersList();
            List<SelectListItem> lstGetAllUser = new List<SelectListItem>();
            lstUsers = lstUsers.Where(x => x.UserStatusCode.ToString() == "101001" && x.UserId != UserId).GroupBy(x => x.UserName).Select(g => g.First()).ToList();
            foreach (var item in lstUsers)
            {
                lstGetAllUser.Add(new SelectListItem { Value = item.UserId.ToString(), Text = item.UserName.ToString() });
            }
            return new SelectList(lstGetAllUser, "Value", "Text");
        }
        #endregion
        public static SelectList GetStatusType()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "ALL", Text = "ALL" });
            lstAllFilter.Add(new SelectListItem { Value = "FAILED", Text = "FAILED" });
            lstAllFilter.Add(new SelectListItem { Value = "SUCCESS", Text = "SUCCESS" });
            lstAllFilter.Add(new SelectListItem { Value = "RUNNING", Text = "RUNNING" });
            return new SelectList(lstAllFilter, "Value", "Text");
        }
        public static SelectList GetETLType()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "CLEANSE_MATCH", Text = "CLEANSE_MATCH" });
            lstAllFilter.Add(new SelectListItem { Value = "ENRICHMENT", Text = "ENRICHMENT" });
            lstAllFilter.Add(new SelectListItem { Value = "CUSTOM", Text = "CUSTOM" });
            lstAllFilter.Add(new SelectListItem { Value = "IMPORT", Text = "IMPORT" });
            lstAllFilter.Add(new SelectListItem { Value = "EXPORT", Text = "EXPORT" });
            return new SelectList(lstAllFilter, "Value", "Text");
        }
        public static SelectList GetDurationHours()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "3", Text = "last 3 hours" });
            lstAllFilter.Add(new SelectListItem { Value = "6", Text = "last 6 hours" });
            lstAllFilter.Add(new SelectListItem { Value = "12", Text = "last 12 hours" });
            lstAllFilter.Add(new SelectListItem { Value = "24", Text = "last 24 hours" });
            return new SelectList(lstAllFilter, "Value", "Text");
        }
        public static SelectList GetTradeUps()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "", Text = "None" });
            lstAllFilter.Add(new SelectListItem { Value = "hq", Text = "hq" });
            lstAllFilter.Add(new SelectListItem { Value = "domhq", Text = "domhq" });
            return new SelectList(lstAllFilter, "Value", "Text");
        }
        // For filling up the values of ResearchSubTypes in targeted iresearch investigation
        public static SelectList GetResearchSubTypesForInvestigation(string ConnectionString)
        {
            List<iResearchEntityTargetedEntity> lstiResearchEntity = new List<iResearchEntityTargetedEntity>();
            iResearchFacade fac = new iResearchFacade(ConnectionString);
            lstiResearchEntity = fac.GetDnBReferenceDataBycategoryID(760);
            List<SelectListItem> lstResearchInvestigation = new List<SelectListItem>();
            foreach (var item in lstiResearchEntity)
            {
                lstResearchInvestigation.Add(new SelectListItem { Value = item.code.ToString(), Text = item.code.ToString() + "-" + item.description.ToString() });
            }
            return new SelectList(lstResearchInvestigation, "Value", "Text");
        }

        public static SelectList GetiResearchMarketApplicability(string MarketApplicability, string ConnectionString,int fullList = 0)
        {
            List<SelectListItem> lstResearchInvestigation = new List<SelectListItem>();
            iResearchFacade fac = new iResearchFacade(ConnectionString);
            DataTable dt = fac.GetiResearchMarketApplicability(MarketApplicability,fullList);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstResearchInvestigation.Add(new SelectListItem { Value = dt.Rows[i]["SubTypeCode"].ToString(), Text = dt.Rows[i]["SubTypeCode"].ToString() + "-" + dt.Rows[i]["ResearchSubType"].ToString() });
            }
            return new SelectList(lstResearchInvestigation, "Value", "Text");
        }

        public static SelectList GetiResearchInvestigationCaseLookup(string ConnectionString)
        {
            List<SelectListItem> lstAllStatus = new List<SelectListItem>();
            iResearchFacade fac = new iResearchFacade(ConnectionString);
            DataTable dataTable = fac.GetiResearchInvestigationCaseLookup();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                lstAllStatus.Add(new SelectListItem { Value = dataTable.Rows[i]["CaseStatus"].ToString(), Text = dataTable.Rows[i]["CaseStatus"].ToString() });
            }
            return new SelectList(lstAllStatus, "Value", "Text");
        }

        public static SelectList GetFieldsForThirdPartyEnrichment(string ConnectionString, string providerCode = "0")
        {
            int code = 0;
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            CVRefFacade fac = new CVRefFacade(ConnectionString);
            try
            {
                code = Convert.ToInt32(providerCode);
            }
            catch (Exception)
            {
                code = 0;
            }
            DataTable dt = fac.GetThirdPartyProviderEnrichments(code);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAllFilter.Add(new SelectListItem { Value = dt.Rows[i]["Description"].ToString(), Text = dt.Rows[i]["Description"].ToString() });
            }
            return new SelectList(lstAllFilter, "Value", "Text");
        }

        public static SelectList GetProviderLookups(int providerCode,string connection)
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            MultiPassFacade mFac = new MultiPassFacade(connection);
            DataTable dt = mFac.GetProviderLookups(providerCode);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAllFilter.Add(new SelectListItem { Value = dt.Rows[i]["LookupName"].ToString(), Text = dt.Rows[i]["LookupName"].ToString() });
            }
            return new SelectList(lstAllFilter, "Value", "Text");
        }

        public static SelectList GetTagsForMPM(int providerCode, string connection)
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            MultiPassFacade mFac = new MultiPassFacade(connection);
            DataTable dt = mFac.GetTagsForMPM(providerCode);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAllFilter.Add(new SelectListItem { Value = dt.Rows[i]["Tag"].ToString(), Text = dt.Rows[i]["Tag"].ToString() });
            }
            return new SelectList(lstAllFilter, "Value", "Text");
        }
    }
    public enum UserRole
    {
        [Description("GLOBAL")]
        GLOBAL,
        [Description("LOB")]
        LOB
    }
    public enum ImportProcess
    {
        [Description("SessionFilter")]
        SessionFilter,
        [Description("RejectData")]
        RejectData,
        [Description("CleanReMatchData")]
        CleanReMatchData,
        [Description("ExportData")]
        ExportData,
        [Description("UnprocessedInputRecord")]
        UnprocessedInputRecord
    }
    public enum ExternalDataSources
    {
        [Description("Azure Storage")]
        AZURE = 1,
        [Description("FTP")]
        FTP = 2,
        [Description("SFTP")]
        SFTP = 3,
        [Description("AWS S3")]
        AWS = 4,

    }
}