using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShortUrl {
    public class ShortUrl {
        private const string CsCreateUrl = "https://dwz.cn/admin/v2/create";
        private const string CsQueryUrl = "https://dwz.cn/admin/v2/query";
        private const string CsToken = "4ef6f2903031c773d2c78b386d740529";

        /// <summary>
        ///     生成短地址
        /// </summary>
        /// <param name="longUrl">长地址</param>
        /// <returns>返回<see cref="ShortUrlResult" /></returns>
        public static async Task<ShortUrlResult> Create(string longUrl) {
            return await CreateShortUrl(CsToken, CsCreateUrl, longUrl);
        }

        /// <summary>
        ///     短网址还原接口
        /// </summary>
        /// <param name="shortUrl">短地址</param>
        /// <returns>返回<see cref="ShortUrlResult" /></returns>
        public static async Task<ShortUrlResult> Query(string shortUrl) {
            return await QueryShortUrl(CsToken, CsQueryUrl, shortUrl);
        }

        private static async Task<ShortUrlResult> CreateShortUrl(string token, string createUrl, string longUrl) {
            var client = CreateHttpClient(token);
            var response = await client.PostAsync(createUrl,
                new StringContent(JsonConvert.SerializeObject(new {Url = longUrl})));
            if (response.IsSuccessStatusCode) {
                var data = response.Content.ReadAsStringAsync();
                var ret = JsonConvert.DeserializeObject<ShortUrlResult>(data.Result);
                return ret;
            }

            var message = response.Content.ReadAsStringAsync().Result;
            throw new Exception($"{response.StatusCode} {message}");
        }

        private static async Task<ShortUrlResult> QueryShortUrl(string token, string queryUrl, string shortUrl) {
            var client = CreateHttpClient(token);
            var response = await client.PostAsync(queryUrl,
                new StringContent(JsonConvert.SerializeObject(new {ShortUrl = shortUrl})));
            if (response.IsSuccessStatusCode) {
                var data = response.Content.ReadAsStringAsync();
                var ret = JsonConvert.DeserializeObject<ShortUrlResult>(data.Result);
                return ret;
            }

            var message = response.Content.ReadAsStringAsync().Result;
            throw new Exception($"{response.StatusCode} {message}");
        }

        private static HttpClient CreateHttpClient(string token) {
            var client = HttpClientFactory.Create();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Token", token);
            return client;
        }
    }

    public class ShortUrlResult {
        /// <summary>
        ///     返回值：
        ///     0：正常返回短网址
        ///     -1：短网址对应的长网址不合法
        ///     -2：短网址不存在
        ///     -3：查询的短网址不合法
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     短地址
        /// </summary>
        public string ShortUrl { get; set; }

        /// <summary>
        ///     长地址
        /// </summary>
        public string LongUrl { get; set; }

        /// <summary>
        ///     错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}