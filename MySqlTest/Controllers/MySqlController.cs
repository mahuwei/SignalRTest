using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using MySqlTest.Models;

namespace MySqlTest.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MySqlController : ControllerBase {
        private readonly MyContext _mc;

        public MySqlController(MyContext mc) {
            _mc = mc;

            if (EnumerableExtensions.Any(_mc.Businesses)) return;
            var business = new Business {
                Id = Guid.NewGuid(),
                No = "001",
                Name = "名称001",
                Status = (int) EntityStatus.Init,
                LastChange = DateTime.Now
            };
            _mc.Businesses.Add(business);
            business = new Business {
                Id = Guid.NewGuid(),
                No = "002",
                Name = "名称002",
                Status = (int) EntityStatus.Init,
                LastChange = DateTime.Now
            };
            _mc.Businesses.Add(business);
            _mc.SaveChanges();
        }

        public ActionResult<IEnumerable<Business>> Get() {
            return Ok(_mc.Businesses.ToList());
        }
    }
}