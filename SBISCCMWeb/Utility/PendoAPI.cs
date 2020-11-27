using Newtonsoft.Json;
using SBISCCMWeb.Models;
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
    public class PendoAPI
    {
        #region Tracking Events Configuration-Pendo API
        public Pendo pendoAPI(string SearchEvent)
        {
            Pendo model = new Pendo();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string endpoint = ConfigurationManager.AppSettings["PendoAPI"];

            model = GetValues(SearchEvent);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("x-pendo-integration-key", "dc22d69f-97ca-4288-52a4-00fe566b48d2");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
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
                    var serializer = new JavaScriptSerializer();
                    Pendo objResponse = serializer.Deserialize<Pendo>(result);
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
                        Pendo objResponse = JsonConvert.DeserializeObject<Pendo>(result);
                        return objResponse;
                    }
                }
            }
            return null;
        }
        public Pendo GetValues(string SearchEvent)
        {
            Pendo model = new Pendo();
            model.properties = new Properties();
            model.context = new Context();

            model.type = "track";
            model.@event = SearchEvent;
            model.visitorId = Helper.oUser.EmailAddress;
            model.accountId = System.Web.HttpContext.Current.Request.Url.Authority;
            model.timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            model.properties.plan = "";
            model.properties.accountType = "";
            model.context.ip = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            model.context.userAgent = System.Web.HttpContext.Current.Request.UserAgent;
            model.context.url = System.Web.HttpContext.Current.Request.Url.ToString();
            model.context.title = new System.Uri(System.Web.HttpContext.Current.Request.Url.ToString()).AbsolutePath.Replace("/", "");
            return model;
        }
        #endregion
    }
}