using SBISCCMWeb.Models.OI.CleanseMatch;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Utility
{
    public class APIUtility
    {
        #region GetOICleanseMatchResult Function

        public static OICleanseMatchViewModel GetOICleanseMatchResult(string CompanyName, string Address1, string Address2, string City, string State, string Country, string Zipcode, string Telephon, string connectionString, string CustomerSubDomain, string SrcRecordId = "123", string inputId = "", string orb_num = "", string EIN = "", string Website = "", string Email = "")
        {
            OICleanseMatchViewModel oICleanseMatchViewModel = new OICleanseMatchViewModel();
            OICleanseMatchResponse oICleanseMatchResponse = new OICleanseMatchResponse();
            List<OICleanseMatchOutput> oICleanseMatchOutputs = new List<OICleanseMatchOutput>();

            try
            {
                SettingFacade fac = new SettingFacade(connectionString);
                //string endPoint = fac.GetOICleanseMatchURLEncode(CustomerSubDomain, Country, SrcRecordId == null ? "123" : SrcRecordId, inputId == null ? "" : inputId, CompanyName == null ? "" : CompanyName, Address1, Address2, City, State, Zipcode, Telephon, "", "", "", "", "", "");
                string endPoint = fac.GetOICleanseMatchURLEncode(CustomerSubDomain, Country == null ? "" : Country, string.IsNullOrEmpty(SrcRecordId) ? "123" : SrcRecordId, inputId, CompanyName == null ? "" : CompanyName, Address1, Address2, City, State, Zipcode, Telephon, "", "", "", "", "", "");
                //string endPoint = "https://localhost/CleanseMatch/OI?CustomerSubDomain=sbisccmdev:9092&Country=USA&SrcRecordId=123&CompanyName=dell&address1=St&city=MtDora&state=FL&PostalCode=32757";

                if (!string.IsNullOrEmpty(orb_num))
                {
                    endPoint += "&OrbNum=" + orb_num;
                }
                if (!string.IsNullOrEmpty(EIN))
                {
                    endPoint += "&EIN=" + EIN;
                }
                if (!string.IsNullOrEmpty(Website))
                {
                    endPoint += "&Website=" + Website;
                }
                if (!string.IsNullOrEmpty(Email))
                {
                    endPoint += "&Email=" + Email;
                }
                string cleanJsonResponse = string.Empty;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + Helper.OIAPIKey);
                //httpWebRequest.Headers.Add("Authorization", "Bearer 30324d3b-49f9-4078-82f0-5240830c5709");

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    cleanJsonResponse = streamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(cleanJsonResponse))
                    {
                        var serializer = new JavaScriptSerializer();
                        oICleanseMatchResponse = serializer.Deserialize<OICleanseMatchResponse>(cleanJsonResponse);

                        //getting wrong candidate counts from API, for resolving that issue set records count which are get from API 
                        //for resolving error set custom result count (MP-718)
                        if (oICleanseMatchResponse != null && oICleanseMatchResponse.results != null && oICleanseMatchResponse.results.Any())
                        {
                            oICleanseMatchResponse.results_count = oICleanseMatchResponse.results.Count;
                        }

                        if (oICleanseMatchResponse != null && oICleanseMatchResponse.results_count > 0)
                        {
                            foreach (var item in oICleanseMatchResponse.results)
                            {
                                OICleanseMatchOutput oICleanseMatchOutput = new OICleanseMatchOutput();
                                oICleanseMatchOutput.result_number = item.result_number;
                                oICleanseMatchOutput.orb_num = item.orb_num;
                                oICleanseMatchOutput.name = item.name;
                                oICleanseMatchOutput.std_streetnum = item.std_streetnum;
                                oICleanseMatchOutput.std_streetname = item.std_streetname;
                                oICleanseMatchOutput.address1 = item.address1;
                                oICleanseMatchOutput.city = item.city;
                                oICleanseMatchOutput.state = item.state;
                                oICleanseMatchOutput.zip = item.zip;
                                oICleanseMatchOutput.company_status = item.company_status;
                                oICleanseMatchOutput.confidence_score = item.confidence_score;
                                oICleanseMatchOutput.entity_type = item.entity_type;
                                oICleanseMatchOutput.is_standalone_company = item.is_standalone_company;
                                oICleanseMatchOutput.branches_count = item.branches_count;
                                oICleanseMatchOutputs.Add(oICleanseMatchOutput);
                            }
                            oICleanseMatchViewModel.oICleanseMatchOutputs = oICleanseMatchOutputs;
                            //oICleanseMatchViewModel.ResponseJson = cleanJsonResponse;

                            //getting wrong candidate counts from API, for resolving that issue set records count which are get from API 
                            //for resolving error set custom result count (MP-718)
                            oICleanseMatchViewModel.ResponseJson = serializer.Serialize(oICleanseMatchResponse);


                        }
                        else if (oICleanseMatchResponse != null && oICleanseMatchResponse.results_count == 0 && oICleanseMatchViewModel.Error == null)
                        {
                            oICleanseMatchViewModel.Error = "No records found.";
                            oICleanseMatchViewModel.ResponseJson = cleanJsonResponse;
                        }
                        else
                        {
                            oICleanseMatchViewModel.Error = oICleanseMatchResponse.ErrorMessage;
                        }
                        oICleanseMatchViewModel.MatchUrl = endPoint;
                    }

                }
            }
            catch (WebException ex)
            {
                string ErrorMessage = ex.Message;

                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    oICleanseMatchViewModel.Error = result.ToString();
                }
            }
            catch (Exception ex)
            {
                oICleanseMatchViewModel.Error = ex.Message;
            }
            return oICleanseMatchViewModel;
        }

        #endregion //GetOICleanseMatchResult Function
    }
}