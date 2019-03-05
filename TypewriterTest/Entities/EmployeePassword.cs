using System;

namespace TypewriterTest.Entities {
    [TypeScript]
    public class EmployeePassword : Entity {
        public Guid EmployeeId { get; set; }
        public string Pwd { get; set; }
    }

}