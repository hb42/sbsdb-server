using System;
using System.Runtime.InteropServices;
using hb.Common.Validation;
using hb.Common.Version;
using hb.SbsdbServer.Model;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.sbsdbv4.model;
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
#if TESTSYSTEM  // unterschiedliche Connection-Strings fuer verschiedene Systeme
            string connStr = Configuration.GetConnectionString("sbsdbxx");
            string connStrv4 = Configuration.GetConnectionString("sbsdbv4xx");
#else
            string connStr = Configuration.GetConnectionString("sbsdb");
            string connStrv4 = Configuration.GetConnectionString("sbsdbv4");
#endif
            // alter Bestand - MySQL/EF
            // TODO kann raus, sobald alles auf neue Oracle-Struktur ueberfuehrt
            // Replace with your server version and type.
            // Use 'MariaDbServerVersion' for MariaDB.
            // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
            // For common usages, see pull request #1233.
            // var serverVersion = ServerVersion.AutoDetect(connStrv4); //new MySqlServerVersion(new Version(5, 5, 60));
            var serverVersion = new MariaDbServerVersion("10.0.0");
            services.AddDbContextPool<Sbsdbv4Context>(
                    dbContextOptions => dbContextOptions
                        .UseMySql(connStrv4, serverVersion)
                        .EnableSensitiveDataLogging() // These two calls are optional but help
                        .EnableDetailedErrors()      // with debugging (remove for production).
                );

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
            services.AddTransient<IExtProgRepository, ExtProgRepository>();

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
                app.UsePathBase(new PathString(Const.BASE_URL)); 
            }

            app.UseHttpsRedirection();  // falls das mal auf https laeuft

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
            app.UseSpa(conf => conf.Options.DefaultPage = "/index.html"); // Angular-SPA
        }
    }
}
