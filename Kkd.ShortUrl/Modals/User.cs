using System.ComponentModel.DataAnnotations;

namespace Kkd.ShortUrl.Modals {
    public class User : Entity {
        [Required] [MaxLength(100)] public string CompanyName { get; set; }

        [Required] [MaxLength(32)] public string Token { get; set; }
    }
}