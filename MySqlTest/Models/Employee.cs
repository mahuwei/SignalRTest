using System;
using Newtonsoft.Json;

namespace MySqlTest.Models {
    public class Employee : Entity {
        public Guid BusinessId { get; set; }

        [JsonIgnore] public Business Business { get; set; }

        public string No { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
    }
}