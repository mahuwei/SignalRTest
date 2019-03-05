using System.Collections.Generic;

namespace TypewriterTest.Entities {
    [TypeScript]
    public class Employee : Entity {
        public string No { get; set; }
        public string Name { get; set; }
        public List<string> Address { get; set; }
    }
}