using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SBISCCMWeb.Utility
{
    public class RestfulHttpClient<T, TResourceIdentifier> : IDisposable where T : class
    {
        private bool disposed = false;
        private HttpClient httpClient;
        protected readonly string serviceBaseAddress;
        private readonly string addressSuffix;
        private string UserName, Password, Token;
        private string MediaType = "application/json";

        /// <summary>
        /// Set Http Client with UserName and Password
        /// </summary>
        /// <param name="serviceBaseAddress"></param>
        /// <param name="addressSuffix"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="mediaType"></param>
        public RestfulHttpClient(string serviceBaseAddress, string addressSuffix, string userName, string password, string mediaType)
        {
            this.serviceBaseAddress = serviceBaseAddress;
            this.addressSuffix = addressSuffix;
            this.MediaType = mediaType;
            this.UserName = userName;
            this.Password = password;
            httpClient = CreateHttpClient(serviceBaseAddress);
        }
        /// <summary>
        /// Set Http Client with Token.
        /// </summary>
        /// <param name="serviceBaseAddress"></param>
        /// <param name="addressSuffix"></param>
        /// <param name="token"></param>
        /// <param name="mediaType"></param>
        public RestfulHttpClient(string serviceBaseAddress, string addressSuffix, string token, string mediaType)
        {
            this.serviceBaseAddress = serviceBaseAddress;
            this.addressSuffix = addressSuffix;
            this.MediaType = mediaType;
            this.Token = token;
            httpClient = CreateHttpClient(serviceBaseAddress);
        }
        private HttpClient CreateHttpClient(string serviceBaseAddress)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceBaseAddress);
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaType));
            return httpClient;
        }
        /// <summary>
        /// Get Auth Reposnse - Fetch token
        /// </summary>
        /// <returns></returns>
        public T GetAuthResponse()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, serviceBaseAddress);
            requestMessage.Headers.Add("x-dnb-user", UserName);
            requestMessage.Headers.Add("x-dnb-pwd", Password);
            HttpResponseMessage responseMessage = httpClient.SendAsync(requestMessage).Result;
            if (responseMessage != null)
            {
                return responseMessage.Content.ReadAsAsync<T>().Result;
            }
            return null;
        }
        /// <summary>
        /// Get Response
        /// </summary>
        /// <returns></returns>
        public T GetResponse()
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, serviceBaseAddress);
            requestMessage.Headers.TryAddWithoutValidation("Authorization", Token);

            HttpResponseMessage responseMessage = httpClient.SendAsync(requestMessage).Result;

            if (responseMessage != null)
            {
                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception(responseMessage.ReasonPhrase);

                var fileContent = responseMessage.Content.ReadAsStringAsync().Result;
                var purgetContent = purgeContent(fileContent);

                return JsonConvert.DeserializeObject<T>(purgetContent);
            }
            return null;
        }
        private string purgeContent(string fileContent)
        {
            var result = fileContent.Replace("{\"OrganizationName\":", "{\"_OrganizationName\":")
                                    .Replace("\"IndustryCode\":[", "\"_IndustryCode\":[")
                                    .Replace("\"IndustryCode\":{\"$\"", "\"IndustryCode\":{\"$\"")
                                    .Replace("\"@type\":", "\"_type\":")
                                    .Replace("\"$\"", "\"_param\"")
                                    .Replace("@", "");

            return result;
        }
        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (httpClient != null)
                {
                    var hc = httpClient;
                    httpClient = null;
                    hc.Dispose();
                }
                disposed = true;
            }
        }

        #endregion IDisposable Members
    }
}