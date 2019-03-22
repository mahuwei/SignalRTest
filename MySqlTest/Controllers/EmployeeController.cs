using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MySqlTest.Models;

namespace MySqlTest.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase {
        private readonly MyContext _mc;

        public EmployeeController(MyContext mc) {
            _mc = mc;
            if (_mc.Employees.Any()) return;
            var businesses = _mc.Businesses.ToList();
            foreach (var business in businesses) {
                var e = new Employee {
                    Id = Guid.NewGuid(),
                    BusinessId = business.Id,
                    No = "001",
                    Name = "管理员",
                    MobileNo = "139",
                    Status = 0,
                    LastChange = DateTime.Now
                };
                _mc.Employees.Add(e);
            }

            _mc.SaveChanges();
        }

        public ActionResult<IEnumerable<Employee>> Get() {
            return Ok(_mc.Employees.ToList());
        }

        [HttpPost]
        public ActionResult<IEnumerable<Employee>> Save([FromBody] Employee employee) {
            if (employee == null) return BadRequest("传入参数为null");
            var old = _mc.Employees.Find(employee.Id);
            if (old == null) return BadRequest("没有找到原对象");
            if (old.RowFlag == null || employee.RowFlag == null) return BadRequest("没有行标识");

            for (var i = 0; i < old.RowFlag.Length; i++)
                if (employee.RowFlag[i] != old.RowFlag[i])
                    return BadRequest("行标识与库中数据不符。");

            _mc.Entry(old).CurrentValues.SetValues(employee);
            _mc.SaveChanges();
            return Ok(new List<Employee> {old, employee});
        }
    }
}