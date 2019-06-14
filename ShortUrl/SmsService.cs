using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ShortUrl {
    public class SmsService {
        private readonly string _cNo;
        private readonly string _orderNo;

        public SmsService(string orderNo, string cNo) {
            _orderNo = orderNo;
            _cNo = cNo;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public void Send(string mobileNo, string url) {
            var shortUrl = ShortUrl.Create(url).Result.ShortUrl;
            var modalText = "{orderNo}{mobileNo}|{codeNo}|尊敬的{customerName}，欢迎您入住{hotelShortName}{roomNo}房间，点击 {shortUrl} 查看酒店介绍和相关活动【{signName}】";
            var data = $"1021{mobileNo}|102001|尊敬的马虎维，欢迎您入住国贸大饭店8309房间，点击 {shortUrl} 查看酒店介绍和相关活动【国贸大饭店】";
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var address = IPAddress.Parse("218.26.36.12");
            socket.Connect(address, 9905);
            var bytes = Encoding.GetEncoding("GBK").GetBytes(data);
            var count = bytes.Length.ToString().PadLeft(4,'0');
            bytes = Encoding.GetEncoding("GBK").GetBytes(count + data);

            socket.Send(bytes);
            var receiveBuffers = new byte[1024];
            var length = socket.Receive(receiveBuffers);
            if (length <= 0) {
                Console.WriteLine("读取返回失败。");
            }
            else {
                var rd = Encoding.GetEncoding("GBK").GetString(receiveBuffers, 0, length);
                Console.WriteLine($"返回结果：{rd}");   
            }

            socket.Close();
            socket.Dispose();
        }
    }
}