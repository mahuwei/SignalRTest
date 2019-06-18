using System.ComponentModel.DataAnnotations;

namespace Kkd.ShortUrl.Modals {
    public class UrlMap : Entity {
        [Required] [MaxLength(200)] public string LongUrl { get; set; }

        [Required] [MaxLength(200)] public string ShortUrl { get; set; }

        [Required] [MaxLength(50)] public string Md5 { get; set; }
    }
}