using System;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace ShortUrl {
    public class H5ip {
        private const string QueryUrl = "http://h5ip.cn/index/api?format=json&url={0}";

        public static async Task<string> Create(string longUrl) {
            var lr = HttpUtility.UrlEncode(longUrl);
            var client = ShortUrl.CreateHttpClient(null);
            var response = await client.PostAsync(string.Format(QueryUrl, lr),null);
            if (response.IsSuccessStatusCode) {
                var data = response.Content.ReadAsStringAsync();
                var ret = JsonConvert.DeserializeObject<H5ipResult>(data.Result);
                if (ret.code == 0)
                    return ret.short_url;
                throw new Exception(ret.msg);
            }

            throw new Exception("error");
        }
    }

    public class H5ipResult {
        public int code { get; set; }
        public string msg { get; set; }
        public string short_url { get; set; }
    }
}