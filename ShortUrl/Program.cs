using System;

namespace ShortUrl {
    internal class Program {
        private static void Main(string[] args) {
            var longUrl = "http://192.168.1.160:44373/Welcome/4d954670-66b9-44a1-9c62-d4339b12f839/Preview/index.html";

            //var result = ShortUrl.Create(longUrl).Result;
            //Console.WriteLine($"执行{(result.Code == 0 ? "成功." : $"失败:{result.Code} {result.ErrMsg}")}");
            //if (result.Code == 0) Console.WriteLine($" ShortUrl:{result.ShortUrl}");
            //Console.WriteLine("\n");

            //var queryResult = ShortUrl.Query(result.ShortUrl).Result;
            //Console.WriteLine($"执行{(queryResult.Code == 0 ? "成功." : $"失败:{queryResult.Code} {queryResult.ErrMsg}")}");
            //if (queryResult.Code == 0) Console.WriteLine($" LongUrl:{queryResult.LongUrl}");

            //Console.WriteLine("\n");
            //Console.WriteLine($"与源地址比较：{queryResult.LongUrl == longUrl}");

            var ms = new SmsService("1021", "102001");
            do {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;
                if (input.ToLower() == "exit") break;

                var orders = input.ToLower().Split(' ');
                if (orders[0] != "send" || orders.Length == 1 || string.IsNullOrEmpty(orders[1])) continue;

                ms.Send(orders[1], longUrl);
            } while (true);

            Console.ReadLine();
        }
    }
}