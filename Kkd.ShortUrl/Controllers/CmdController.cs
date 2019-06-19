using System;
using System.Text;
using Kkd.ShortUrl.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kkd.ShortUrl.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CmdController : ControllerBase {
        private readonly ILogger<CmdController> _logger;
        private readonly Service _service;

        public CmdController(ILogger<CmdController> logger) {
            _service = Service.GetInstance();
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Create([FromBody] string longUrl) {
            var err = _service.CheckToken(Request);
            if (err != null) {
                _logger.LogWarning("生成，返回:404 {@err};  参数:{@longUrl} 调用:{@sourceUrl}", err, longUrl,
                    _service.GetRemote(Request));
                return BadRequest(err);
            }

            var url = GetBaseUrl();
            var ret = _service.Create(longUrl, url);
            _logger.LogInformation("生成：{@ret} 调用:{@sourceUrl}", JsonConvert.SerializeObject(ret),
                _service.GetRemote(Request));
            return Ok(ret);
        }

        private string GetBaseUrl() {
            var url = new StringBuilder()
                .Append(Request.Scheme)
                .Append("://")
                .Append(Request.Host).ToString();
            return url;
        }

        [HttpPost]
        public ActionResult Query([FromBody] string shortUrl) {
            var err = _service.CheckToken(Request);
            if (err != null) {
                _logger.LogWarning("查询，返回:404 {@err};  参数:{@shortUrl} 调用:{@sourceUrl}", err, shortUrl,
                    _service.GetRemote(Request));
                return BadRequest(err);
            }

            var baseUrl = GetBaseUrl();
            if (shortUrl.IndexOf(baseUrl, StringComparison.OrdinalIgnoreCase) == -1) {
                var shortUrlResult = new ShortUrlResult {
                    Code = -3,
                    ShortUrl = shortUrl,
                    ErrMsg = "无效的地址。"
                };

                WriteLog(shortUrl, shortUrlResult);
                return Ok(shortUrlResult);
            }

            var tmp = shortUrl.Substring(baseUrl.Length + 1);
            var ret = _service.GetLongUrl(tmp);
            if (ret == null) {
                var shortUrlResult = new ShortUrlResult {
                    Code = -3,
                    ShortUrl = shortUrl,
                    ErrMsg = "没找到"
                };
                WriteLog(shortUrl, shortUrlResult);
                return Ok(shortUrlResult);
            }

            var urlResult = new ShortUrlResult {
                Code = 0,
                ShortUrl = shortUrl,
                LongUrl = ret
            };
            WriteLog(shortUrl, urlResult);
            return Ok(urlResult);
        }

        private void WriteLog(string shortUrl, ShortUrlResult shortUrlResult) {
            _logger.LogInformation("查询，返回:200 {@ret};  参数:{@shortUrl} 调用:{@sourceUrl}",
                shortUrlResult,
                shortUrl,
                _service.GetRemote(Request));
        }
    }
}