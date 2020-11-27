using Newtonsoft.Json;
using SBISCCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Utility
{
    public class Clients
    {
        public string EditClientsListByClientCode(string ClientCode, bool GetMissingDataFromProvider)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string endpoint = ConfigurationManager.AppSettings["EditMissingProvider"];

            endpoint += "?ClientCode=" + ClientCode;
            endpoint += "&GetMissingDataFromProvider=" + GetMissingDataFromProvider;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    ClientCode = ClientCode,
                    GetMissingDataFromProvider = GetMissingDataFromProvider
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
                    return result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        result = streamReader.ReadToEnd();
                        return result;
                    }
                }
            }
            return null;
        }

        public string GetClientCode(string ClientCode)
        {
            if (!string.IsNullOrEmpty(ClientCode))
            {
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string endpoint = ConfigurationManager.AppSettings["GetMissingProvider"];

            endpoint += "?ClientCode=" + ClientCode;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        result = streamReader.ReadToEnd();
                        return result;
                    }
                }
            }
            return null;
        }

        public List<DownloadCacheDataModel> DownloadCacheData(string ClientCode, string CountryISOAlpha2Code, string APIFamily)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string endpoint = ConfigurationManager.AppSettings["DownloadCacheData"];

            if (ClientCode != null)
            {
                endpoint += "?ClientCode=" + ClientCode;
            }
            if (CountryISOAlpha2Code != null)
            {
                endpoint += "&CountryISOAlpha2Code=" + CountryISOAlpha2Code;
            }
            if (APIFamily != null)
            {
                endpoint += "&APIFamily=" + APIFamily;
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;

                    if (result.Contains("No records found."))
                    {
                        return null;
                    }
                    List<DownloadCacheDataModel> objResponse = serializer.Deserialize<List<DownloadCacheDataModel>>(result);
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        List<DownloadCacheDataModel> objResponse = serializer.Deserialize<List<DownloadCacheDataModel>>(result);
                        return objResponse;
                    }
                }
            }
            return null;
        }



        public string DeleteCachedCleanseMatch(string ClientCode, string APIFamily, DateTime? BeginDateTime, DateTime? EndDateTime)
        {
            string endpoint = ConfigurationManager.AppSettings["DeleteCachedCleanseMatch"];
            endpoint += "?ClientCode=" + ClientCode;
            if (!string.IsNullOrEmpty(APIFamily))
                endpoint += "&APIFamily=" + APIFamily;
            if (BeginDateTime.HasValue)
                endpoint += "&BeginDateTime=" + BeginDateTime;
            if (EndDateTime.HasValue)
                endpoint += "&EndDateTime=" + EndDateTime;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        result = streamReader.ReadToEnd();
                        return result;
                    }
                }
            }
            return null;
        }
        public string DeleteCachedEnrichment(string ClientCode, string APIType, string DUNSNumber, DateTime? BeginDateTime, DateTime? EndDateTime)
        {
            string endpoint = ConfigurationManager.AppSettings["DeleteCachedEnrichment"];
            endpoint += "?ClientCode=" + ClientCode;
            if (!string.IsNullOrEmpty(APIType))
                endpoint += "&APIType=" + APIType;
            if (!string.IsNullOrEmpty(DUNSNumber))
                endpoint += "&DUNSNumber=" + DUNSNumber;
            if (BeginDateTime.HasValue)
                endpoint += "&BeginDateTime=" + BeginDateTime;
            if (EndDateTime.HasValue)
                endpoint += "&EndDateTime=" + EndDateTime;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        result = streamReader.ReadToEnd();
                        return result;
                    }
                }
            }
            return null;
        }
    }
}