using Newtonsoft.Json;
using SBISCCMWeb.Models.PreviewMatchData.Main;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Utility
{
    public class PreviewEnrichmentData
    {
        public string PreviewData(string EnrichmentURL, string AuthToken)
        {
            DunsInfo model = new DunsInfo();

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
                    //DunsInfo objResponse = serializer.Deserialize<DunsInfo>(result);
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
                    //if (result != null)
                    //{
                    //    var serializer = new JavaScriptSerializer();
                    //    //DunsInfo objResponse = JsonConvert.DeserializeObject<DunsInfo>(result);
                    //    string objResponse = serializer.DeserializeObject(result).ToString();
                    //    return objResponse;
                    //}
                }
            }
            return null;
        }
    }
}