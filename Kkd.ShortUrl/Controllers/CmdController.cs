using System;
using System.Linq;
using System.Text;
using Kkd.ShortUrl.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Kkd.ShortUrl.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CmdController : ControllerBase {
        private readonly Service _service;
        private readonly IOptions<AppSettings> _setting;

        public CmdController(IOptions<AppSettings> settings) {
            _service = Service.GetInstance();
            _setting = settings;
        }

        [HttpPost]
        public ActionResult Create([FromBody] string longUrl) {
            if (CheckToken(out var badRequest)) return badRequest;
            //var url = Request.GetDisplayUrl();
            var url = GetBaseUrl();
            var ret = _service.Create(longUrl, url);
            return Ok(ret);
        }

        private string GetBaseUrl() {
            var url = new StringBuilder()
                .Append(Request.Scheme)
                .Append("://")
                .Append(Request.Host).ToString();
            return url;
        }

        private bool CheckToken(out ActionResult badRequest) {
            badRequest = null;
            var header =
                Request.Headers.FirstOrDefault(d => d.Key.Equals("token", StringComparison.InvariantCultureIgnoreCase));
            if (string.IsNullOrEmpty(header.Key)) {
                badRequest = BadRequest("没有指定Token。");
                return true;
            }

            try {
                _service.CheckUser(header.Value[0]);
            }
            catch (Exception e) {
                {
                    badRequest = BadRequest($"{e.Message}");
                    return true;
                }
            }

            return false;
        }

        [HttpPost]
        public ActionResult Query([FromBody] string shortUrl) {
            if (CheckToken(out var badRequest)) return badRequest;
            var baseUrl = GetBaseUrl();
            if (shortUrl.IndexOf(baseUrl, StringComparison.OrdinalIgnoreCase) == -1)
                return Ok(new ShortUrlResult {
                    Code = -3,
                    ErrMsg = "无效的地址。"
                });
            var tmp = shortUrl.Substring(baseUrl.Length + 1);
            var ret = _service.GetLongUrl(tmp);
            if (ret == null)
                return Ok(new ShortUrlResult {
                    Code = -3,
                    ErrMsg = "没找到"
                });
            return Ok(new ShortUrlResult {
                Code = 0,
                ShortUrl = shortUrl,
                LongUrl = ret
            });
        }
    }
}