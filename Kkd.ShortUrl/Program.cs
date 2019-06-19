using System;
using System.IO;
using Kkd.ShortUrl.Modals;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kkd.ShortUrl {
    public class Program {
        public static IConfiguration Configuration { get; } =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    true)
                .AddEnvironmentVariables()
                .Build();

        public static void Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            Log.Information("程序启动...");
            Console.Title = "短地址服务";

            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope()) {
                try {
                    var dc = scope.ServiceProvider.GetService<ShortUrlContext>();
                    dc.Database.Migrate();
                }
                catch (Exception ex) {
                    Log.Error(ex, "An error occurred while migrating or initializing the database.");
                }
            }

            try {
                host.Run();
                Log.Information("程序结束。");
            }
            catch (Exception ex) {
                Log.Fatal(ex, "意外终止。");
            }
            finally {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(Configuration)
                .UseSerilog();
        }
    }
}