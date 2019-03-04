using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRTest.Models;

namespace SignalRTest {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors();
            services.AddIdentityServer()
                .AddDeveloperSigningCredential();

            //services.AddCors(options => options.AddPolicy("CorsPolicy",
            //    builder => {
            //        builder.AllowAnyOrigin() // .WithOrigins("http://localhost:4200")
            //            .AllowAnyMethod()
            //            .AllowAnyHeader();
            //            //.AllowCredentials();
            //    }));

            services.AddCors(options => {
                options.AddPolicy(
                    "CorsPolicy",
                    x => {
                        x.AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(_ => {
                                Console.WriteLine(_);
                                return true;
                            })
                            .AllowCredentials();
                    });
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            //app.UseCors(builder => {
            //    builder.WithOrigins("http://localhost:4200") // Configuration["AllowedHosts"])
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials();
            //    //builder.SetIsOriginAllowedToAllowWildcardSubdomains();
            //});

            //app.UseSignalR(options => { options.MapHub<ChatHub>("/hub"); });

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes => { routes.MapHub<ChatHub>("/hub"); });

            //app.UseIdentityServer();
            app.UseMvc();
        }
    }
}