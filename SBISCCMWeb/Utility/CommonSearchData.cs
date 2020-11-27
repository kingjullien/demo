using Microsoft.Exchange.WebServices.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility.IdentityResolution;
using SBISCCMWeb.Utility.SearchByDomain;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace SBISCCMWeb.Utility
{
    public class CommonSearchData
    {
        #region "Variable declaration"     
        APIResponse response;
        #endregion

        #region "Load Data on Search"
        public MainMatchEntity LoadData(string CompanyName, string Address, string Address2, string City, string State, string Country, string zip, string Phone, bool ExcludeNonHeadQuarters, bool ExcludeNonMarketable, bool ExcludeOutofBusiness, bool ExcludeUndeliverable, bool ExcludeUnreachable, string Language, string SrcRecId, string ConnectionString, string InputId)
        {
            MainMatchEntity objmainMatchEntity = new MainMatchEntity();
            try
            {
                // Checking APIType is there or not
                string APItype = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.APIType.ToString());
                if (string.IsNullOrEmpty(APItype))
                {
                    objmainMatchEntity.ResponseErroeMessage = CommonMessagesLang.msgNoDefaultKeyForSearch;
                    return objmainMatchEntity;
                }
                bool IsDirectPlusCall = false;
                CommonMethod objCommon = new CommonMethod();
                Utility api = new Utility();
                var objResult = objCommon.LoadCleanseMatchSettings(ConnectionString);

                bool IsGlobal = true;
                string LOBTag = null;
                int PageSize = 10, PageNumber = 1, SortOrder = 0, TotalRecords = 0;
                int totalCount = 0;
                DataTable lstThirdPartyAPICredentials = new DataTable();
                ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(ConnectionString);
                lstThirdPartyAPICredentials = fac.GetMinConfidenceSettingsListPaging(IsGlobal, LOBTag);
                int candidateMaxQuantity = 0, confidenceLowerLevelThresholdValue = 0;
                foreach (DataRow row in lstThirdPartyAPICredentials.Rows)
                {
                    candidateMaxQuantity = Convert.ToInt32(row["MaxCandidateQty"]);
                    confidenceLowerLevelThresholdValue = Convert.ToInt32(row["MinConfidenceCode"]);
                }

                CountryGroupModel.CountryISOAlpha2Enum objEnum = new CountryGroupModel.CountryISOAlpha2Enum();
                try
                {
                    objEnum = (CountryGroupModel.CountryISOAlpha2Enum)Enum.Parse(typeof(CountryGroupModel.CountryISOAlpha2Enum), Convert.ToString(Country));
                }
                catch
                {
                    objEnum = (CountryGroupModel.CountryISOAlpha2Enum)Enum.Parse(typeof(CountryGroupModel.CountryISOAlpha2Enum), "US");
                }
                try
                {
                    CompanyFacade fcd = new CompanyFacade(ConnectionString, Helper.oUser.UserName);
                    if (APItype.ToLower() == ApiLayerType.Directplus.ToString().ToLower())
                    {
                        IsDirectPlusCall = true;
                        response = api.GetCleanseMatchResultDirectPlus(CompanyName, Address, Address2, City, State, objEnum, zip, Phone, ExcludeNonHeadQuarters, ExcludeNonMarketable, ExcludeOutofBusiness, ExcludeUndeliverable, ExcludeUnreachable, Language, candidateMaxQuantity, confidenceLowerLevelThresholdValue, ConnectionString, InputId);
                    }
                    else if (APItype.ToLower() == ApiLayerType.Direct20.ToString().ToLower())
                    {
                        response = api.GetCleanseMatchResult(CompanyName, Address, Address2, City, State, objEnum, zip, Phone, ExcludeNonHeadQuarters, ExcludeNonMarketable, ExcludeOutofBusiness, ExcludeUndeliverable, ExcludeUnreachable, candidateMaxQuantity, confidenceLowerLevelThresholdValue, ConnectionString, InputId);
                    }

                    if (response != null && response.TransactionResponseDetail != null)
                    {
                        objCommon.InsertAPILogs(response.TransactionResponseDetail, ConnectionString);
                        fcd.InsertCleanseMatchCallResults(SrcRecId, response.ResponseJSON, response.APIRequest, Helper.oUser.UserId, InputId);
                        objmainMatchEntity.lstMatches = response.MatchEntities;
                        objmainMatchEntity.lstMatches.ForEach(x => x.MatchDataCriteriaText = response?.TransactionResponseDetail?.MatchDataCriteriaText);
                    }
                    else
                    {
                        objmainMatchEntity.ResponseErroeMessage = CommonMessagesLang.msgResponseErroeMessage;
                    }
                    if (!string.IsNullOrEmpty(response?.ResponseJSON))
                    {
                        dynamic data = JObject.Parse(response.ResponseJSON);
                        if (data.error != null && !string.IsNullOrEmpty(data.error.errorMessage.Value))
                        {
                            objmainMatchEntity.ResponseErroeMessage = data.error.errorMessage.Value;
                        }
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
                            if (IsDirectPlusCall == false)
                            {
                                GetCleanseMatchResponseMain objResponse = serializer.Deserialize<GetCleanseMatchResponseMain>(result);
                                if (objResponse != null && objResponse.GetCleanseMatchResponse != null && objResponse.GetCleanseMatchResponse.TransactionResult != null)
                                {
                                    objmainMatchEntity.ResponseErroeMessage = objResponse.GetCleanseMatchResponse.TransactionResult.ResultText;
                                    //objCommon.InsertAPILogs(ConnectionString, null, null, objResponse.GetCleanseMatchResponse.TransactionResult.SeverityText, objResponse.GetCleanseMatchResponse.TransactionResult.ResultID, objResponse.GetCleanseMatchResponse.TransactionResult.ResultText, null, 0, null);
                                }
                            }
                            else
                            {
                                var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                                IdentityResolutionResponse objResponse = JsonConvert.DeserializeObject<IdentityResolutionResponse>(result, settings);
                                if (objResponse != null)
                                {
                                    objmainMatchEntity.ResponseErroeMessage = objResponse.error.errorMessage;
                                    objCommon.InsertAPILogs(ConnectionString, objResponse.transactionDetail.transactionID, Convert.ToDateTime(objResponse.transactionDetail.transactionTimestamp), null, null, objResponse.error.errorMessage, null, 0, null);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objmainMatchEntity.ResponseErroeMessage = CommonMessagesLang.msgResponseErroeMessage;
            }
            finally
            {
            }
            return objmainMatchEntity;
        }
        #endregion
        #region Search By DUNS
        public APIResponse SearchByDUNS(string DUNSNO, string ConnectionString)
        {

            // Checking APIType is there or not
            string APItype = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_SINGLE_ENTITY_SEARCH.ToString(), ThirdPartyProperties.APIType.ToString());
            if (string.IsNullOrEmpty(APItype))
            {
                Helper.ResponseErroeMessage = CommonMessagesLang.msgNoDefaultKeyForSearch;
            }
            else
            {
                Helper.ResponseErroeMessage = null;
            }
            Helper.IsSearchBYDUNS = true;
            CommonMethod objCommon = new CommonMethod();
            Utility api = new Utility();
            var objResult = objCommon.LoadCleanseMatchSettings(ConnectionString);

            bool IsGlobal = true;
            string LOBTag = null;
            int PageSize = 10, PageNumber = 1, SortOrder = 0, TotalRecords = 0;
            int totalCount = 0;
            DataTable lstThirdPartyAPICredentials = new DataTable();
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(ConnectionString);
            lstThirdPartyAPICredentials = fac.GetMinConfidenceSettingsListPaging(IsGlobal, LOBTag);
            int candidateMaxQuantity = 0, confidenceLowerLevelThresholdValue = 0;
            foreach (DataRow row in lstThirdPartyAPICredentials.Rows)
            {
                confidenceLowerLevelThresholdValue = Convert.ToInt32(row["MinConfidenceCode"]);
            }

            // if Country is not specified than by default pass United state as a default company.
            bool IsDirectPlusCall = false;
            try
            {
                if (APItype.ToLower() == ApiLayerType.Directplus.ToString().ToLower())
                {
                    IsDirectPlusCall = true;
                    response = api.GetMatchResultDirectPlus(DUNSNO.ToString().Trim(), ConnectionString, confidenceLowerLevelThresholdValue);
                }
                else if (APItype.ToLower() == ApiLayerType.Direct20.ToString().ToLower())
                {
                    response = api.GetMatchResult(DUNSNO.ToString().Trim(), ConnectionString, confidenceLowerLevelThresholdValue);
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

                        if (IsDirectPlusCall == false)
                        {
                            MatchResponseMain objResponse = serializer.Deserialize<MatchResponseMain>(result);
                            if (objResponse != null && objResponse.MatchResponse != null && objResponse.MatchResponse.TransactionResult != null)
                            {
                                Helper.ResponseErroeMessage = objResponse.MatchResponse.TransactionResult.ResultText;
                                objCommon.InsertAPILogs(ConnectionString, null, null, objResponse.MatchResponse.TransactionResult.SeverityText, objResponse.MatchResponse.TransactionResult.ResultID, objResponse.MatchResponse.TransactionResult.ResultText, null, 0, null);
                            }
                        }
                        else
                        {
                            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                            IdentityResolutionResponse objResponse = JsonConvert.DeserializeObject<IdentityResolutionResponse>(result, settings);
                            if (objResponse != null)
                            {
                                Helper.ResponseErroeMessage = objResponse.error?.errorMessage;
                                objCommon.InsertAPILogs(ConnectionString, objResponse.transactionDetail.transactionID, Convert.ToDateTime(objResponse.transactionDetail.transactionTimestamp), null, null, objResponse.error.errorMessage, null, 0, null);
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(Helper.ResponseErroeMessage))
                {
                    Helper.ResponseErroeMessage = "No match found.";
                }
            }
            return response;
        }


        #endregion

        #region Type Ahead Functionality Implementation
        public string LoadTypeAheadData(string searchTerm, string countryISOAlpha2Code)
        {
            Utility api = new Utility();
            string JSONResponse = api.SearchDataTypeAheadAPI(searchTerm, countryISOAlpha2Code);
            return JSONResponse;
        }
        #endregion
    }
}