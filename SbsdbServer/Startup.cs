using System;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using hb.Common.Validation;
using hb.Common.Version;
using hb.SbsdbServer.Model;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.sbsdbv4.model;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
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

            services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate(); 
            
            // Authorization Helper als Singleton registrieren
            var auth = new AuthorizationHelper(/*Configuration*/);
            services.AddSingleton(auth);
            
            // Auth via [Authorize(Role = ... ] klappt nicht auf allen Systemen,
            // daher wird hier eine Policy definiert, die die Unterschiede
            // beruecksichtigen kann -> [Authorize(Policy = "adminUser")].
            //
            // Die fn wird immer zweimal aufgerufen, beim ersten Mal ohen Daten fuer den Benutzer,
            // User.Identity.IsAuthenticated ist dann false. Da ist natuerlich auch kein Check
            // auf Gruppen moeglich, es ist auch egal, ob true oder false zurueckgegeben wird.
            // Desalb der zusaetzliche Check auf IsAuthenticated.
            services.AddAuthorization(options => {
                options.AddPolicy("adminUser",
                    policyBuilder => policyBuilder.RequireAssertion(
                        context => context.User.Identity.IsAuthenticated ? auth.IsAdmin(context.User) : false
                        ));
            });
            
            // json komprimieren
            services.AddResponseCompression(options => {
                options.EnableForHttps = true;
                options.MimeTypes = new[] {"application/json"};
            });
            
            // DB-Connection-Strings holen
#if TESTSYSTEM  // unterschiedliche Connection-Strings fuer verschiedene System
            string connStr;
            string connStrv4;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
              connStr = Configuration.GetConnectionString("sbsdbxx");
              connStrv4 = Configuration.GetConnectionString("sbsdbv4x");
            }
            else {
              connStr = Configuration.GetConnectionString("sbsdbx");
              connStrv4 = Configuration.GetConnectionString("sbsdbv4x");
            }
#else
            string connStr = Configuration.GetConnectionString("sbsdb");
            string connStrv4 = Configuration.GetConnectionString("sbsdbv4");
#endif
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
            Console.WriteLine(version.Package());

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
            // TODO auch in Produktion drinlassen?
            // if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            // }

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
            
            // wird nur fuer Kestrel-Server (Linux, macOS) gebraucht
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                app.UsePathBase(new PathString("/791/sbsdb")); 
            }
            //      app.UseHttpsRedirection();  // falls das mal auf https laeuft

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
