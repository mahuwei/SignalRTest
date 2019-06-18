using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Kkd.ShortUrl.Modals {
    public class Service {
        private const string CsAdminName = "筷开动";
        private static Service _serviceInstance;
        private static readonly object Locker = new object();

        private static readonly string[] Chars = {
            "G", "H", "I", "J", "K", "L", "M", "N", "O", "P",
            "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            "g", "h", "i", "j", "k", "l", "m", "n", "o", "p",
            "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
        };

        private static User _userAdmin;

        private readonly Dictionary<string, UrlMap> _dictMaps;
        private MaxRecord _maxRecord;

        private Service() {
            using (var dc = new ShortUrlContext()) {
                _maxRecord = dc.MaxRecords.FirstOrDefault();
                _dictMaps = new Dictionary<string, UrlMap>();
                var maps = dc.UrlMaps.Where(d => d.Status < 1000).OrderByDescending(d => d.LastChange).Take(100);
                foreach (var map in maps) _dictMaps.Add(map.ShortUrl, map);
            }
        }

        public static Service GetInstance() {
            if (_serviceInstance == null) {
                using (var dc = new ShortUrlContext()) {
                    _userAdmin = dc.Users.FirstOrDefault(d => d.CompanyName == CsAdminName);
                    if (_userAdmin == null) {
                        _userAdmin = new User {
                            Id = Guid.NewGuid(),
                            CompanyName = CsAdminName,
                            LastChange = DateTime.Now
                        };
                        _userAdmin.Token = Md5(_userAdmin.Id + CsAdminName);
                        dc.Users.Add(_userAdmin);
                        dc.SaveChanges();
                    }
                }

                _serviceInstance = new Service();
            }

            return _serviceInstance;
        }

        public ShortUrlResult Create(string longUrl, string baseUrl) {
            var result = new ShortUrlResult {
                Code = 0
            };
            var md5 = Md5(longUrl);
            var hadMap = _dictMaps.FirstOrDefault(d => d.Value.Md5 == md5);
            if (hadMap.Value != null) {
                result.ShortUrl = $"{baseUrl}/{hadMap.Value.ShortUrl}";
                return result;
            }

            lock (Locker) {
                using (var dc = new ShortUrlContext()) {
                    var umTmp = dc.UrlMaps.FirstOrDefault(d => d.Md5 == md5);
                    if (umTmp != null) {
                        ChangeRate(umTmp);
                        result.ShortUrl = umTmp.ShortUrl;
                        return result;
                    }

                    if (_maxRecord == null) {
                        _maxRecord = new MaxRecord {
                            Id = Guid.NewGuid(),
                            Status = 0,
                            No = 1,
                            LastChange = DateTime.Now
                        };
                        dc.MaxRecords.Add(_maxRecord);
                    }
                    else {
                        _maxRecord.No += 1;
                        _maxRecord.LastChange = DateTime.Now;
                        var old = dc.MaxRecords.Find(_maxRecord.Id);
                        dc.Entry(old).CurrentValues.SetValues(_maxRecord);
                    }

                    var um = new UrlMap {
                        Id = Guid.NewGuid(),
                        LongUrl = longUrl,
                        ShortUrl = Encrypt(_maxRecord.No),
                        Md5 = md5,
                        Status = (int) EntityStatus.Init,
                        LastChange = DateTime.Now
                    };
                    dc.UrlMaps.Add(um);
                    dc.SaveChanges();
                    ChangeRate(um);
                    result.ShortUrl = $"{baseUrl}/{um.ShortUrl}";
                    return result;
                }
            }
        }

        public string GetLongUrl(string shortUrl) {
            var key = shortUrl;
            var map = _dictMaps.FirstOrDefault(d => d.Key == key);
            if (map.Value != null) {
                ChangeRate(map.Value);
                return map.Value.LongUrl;
            }

            using (var dc = new ShortUrlContext()) {
                var urlMap = dc.UrlMaps.FirstOrDefault(d => d.ShortUrl == shortUrl);
                if (urlMap == null) return null;

                ChangeRate(urlMap);
                return urlMap.LongUrl;
            }
        }

        private void ChangeRate(UrlMap urlMap) {
            if (urlMap == null) return;
            urlMap.LastChange = DateTime.Now;
            if (_dictMaps.ContainsKey(urlMap.ShortUrl)) return;
            if (_dictMaps.Count >= 100) {
                var tmp = _dictMaps.OrderBy(d => d.Value.LastChange);
                var minUrl = tmp.First();
                _dictMaps.Remove(minUrl.Key);
            }

            _dictMaps.Add(urlMap.ShortUrl, urlMap);
        }

        /// <summary>
        ///     MD5加密字符串（32位大写）
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5(string source) {
            var md5 = new MD5CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(source);
            var result = BitConverter.ToString(md5.ComputeHash(bytes));
            return result.Replace("-", "");
        }


        public static string Encrypt(long n) {
            var hexStr = n.ToString("X");
            var len = hexStr.Length;
            var random = new Random();
            var result = "";
            for (var i = 0; i < 6 - len; i++) {
                var index = random.Next(Chars.Length - 1);
                result += Chars[index];
            }

            result += hexStr;
            return result;
        }

        public User CreateUser(string companyName) {
            using (var dc = new ShortUrlContext()) {
                var user = dc.Users.FirstOrDefault(d => d.CompanyName == companyName.Trim());
                if (user != null) return user;
                user = new User {
                    Id = Guid.NewGuid(),
                    CompanyName = companyName.Trim(),
                    LastChange = DateTime.Now
                };
                user.Token = Md5(user.Id + user.CompanyName);
                dc.Users.Add(user);
                dc.SaveChanges();
                return user;
            }
        }

        public void CheckUser(string token, bool isAdmin = false) {
            if (string.IsNullOrEmpty(token)) throw new Exception("没有指定Token");

            if (isAdmin)
                if (_userAdmin.Token != token)
                    throw new Exception("非法操作。");

            using (var dc = new ShortUrlContext()) {
                var user = dc.Users.FirstOrDefault(d => d.Token == token && d.Status == (int) EntityStatus.Init);
                if (user == null) throw new Exception("Token无效。");
            }
        }
    }

    public class ShortUrlResult {
        /// <summary>
        ///     返回值：
        ///     0：正常返回短网址
        ///     -1：短网址对应的长网址不合法
        ///     -2：短网址不存在
        ///     -3：查询的短网址不合法
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     短地址
        /// </summary>
        public string ShortUrl { get; set; }

        /// <summary>
        ///     长地址
        /// </summary>
        public string LongUrl { get; set; }

        /// <summary>
        ///     错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}