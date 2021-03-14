using System;
using hb.Common.Validation;
using hb.Common.Version;
using hb.SbsdbServer.Model;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.sbsdbv4.model;
using hb.SbsdbServer.Services;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace hb.SbsdbServer {
    public class Startup {
        // private readonly ILogger<Startup> _log;

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
            
            // _log = logger;
            // _log.LogDebug("Startup c'tor");
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // _log.LogDebug("ConfigureService()");
            services.AddControllers()  // #3.0# alt AddMvc()
                .AddNewtonsoftJson(options => 
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                // Validierungsfehler fangen
                .ConfigureApiBehaviorOptions(o => {
                    o.InvalidModelStateResponseFactory = context => {
                        // _log.LogInformation("Validation Error");
                        return new ValidationProblemDetailsResult();
                    };
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // _log.LogDebug("ConfigureService() #1#");
            // NTLM via IIS
            services.AddAuthentication(IISDefaults.AuthenticationScheme);  // #3.0# nicht mehr noetig??
            
            // _log.LogDebug("ConfigureService() #2#");
            // json komprimieren
            services.AddResponseCompression(options => {
                options.EnableForHttps = true;
                options.MimeTypes = new[] {"application/json"};
//                                            "application/json; charset=utf-8",
//                                            "application/json;charset=utf-8" };
            });
            
            // _log.LogDebug("ConfigureService() #3#");
            // connection strings holen
            string connStr;
            string connStrv4;
            if (Configuration.GetValue<string>("VRZKennung") != null) { // SPK-Umgebung
                connStr = Configuration.GetConnectionString("sbsdb");
                connStrv4 = Configuration.GetConnectionString("sbsdbv4");
            }
            else {
                connStr = Configuration.GetConnectionString("sbsdbx");
                connStrv4 = Configuration.GetConnectionString("sbsdbv4x");
            }

            // _log.LogDebug("ConfigureService() #4#");
            // alter Bestand - MySQL/EF
            // TODO kann raus, sobald alles auf neue Oracle-Struktur ueberfuehrt
            services.AddDbContextPool<Sbsdbv4Context>(
                options => options
                    //   .UseLazyLoadingProxies()   // sofern lazy loading gewuenscht
                    .UseMySql(connStrv4,
                        mySqlOptions => { mySqlOptions.ServerVersion(new Version(5, 5, 60), ServerType.MySql); }
                    )
            );

            // _log.LogDebug("ConfigureService() #5#");
            // neuer Bestand - Oracle/EF
            services.AddDbContextPool<SbsdbContext>(
                options => options 
                    // .UseLazyLoadingProxies() // sofern lazy loading gewuenscht
                    .UseOracle(connStr)
            );


            // _log.LogDebug("ConfigureService() #6#");
            // Versionsinfos werden per .csproj gesetzt, hier aus Assembly auslesen
            var version = new VersionResource();
            services.AddSingleton(version);
            // _log.LogDebug("ConfigureService() " + version);

            // _log.LogDebug("ConfigureService() #7#");
            // Services und Repositories
            services.AddTransient<TestService, TestService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITreeService, TreeService>();
            services.AddTransient<ITreeRepository, TreeRepository>();
            services.AddTransient<IApService, ApService>();
            services.AddTransient<IApRepository, ApRepository>();
            services.AddTransient<IConfigService, ConfigService>();
            services.AddTransient<IConfigRepository, ConfigRepository>();

            services.AddTransient<v4Migration, v4Migration>();

            // _log.LogInformation("Starting  " + version);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            // create-table-commands fuer alle Entities die SbsdbContext verwaltet ins Log schreiben
            // TODO kann raus, wenn die Datenbankstruktur steht 
            /*using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                         .CreateScope()) {
              LOG.LogDebug(serviceScope.ServiceProvider.GetService<SbsdbContext>()
                  .Database.GenerateCreateScript());
            }
            */

            // Exceptions fangen und einheitlich zurueckgeben (Status 500)
            app.UseExceptionHandler(a => a.Run(async context => {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;
                var result = JsonConvert.SerializeObject(
                    (exception: exception.GetType().Name,
                    message: exception.Message,
                    stacktrace: exception.StackTrace));
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(result);
            }));

            app.UseResponseCompression();
            //      app.UseHttpsRedirection();  // falls das mal auf https laeuft
            // app.UseMvc(); // #3.0# 
            app.UseDefaultFiles(); // wg. index.html
            app.UseStaticFiles(new StaticFileOptions()); // -> wwwroot
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
            app.UseSpa(conf => conf.Options.DefaultPage = "/index.html"); // Angular-SPA
            // app.UseServerSentEvents();  // TODO noch zu testen
        }
    }
}
