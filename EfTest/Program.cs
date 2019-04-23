using System;
using System.Linq;
using EfTest.Entities;

namespace EfTest {
    internal class Program {
        private static void Main(string[] args) {
            Console.Title = "EFTest";
            Add();

            Read();

            var isContinue = true;
            do {
                var cmd = Console.ReadLine().ToLower();
                if (string.IsNullOrEmpty(cmd)) {
                    continue;
                }
                switch (cmd) {
                    case "add":
                        Add();
                        break;
                    case "read":
                        Read();
                        break;
                    case "exit":
                    case "quit":
                        isContinue = false;
                        break;
                    default:
                        break;
                }
            } while (isContinue);
        }

        private static void Read() {
            Console.WriteLine(" 读取记录 -------开始-------");
            using (var dc = new TestContext()) {
                foreach (var b in dc.Businesses.OrderBy(d => d.No)) {
                    Console.WriteLine($"    no={b.No}, SIdOnAdd={b.SIdOnAdd}");
                }

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
