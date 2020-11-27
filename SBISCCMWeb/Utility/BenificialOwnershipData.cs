using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SBISCCMWeb.Models.BeneficialOwnership;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Utility
{
    public class BenificialOwnershipData
    {
        public string GetBenificialOwnershipData(string EnrichmentURL, string AuthToken)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string endpoint = EnrichmentURL;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", AuthToken);

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    string objResponse = result;
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    string objResponse = result;
                    return objResponse;
                }
            }
            return null;
        }

        public ScreeningResponse ScreeningMultiPleData(BeneficialOwnership_Main beneficial,int memberId)
        {
            ScreeningResponse screeningResponse = new ScreeningResponse();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string endpoint = "https://rpstest.visualcompliance.com/RPS/RPSService.svc/SearchEntity";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Host = "rpstest.visualcompliance.com";
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip,deflate");

            ScreeningRequest screeningRequest = new ScreeningRequest();
            screeningRequest.__type = "searchrequest:http://eim.visualcompliance.com/RPSService/2016/11";
            screeningRequest.sguid = "";
            screeningRequest.stransid = "";
            screeningRequest.ssecno = "XA65D";
            screeningRequest.spassword = "645LMQ847";
            screeningRequest.smodes = "";
            screeningRequest.srpsgroupbypass = "";

            screeningRequest.searches = new List<Search>();
            foreach (var item in memberId > 0 ? beneficial.lstCombinedData.Where(x => x.memberID == memberId) : beneficial.lstCombinedData)
            {
                Search search = new Search();
                search.__type = "search:http://eim.visualcompliance.com/RPSService/2016/11";
                search.soptionalid = Convert.ToString(item.memberID);
                if(item.beneficiaryType.ToLower() == "business")
                {
                    search.sname = "";
                    search.scompany = item.name.Replace("*","").TrimStart();
                }
                else
                {
                    search.sname = item.name.Replace("*", "").TrimStart();
                    search.scompany = "";
                }
                search.saddress1 = item.addressStreetLine1;
                search.saddress2 = item.addressStreetLine2;
                search.saddress3 = item.addressStreetLine3;
                search.scity = item.addressCity;
                search.sstate = item.addressState;
                search.szip = item.addressPostalCode;
                search.scountry = item.addressCountryIsoAlpha2Code;
                search.selective1 = Convert.ToString(item.memberID);
                search.selective2 = item.beneficiaryType;
                screeningRequest.searches.Add(search);
            }


            string json = string.Empty;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                json = JsonConvert.SerializeObject(screeningRequest,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    JObject jsonResult = JObject.Parse(result);
                    if(jsonResult.ContainsKey("serrorstring"))
                    {
                        screeningResponse.Errormsg = jsonResult["serrorstring"].ToString();
                    }
                    screeningResponse.resultsJSON = result;
                    screeningResponse.userId = Helper.oUser.UserId;
                    screeningResponse.credentialId = Helper.lstEnrichCreds.FirstOrDefault(x => x.EnrichmentType == "OWNERSHIP").CredentialId;
                    screeningResponse.searchJSON = json;
                    screeningResponse.requestUrl = endpoint;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    screeningResponse.resultsJSON = result;
                    screeningResponse.userId = Helper.oUser.UserId;
                    screeningResponse.credentialId = Helper.lstEnrichCreds.FirstOrDefault(x => x.EnrichmentType == "OWNERSHIP").CredentialId;
                    screeningResponse.searchJSON = json;
                    screeningResponse.requestUrl = endpoint;
                }
            }
            return screeningResponse;
        }
    }
}