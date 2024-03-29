﻿using System;
using System.Runtime.InteropServices;
using hb.Common.Validation;
using hb.Common.Version;
using hb.SbsdbServer.Model;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
                });
                // obsolete? .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate(); 
            
            // Authorization Helper als Singleton registrieren
            var auth = new AuthorizationHelper(Configuration);
            services.AddSingleton(auth);
            
            // Auth via [Authorize(Role = ... ] klappt nicht auf allen Systemen,
            // daher wird hier eine Policy definiert, die die Unterschiede
            // beruecksichtigen kann -> [Authorize(Policy = "adminUser")].
            //
            // Die fn wird immer zweimal aufgerufen, beim ersten Mal ohne Daten fuer den Benutzer,
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
            string connStr = Configuration.GetConnectionString("sbsdb");

            // neuer Bestand - Oracle/EF
            services.AddDbContextPool<SbsdbContext>(
                options => options 
                    .UseOracle(connStr, opt => 
                        opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery).CommandTimeout(10)
                        )
            );

            // Notifications
            services.AddSignalR();
            
            // Versionsinfos werden per .csproj gesetzt, hier aus Assembly auslesen 
            // und fuer alle Services zur Verfuegung stellen
            var version = new VersionResource();
            services.AddSingleton(version);
            Console.WriteLine(version.Package());

            // Services und Repositories
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IApService, ApService>();
            services.AddTransient<IApRepository, ApRepository>();
            services.AddTransient<IConfigService, ConfigService>();
            services.AddTransient<IConfigRepository, ConfigRepository>();
            services.AddTransient<IBetrstService, BetrstService>();
            services.AddTransient<IBetrstRepository, BetrstRepository>();
            services.AddTransient<IHwKonfigService, HwKonfigService>();
            services.AddTransient<IHwKonfigRepository, HwKonfigRepository>();
            services.AddTransient<IHwService, HwService>();
            services.AddTransient<IHwRepository, HwRepository>();
            services.AddTransient<IExternalService, ExternalService>();
            services.AddTransient<ISvzRepository, SvzRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            // Exceptions fangen und einheitlich zurueckgeben (Status 500)
            app.UseExceptionHandler(a => a.Run(async context => {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature?.Error;
                var result = JsonConvert.SerializeObject(
                    (exception: exception?.GetType().Name,
                    message: exception?.Message,
                    stacktrace: exception?.StackTrace));
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(result);
            }));

            app.UseResponseCompression();
            
            // wird nur fuer Kestrel-Server (Linux, macOS) gebraucht
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                app.UsePathBase(new PathString(Const.BASE_URL)); 
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions() {
                OnPrepareResponse = (context) => {
                    // Disable caching for all static files.
                    context.Context.Response.Headers["Cache-Control"] = Configuration["StaticFiles:Headers:Cache-Control"];
                    context.Context.Response.Headers["Pragma"] = Configuration["StaticFiles:Headers:Pragma"];
                    context.Context.Response.Headers["Expires"] = Configuration["StaticFiles:Headers:Expires"];
                }
            });
            app.UseRouting(); 
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapHub<NotificationHub>(Const.NOTIFICATION_PATH);
                endpoints.MapControllers();
            });
            // Angular-SPA
            app.UseSpa(conf => conf.Options.DefaultPage = "/index.html");
        }
    }
}
