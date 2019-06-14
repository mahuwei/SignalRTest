using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShortUrl {
    /// <summary>
    ///     .net core 2 HttpClient
    /// </summary>
    public class HttpClientCore {
        private const string CsAuthScheme = "Bearer";
        private const int CsTimeoutSeconds = 15;

        private static HttpClient CreateClient(string token = null) {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Token", token);
            client.Timeout = new TimeSpan(0, 0, 0, CsTimeoutSeconds);

            if (token != null) {
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(CsAuthScheme, token);
            }
            return client;
        }


        public static async Task<T> Get<T>(string serverUrl, string route) where T : class {
            return await Get<T>(serverUrl, route, null, null, null);
        }

        public static async Task<T> Get<T>(string serverUrl, string route, string token) where T : class {
            return await Get<T>(serverUrl, route, token, null, null);
        }


        public static async Task<T> Get<T>(string serverUrl, string route, string[] paramVariables,
            string[] paramValues) where T : class {
            return await Get<T>(serverUrl, route, null, paramVariables, paramValues);
        }

        /// <summary>
        ///     REST GET
        ///     捕获错误需要ex.GetBaseException().Message来获取错误信息
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="serverUrl">Url</param>
        /// <param name="route">
        ///     对应Controller名称和Route;不包含api
        ///     <example>values/route</example>
        /// </param>
        /// <param name="token">Token</param>
        /// <param name="paramVariables">参数数组</param>
        /// <param name="paramValues">参数值数组</param>
        /// <returns></returns>
        public static async Task<T> Get<T>(string serverUrl, string route, string token, string[] paramVariables,
            string[] paramValues) where T : class {
            var client = CreateClient(token);
            var url = CreateUrl(serverUrl, route, paramVariables, paramValues);
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode) {
                var data = response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data.Result);
            }

            var message = response.Content.ReadAsStringAsync().Result;
            throw new Exception($"{response.StatusCode} {message}");
        }

        public static async Task<T> Post<T>(string serverUrl, string route, object value) where T : class {
            return await Post<T>(serverUrl, route, null, null, null, value);
        }

        public static async Task<T> Post<T>(string serverUrl, string route, string token, object value)
            where T : class {
            return await Post<T>(serverUrl, route, token, null, null, value);
        }

        public static async Task<T> Post<T>(string serverUrl, string route, string[] paramVariables,
            string[] paramValues, object value) where T : class {
            return await Post<T>(serverUrl, route, null, paramVariables, paramValues, value);
        }

        /// <summary>
        ///     REST Post
        ///     捕获错误需要ex.GetBaseException().Message来获取错误信息
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="serverUrl">Url</param>
        /// <param name="route">
        ///     对应Controller名称和Route;不包含api
        ///     <example>values/route</example>
        /// </param>
        /// <param name="token">Token</param>
        /// <param name="paramVariables">参数数组</param>
        /// <param name="paramValues">参数值数组</param>
        /// <param name="value">需要携带的Data</param>
        /// <returns></returns>
        public static async Task<T> Post<T>(string serverUrl, string route, string token, string[] paramVariables,
            string[] paramValues, object value) where T : class {
            var client = CreateClient(token);
            var sc = new StringContent(value is string s ? s : JsonConvert.SerializeObject(value), Encoding.UTF8,
                "application/json");
            var response = await client.PostAsync(CreateUrl(serverUrl, route, paramVariables, paramValues), sc);
            if (response.IsSuccessStatusCode) {
                var data = response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data.Result);
            }

            var message = response.Content.ReadAsStringAsync().Result;
            throw new Exception($"{response.StatusCode} {message}");
        }


        public static async Task<T> Post<T>(string serverUrl, string route, string[] paramVariables,
            string[] paramValues, MultipartFormDataContent content) where T : class {
            return await Post<T>(serverUrl, route, null, paramVariables, paramValues, content);
        }

        /// <summary>
        ///     上传文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serverUrl"></param>
        /// <param name="route"></param>
        /// <param name="token"></param>
        /// <param name="paramVariables"></param>
        /// <param name="paramValues"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<T> Post<T>(string serverUrl, string route, string token, string[] paramVariables,
            string[] paramValues, MultipartFormDataContent content) where T : class {
            var client = CreateClient(token);
            var response = await client.PostAsync(CreateUrl(serverUrl, route, paramVariables, paramValues), content);
            if (response.IsSuccessStatusCode) {
                var data = response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data.Result);
            }

            var message = response.Content.ReadAsStringAsync().Result;
            throw new Exception($"{response.StatusCode} {message}");
        }

        private static string CreateUrl(string serverUrl, string router, IReadOnlyList<string> paramVariables,
            IReadOnlyList<string> paramValues) {
            var url = new StringBuilder($"{serverUrl}");
            if (string.IsNullOrEmpty(router) && paramVariables == null) {
                return url.ToString();
            }
            if (serverUrl.EndsWith("/") == false) url.Append("/");
            if (string.IsNullOrEmpty(router) == false) url.Append($"{router}");

            if (paramVariables == null || paramVariables.Count == 0) return url.ToString();
            for (var i = 0; i < paramVariables.Count; i++) {
                url.Append(i != 0 ? "&" : "?");
                url.Append($"{paramVariables[i]}={paramValues[i]}");
            }

            return url.ToString();
        }
    }
}