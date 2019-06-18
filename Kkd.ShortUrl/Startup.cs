using Kkd.ShortUrl.Modals;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Kkd.ShortUrl {
    public class Startup {
        private string _baseUrl;

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<AppSettings> settings) {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            _baseUrl = settings.Value.BaseUrl;
            app.UseHttpsRedirection();

            var options = new RewriteOptions().Add(AppRule);
            app.UseRewriter(options);

            using (var dc = new ShortUrlContext()) {
                dc.Database.Migrate();
            }

            app.UseMvc();
        }

        private void AppRule(RewriteContext context) {
            var request = context.HttpContext.Request;
            if (request.Path.StartsWithSegments(new PathString("/api"))) return;

            var urlEnd = request.Path.Value.Substring(1);
            if (urlEnd.Length != 6) return;
            var service = Service.GetInstance();
            var lr = service.GetLongUrl(urlEnd);
            if (string.IsNullOrEmpty(lr)) return;

            var response = context.HttpContext.Response;
            response.StatusCode = StatusCodes.Status301MovedPermanently;
            context.Result = RuleResult.EndResponse;
            response.Headers[HeaderNames.Location] = lr;
        }
    }
}