using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class RestfulClientFacade<T> where T : class
    {
        private string UserName, Password, URL;

        public RestfulClientFacade(string _UserName, string _Password, string _URL)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            this.UserName = _UserName;
            this.Password = _Password;
            this.URL = _URL;
        }
        public T GetResponse()
        {
            var client = new RestClient(URL);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("x-dnb-user", UserName);
            request.AddHeader("x-dnb-pwd", Password);
            request.RequestFormat = DataFormat.Json;
            IRestResponse restResponse = client.Execute(request);
            string response = restResponse.Content;
            if (!string.IsNullOrEmpty(response))
            {
                return JsonConvert.DeserializeObject<T>(response);
            }
            return null;
        }
        public T GetDirectPlusResponse()
        {
            var client = new RestClient(URL);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");


            string autorization = UserName + ":" + Password;
            byte[] binaryAuthorization = System.Text.Encoding.Default.GetBytes(autorization);
            autorization = Convert.ToBase64String(binaryAuthorization);
            request.AddHeader("Authorization", "Basic " + autorization);
            request.AddHeader("Origin", "www.dnb.com");
            request.AddJsonBody(new { grant_type = "client_credentials" });


            request.RequestFormat = DataFormat.Json;
            IRestResponse restResponse = client.Execute(request);
            string response = restResponse.Content;
            if (!string.IsNullOrEmpty(response))
            {
                return JsonConvert.DeserializeObject<T>(response);
            }
            return null;
        }
        public static T GetJsonFromRefreshURL(string URL, string ApiToken)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(URL);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ApiToken);

            request.RequestFormat = DataFormat.Json;
            IRestResponse restResponse = client.Execute(request);
            string response = restResponse.Content;
            if (!string.IsNullOrEmpty(response))
            {
                return purgeForRefresh(response) as T;
            }
            return null;
        }
        private static string purgeForRefresh(string fileContent)
        {
            var result = fileContent.Replace("{\"OrganizationName\":", "{\"_OrganizationName\":")
                                .Replace("{\"FindCompetitorResponse\":", "{\"OrderProductResponse\":")
                                .Replace("\"IndustryCode\":[", "\"_IndustryCode\":[")
                                .Replace("\"IndustryCode\":{\"$\"", "\"IndustryCode\":{\"$\"")
                                .Replace("\"@type\":", "\"_type\":")
                                .Replace("\"$\"", "\"_param\"")
                                .Replace("@", string.Empty);

            return result;
        }
        public static T GetJsonFromCleanseMatchURL(string URL, string ApiToken = null)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(URL);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            if (!string.IsNullOrEmpty(ApiToken))
            {
                request.AddHeader("Authorization", ApiToken);
            }

            request.RequestFormat = DataFormat.Json;
            IRestResponse restResponse = client.Execute(request);
            string response = restResponse.Content;
            if (!string.IsNullOrEmpty(response))
            {
                return (response) as T;
            }
            return null;
        }
        public static T GetJsonFromCleanseMatchURLWithPurge(string url, string ApiToken)
        {
            T response = GetJsonFromCleanseMatchURL(url, ApiToken);
            return purgeForCleanseMatch(response) as T;
        }
        private static string purgeForCleanseMatch(T fileContent)
        {
            var result = fileContent.ToString().Replace("{\"OrganizationName\":", "{\"_OrganizationName\":")
                                .Replace("{\"FindCompetitorResponse\":", "{\"OrderProductResponse\":")
                                .Replace("\"IndustryCode\":[", "\"_IndustryCode\":[")
                                .Replace("\"IndustryCode\":{\"$\"", "\"IndustryCode\":{\"$\"")
                                .Replace("\"@type\":", "\"_type\":")
                                .Replace("\"$\"", "\"_param\"")
                                .Replace("@", string.Empty);

            return result;
        }
        public static T GetObject(string URL, string ApiToken = null)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(URL);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            if (!string.IsNullOrEmpty(ApiToken))
            {
                request.AddHeader("Authorization", ApiToken);
            }

            request.RequestFormat = DataFormat.Json;
            IRestResponse restResponse = client.Execute(request);
            string response = restResponse.Content;
            if (!string.IsNullOrEmpty(response))
            {
                return JsonConvert.DeserializeObject<T>(response);
            }
            return null;
        }
    }
}