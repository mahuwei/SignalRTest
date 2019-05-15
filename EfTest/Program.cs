using System;
using System.Linq;
using EfTest.Entities;

namespace EfTest {
    internal class Program {
        private static void Main(string[] args) {
            Console.Title = "EFTest";
            //Add();

            //Read();

            var isContinue = true;
            do {
                var cmd = Console.ReadLine()?.ToLower();
                if (string.IsNullOrEmpty(cmd)) continue;

                var input = cmd.Split(' ');
                switch (input[0]) {
                    case "add":
                        Add();
                        break;
                    case "read":
                        Read();
                        break;
                    case "update":
                        if (input.Length == 1) break;

                        Update(input[1]);
                        break;
                    case "exit":
                    case "quit":
                        isContinue = false;
                        break;
                }
            } while (isContinue);
        }

        private static void Update(string no) {
            Console.WriteLine(" 修改记录 -------开始-------");
            using (var dc = new TestContext()) {
                var business = dc.Businesses.FirstOrDefault(d => d.No == no.Trim().PadLeft(4, '0'));
                if (business == null) {
                    Console.WriteLine(" 没有找到原纪录。");
                    return;
                }

                var index = business.Name.IndexOf("Update", StringComparison.Ordinal);
                if (index > -1) business.Name = business.Name.Substring(0, index);

                business.Name += $"Update:{DateTime.Now.ToLongTimeString()}";
                dc.Businesses.Update(business);
                dc.SaveChanges();
            }

            Console.WriteLine(" 修改记录 -------结束-------");
        }

        private static void Read() {
            Console.WriteLine(" 读取记录 -------开始-------");
            using (var dc = new TestContext()) {
                foreach (var b in dc.Businesses.OrderBy(d => d.No))
                    Console.WriteLine($"    no={b.No}, SIdOnAdd={b.SIdOnAdd}");
            }

            Console.WriteLine(" 读取记录 -------结束-------");
        }

        private static void Add() {
            Console.WriteLine(" 添加记录 -------开始-------");
            using (var dc = new TestContext()) {
                var count = dc.Businesses.Count();
                for (var i = count; i < count + 10; i++) {
                    var b = new Business {
                        Id = Guid.NewGuid(),
                        No = (i + 1).ToString().PadLeft(4, '0'),
                        Name = $"第{i}个商户"
                    };
                    dc.Businesses.Add(b);
                }

                dc.SaveChanges();
            }

            Console.WriteLine(" 添加记录 -------结束-------");
        }
    }
}