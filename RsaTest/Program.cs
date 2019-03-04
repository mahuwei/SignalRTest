using System;
using System.Security.Cryptography;
using System.Text;
using XC.RSAUtil;

namespace RsaTest {
    internal class Program {
        private static void Main(string[] args) {
            var keyList = RsaKeyGenerator.XmlKey(2048);
            var privateKey = keyList[0];
            var publicKey = keyList[1];

            var source = "1234再看看看！@#￥#！#";
            var rsaXmlUtil = new RsaXmlUtil(Encoding.UTF8, publicKey, privateKey);
            var encryptData = rsaXmlUtil.Encrypt(source, RSAEncryptionPadding.OaepSHA256);
            Console.WriteLine("加密测试：");
            Console.WriteLine("   加密后的数据：{0}" , encryptData);
            var decryptData = rsaXmlUtil.Decrypt(encryptData, RSAEncryptionPadding.OaepSHA256);
            Console.WriteLine("\n   解密后的数据：{0}", decryptData);

            Console.WriteLine("   解密后的数据与源数据相同:{0}", source == decryptData);
            Console.WriteLine("\n签名测试：");
            var signData = rsaXmlUtil.SignData(source, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            Console.WriteLine("   签名值：{0}", signData);
            var verifyRet = rsaXmlUtil.VerifyData(source, signData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            Console.WriteLine("   验证签名：{0}",verifyRet);

            Console.WriteLine("\n签名转换:");
            var privatePkcs1 = RsaKeyConvert.PrivateKeyXmlToPkcs1(privateKey);
            var publicPkcs1 = RsaKeyConvert.PublicKeyXmlToPem(publicKey);

            Console.WriteLine(" private key of Pkcs1:{0}",privatePkcs1);
            Console.WriteLine(" public key of Pkcs1:{0}",publicPkcs1);


            Console.ReadLine();
        }
    }
}