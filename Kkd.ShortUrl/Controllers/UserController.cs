using System;
using Kkd.ShortUrl.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kkd.ShortUrl.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly ILogger<UserController> _logger;
        private readonly Service _service;

        public UserController(ILogger<UserController> logger) {
            _logger = logger;
            _service = Service.GetInstance();
        }

        [HttpPost]
        public ActionResult Create([FromBody] string companyName) {
            var err = _service.CheckToken(Request, true);
            if (err != null) {
                _logger.LogWarning("生成Token，返回:404 {@err};  参数:{@companyName} 调用:{@sourceUrl}", err, companyName,
                    _service.GetRemote(Request));
                return BadRequest(err);
            }

            try {
                var user = _service.CreateUser(companyName);
                _logger.LogInformation("生成Token，返回:200 {@ret};  参数:{@companyName} 调用:{@sourceUrl}", new {
                        user.CompanyName,
                        user.Token
                    }, companyName,
                    _service.GetRemote(Request));
                return Ok(new {
                    user.CompanyName,
                    user.Token
                });
            }
            catch (Exception ex) {
                _logger.LogError(ex, "生成Token，返回:404;  参数:{@companyName} 调用:{@sourceUrl}", companyName,
                    _service.GetRemote(Request));
                return BadRequest(ex.Message);
            }
        }
    }
}