using System;
using System.Linq;
using Kkd.ShortUrl.Modals;
using Microsoft.AspNetCore.Mvc;

namespace Kkd.ShortUrl.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly Service _service;

        public UserController() {
            _service = Service.GetInstance();
        }

        [HttpPost]
        public ActionResult Create([FromBody] string companyName) {
            if (CheckToken(out var badRequest)) return badRequest;

            try {
                var user = _service.CreateUser(companyName);
                return Ok(new {
                    user.CompanyName,
                    user.Token
                });
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
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
                _service.CheckUser(header.Value[0], true);
            }
            catch (Exception e) {
                {
                    badRequest = BadRequest($"Token无效。\n{e.Message}");
                    return true;
                }
            }

            return false;
        }
    }
}