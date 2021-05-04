using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShoppingModule.Web.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingModule.Web
{
    [ExcludeFromCodeCoverage]
    public class ServicesContract : IServicesContract
    {
        private readonly ILogger<ServicesContract> _logger;
        private static readonly object threadlock = new object();
        private static volatile HttpClient _httpClient;
        public static int TimeOutInSecs = 45;

        public ServicesContract(ILogger<ServicesContract> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method will be called when need to make GET request to API
        /// </summary>
        /// <typeparam name="T">return data type</typeparam>
        /// <param name="url">api endpoint</param>
        /// <param name="token">bearer token</param>
        /// <returns>T Generic Object</returns>
        public async Task<T> GetAsync<T>(string url, string token = "")
        {
            _logger.LogInformation(string.Format("ServiceBase: Calling GET API - Api URL: {0} ", url));
            T response = default(T);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpResponseMessage result = null;

                result = Task.Run(() => GetHttpClient().SendAsync(request))?.Result;

                if (result != null)
                {
                    if (!result.IsSuccessStatusCode)
                    {
                        string error = await result.Content?.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(error))
                        {
                            object formatted = JsonConvert.DeserializeObject(error);
                            _logger.LogError("Result was not success - Message - " + JsonConvert.SerializeObject(formatted));
                        }
                        else
                        {
                            _logger.LogError("Error is NULL!");
                        }
                    }

                    string responseString = Task.Run(() => result.Content?.ReadAsStringAsync())?.Result;
                    if (!string.IsNullOrEmpty(responseString))
                    {
                        var res = JsonConvert.DeserializeObject<T>(responseString);
                        _logger.LogInformation(string.Format("ServiceBase: Calling GET API - Api URL: {0} ", url));
                        return res;
                    }
                    else
                    {
                        _logger.LogError("Response String is empty!");
                    }
                }
                return response;
            }

        }

        /// <summary>
        /// Method will be called when need to make POST request to API
        /// </summary>
        /// <typeparam name="T">return data type</typeparam>
        /// <param name="url">api endpoint</param>
        /// <param name="token">bearer token</param>
        /// <param name="json">request data</param>
        /// <returns>T Generic Object</returns>
        public async Task<T> PostAsync<T>(string url, string json, string token = "")
        {
            _logger.LogInformation(string.Format("ServiceBase: Calling POST API - Api URL: {0} ", url));
            var response = default(T);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var formattedString = JsonConvert.DeserializeObject(json);
                request.Content = new StringContent(Convert.ToString(formattedString), Encoding.UTF8, "application/json");
                HttpResponseMessage result = null;

                result = Task.Run(() => GetHttpClient().SendAsync(request))?.Result;

                if (result != null)
                {
                    if (!result.IsSuccessStatusCode)
                    {
                        string error = await result.Content?.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(error))
                        {
                            object formatted = JsonConvert.DeserializeObject(error);
                            _logger.LogError("Result was not success - Message - " + JsonConvert.SerializeObject(formatted));
                        }
                        else
                        {
                            _logger.LogError("Error is NULL!");
                        }
                    }

                    string responseString = Task.Run(() => result.Content?.ReadAsStringAsync())?.Result;
                    _logger.LogError("Class Service Contract. Method: PostAsync.URL: " + url + " Res string :" + responseString);
                    if (!string.IsNullOrEmpty(responseString))
                    {
                        var res = JsonConvert.DeserializeObject<T>(responseString);
                        _logger.LogInformation(string.Format("ServiceBase : POST - Api URL: {0}. Complete", url));
                        return res;
                    }
                    else
                    {
                        _logger.LogError("Response string is empty!");
                    }
                }
                return response;
            }
        }

        /// <summary>
        /// Method will be called when need to make POST request to API
        /// </summary>
        /// <typeparam name="T">Return datatype</typeparam>        
        /// <param name="data">Input data object</param>
        /// <param name="url">URL of API that needs to be called</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T, M>(M data, string url)
        {
            T response = default;
            try
            {
                _logger.LogInformation(string.Format("ServiceBase: Calling API - Api URL: {0} ", url));

                string postBody = JsonConvert.SerializeObject(data);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                GetHttpClient().DefaultRequestHeaders.Accept.Clear();
                GetHttpClient().DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage result = null;
                result = Task.Run(() => GetHttpClient().PostAsync(url, new StringContent(postBody, Encoding.UTF8, "application/json")))?.Result;

                if (result != null)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        string responseString = await result.Content?.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(responseString))
                        {
                            var res = JsonConvert.DeserializeObject<T>(responseString);
                            return res;
                        }
                        else
                        {
                            _logger.LogError("Response string is empty!");
                        }
                    }
                    else
                    {
                        var apiResponse = await result?.Content?.ReadAsStringAsync();
                        _logger.LogError(string.Format("ServiceBase: Post to Url {0} failed, status code {1}, Reason phrase {2} and Request Headers: {3}  Request Body: {4} and Response: {5}", url, result?.StatusCode, result?.ReasonPhrase, result?.RequestMessage?.Headers?.ToString(), postBody, apiResponse));
                    }
                }
                _logger.LogInformation(string.Format("ServiceBase : Post - Api URL: {0}. Complete", url));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ServiceBase: Error while making POST request", ex);
                return response;
            }
        }

        #region Private Methods

        /// <summary>
        /// Method to Create Http Client Object
        /// </summary>
        /// <param name="apiTimeoutInSeconds">API Timeout Seconds</param>
        /// <returns>Http Client Object</returns>
        private static HttpClient GetHttpClient()
        {

            try
            {
                if (_httpClient == null)
                {
                    lock (threadlock)
                    {
                        if (_httpClient == null)
                        {
                            _httpClient = new HttpClient();
                            _httpClient.Timeout = TimeSpan.FromSeconds(TimeOutInSecs);
                            return _httpClient;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _httpClient;
        }

        #endregion
    }
}
