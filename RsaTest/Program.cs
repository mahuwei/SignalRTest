using System;
using System.Security.Cryptography;
using System.Text;
using XC.RSAUtil;

namespace RsaTest {
    internal class Program {
        private static void Main(string[] args) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var str = "筷开动1234Abc";
            var gbk = Encoding.GetEncoding("GBK").GetBytes(str);
            var gbkToStr = Encoding.GetEncoding("GBK").GetString(gbk);
            Console.WriteLine($"GBK是否相同：{str == gbkToStr}\n");

            Console.WriteLine("获取Ticks测试------start-------");
            for (var i = 0; i < 10; i++) {
                var ticks = DateTime.Now.Ticks.ToString();
                Console.WriteLine($"    {i} ticks:{ticks} len:{ticks.Length}");
            }
            Console.WriteLine("获取Ticks测试------end-------");

            var keyList = RsaKeyGenerator.XmlKey(2048);
            var privateKey = keyList[0];
            var publicKey = keyList[1];

            var source = "1234再看看看！@#￥#！#";
            var rsaXmlUtil = new RsaXmlUtil(Encoding.UTF8, publicKey, privateKey);
            var encryptData = rsaXmlUtil.Encrypt(source, RSAEncryptionPadding.OaepSHA256);
            Console.WriteLine("加密测试：");
            Console.WriteLine("   加密后的数据：{0}", encryptData);
            var decryptData = rsaXmlUtil.Decrypt(encryptData, RSAEncryptionPadding.OaepSHA256);
            Console.WriteLine("\n   解密后的数据：{0}", decryptData);

            Console.WriteLine("   解密后的数据与源数据相同:{0}", source == decryptData);
            Console.WriteLine("\n签名测试：");
            var signData = rsaXmlUtil.SignData(source, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            Console.WriteLine("   签名值：{0}", signData);
            var verifyRet = rsaXmlUtil.VerifyData(source, signData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            Console.WriteLine("   验证签名：{0}", verifyRet);

            Console.WriteLine("\n签名转换:");
            var privatePkcs1 = RsaKeyConvert.PrivateKeyXmlToPkcs1(privateKey);
            var publicPkcs1 = RsaKeyConvert.PublicKeyXmlToPem(publicKey);

            Console.WriteLine(" private key of Pkcs1:{0}", privatePkcs1);
            Console.WriteLine(" public key of Pkcs1:{0}", publicPkcs1);

            Console.WriteLine("Guid转换String测试.---开始---");
            var id = Guid.NewGuid();

            var idString = id.ToString("N");
            Console.WriteLine("Guid.ToString(N)={0},原guid:{1}", idString, id);
            var idFromString = Guid.ParseExact(idString, "N");
            Console.WriteLine("Guid.ParseExact(idString,N):{0} ,是否相等：{1}", idFromString, idFromString == id);
            Console.WriteLine("Guid转换String测试.---结束。");
            Console.ReadLine();
        }
    }
}