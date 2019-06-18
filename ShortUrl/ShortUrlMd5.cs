using System;
using System.Security.Cryptography;
using System.Text;

namespace ShortUrl {
    internal class ShortUrlMd5 {
        public static string[] ShortUrl(string url) {
            //可以自定义生成MD5加密字符传前的混合KEY
            var key = "Leejor";
            //要使用生成URL的字符
            string[] chars = {
                "a", "b", "c", "d", "e", "f", "g", "h",
                "i", "j", "k", "l", "m", "n", "o", "p",
                "q", "r", "s", "t", "u", "v", "w", "x",
                "y", "z", "0", "1", "2", "3", "4", "5",
                "6", "7", "8", "9", "A", "B", "C", "D",
                "E", "F", "G", "H", "I", "J", "K", "L",
                "M", "N", "O", "P", "Q", "R", "S", "T",
                "U", "V", "W", "X", "Y", "Z"
            };
            //对传入网址进行MD5加密
            var hex = Md5(key + url);

            var resUrl = new string[4];

            for (var i = 0; i < 4; i++) {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
                var hexInt = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16);
                var outChars = string.Empty;
                for (var j = 0; j < 6; j++) {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                    var index = 0x0000003D & hexInt;
                    //把取得的字符相加
                    outChars += chars[index];
                    //每次循环按位右移5位
                    hexInt = hexInt >> 5;
                }

                //把字符串存入对应索引的输出数组
                resUrl[i] = outChars;
            }

            return resUrl;
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
    }
}