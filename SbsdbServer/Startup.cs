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
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers()  // keine Views, das erledigt die Angular-App
                .AddNewtonsoftJson(options => 
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                // Validierungsfehler fangen
                .ConfigureApiBehaviorOptions(o => {
                    o.InvalidModelStateResponseFactory = context => new ValidationProblemDetailsResult();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // NTLM via IIS
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            
            // json komprimieren
            services.AddResponseCompression(options => {
                options.EnableForHttps = true;
                options.MimeTypes = new[] {"application/json"};
                                            // "application/json; charset=utf-8",
                                            // "application/json;charset=utf-8" };
            });
            
            // DB-Connection-Strings holen
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

            // alter Bestand - MySQL/EF
            // TODO kann raus, sobald alles auf neue Oracle-Struktur ueberfuehrt
            services.AddDbContextPool<Sbsdbv4Context>(
                options => options
                    //   .UseLazyLoadingProxies()   // sofern lazy loading gewuenscht
                    .UseMySql(connStrv4,
                        mySqlOptions => { mySqlOptions.ServerVersion(new Version(5, 5, 60), ServerType.MySql); }
                    )
            );

            // neuer Bestand - Oracle/EF
            services.AddDbContextPool<SbsdbContext>(
                options => options 
                    // .UseLazyLoadingProxies() // sofern lazy loading gewuenscht
                    .UseOracle(connStr)
            );

            // Versionsinfos werden per .csproj gesetzt, hier aus Assembly auslesen 
            // und fuer alle Services zur Verfuegung stellen
            var version = new VersionResource();
            services.AddSingleton(version);

            // Services und Repositories
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITreeService, TreeService>();
            services.AddTransient<ITreeRepository, TreeRepository>();
            services.AddTransient<IApService, ApService>();
            services.AddTransient<IApRepository, ApRepository>();
            services.AddTransient<IConfigService, ConfigService>();
            services.AddTransient<IConfigRepository, ConfigRepository>();

            services.AddTransient<v4Migration, v4Migration>();
            services.AddTransient<TestService, TestService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            // TODO kann raus, wenn die Datenbankstruktur steht 
            // create-table-commands fuer alle Entities die SbsdbContext verwaltet ins Log schreiben
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
            app.UseDefaultFiles(); // wg. index.html
            app.UseStaticFiles(new StaticFileOptions()); // -> wwwroot f. Angular-App
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
