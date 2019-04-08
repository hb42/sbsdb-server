using hb.Common.Validation;
using hb.Common.Version;
using hb.SbsdbServer.Model;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.sbsdbv4.model;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace hb.SbsdbServer {
  public class Startup {

    private IConfiguration Configuration { get; }
    private ILogger<Startup> LOG;

    public Startup(IConfiguration configuration, ILogger<Startup> logger) {
      Configuration = configuration;
      LOG = logger;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddMvc()
              .AddJsonOptions(      // bei der Ausgabe von DB-Objekten Schleifen bei der JSON-Erstellung verhindern
                options => options.SerializerSettings.ReferenceLoopHandling =
                             Newtonsoft.Json.ReferenceLoopHandling.Ignore)
              // Validierungsfehler fangen
              .ConfigureApiBehaviorOptions(o => {
                o.InvalidModelStateResponseFactory = context => {
                  LOG.LogInformation("Validation Error");
                  return new ValidationProblemDetailsResult();
                };
              })
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      // NTLM via IIS
      services.AddAuthentication(IISDefaults.AuthenticationScheme);

      // connection strings holen
      string connStr;
      string connStrv4;
      if (Configuration.GetValue<string>("VRZKennung") != null) {  // SPK-Umgebung
        connStr = Configuration.GetConnectionString("sbsdb");
        connStrv4 = Configuration.GetConnectionString("sbsdbv4");
      } else {
        connStr = Configuration.GetConnectionString("sbsdbx");
        connStrv4 = Configuration.GetConnectionString("sbsdbv4x");
      }

       // alter Bestand - MySQL/EF
       // TODO kann raus, sobald alles auf neue Oracle-Struktur ueberfuehrt
      services.AddDbContextPool<Sbsdbv4Context>( 
               options => options
              //   .UseLazyLoadingProxies()   // sofern lazy loading gewuenscht
                 .UseMySql(connStrv4, 
                           mySqlOptions => {
                             mySqlOptions.ServerVersion(new Version(5, 5, 60), ServerType.MySql); 
                           }
                 )
     );

      // neuer Bestand - Oracle/EF
      services.AddDbContextPool<SbsdbContext>(
               options => options/*.UseLazyLoadingProxies() */  // sofern lazy loading gewuenscht
                 .UseOracle(connStr)
      );


      // Versionsinfos werden per .csproj gesetzt, hier aus Assembly auslesen
      VersionResource version = new VersionResource();
      services.AddSingleton<VersionResource>(version);

      // Services und Repositories
      services.AddTransient<TestService, TestService>();

      services.AddTransient<IUserService, UserService>();
      services.AddTransient<IUserRepository, UserRepository>();
      services.AddTransient<ITreeService, TreeService>();
      services.AddTransient<ITreeRepository, TreeRepository>();

      services.AddTransient<v4Migration, v4Migration>();

      LOG.LogInformation("Starting  " + version.ToString());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env) {

      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        //app.UseHsts();  // HTTP Strict Transport Security -> https
      }

      // create-table-commands fuer alle Entities die SbsdbContext verwaltet ins Log schreiben
      // TODO kann raus, wenn die Datenbankstruktur steht 
      using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                   .CreateScope()) {
        LOG.LogDebug(serviceScope.ServiceProvider.GetService<SbsdbContext>()
            .Database.GenerateCreateScript());
      }

      // Exceptions fangen und einheitlich zurueckgeben (Status 500)
      app.UseExceptionHandler(a => a.Run(async context => {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = feature.Error;
        var result = JsonConvert.SerializeObject((exception: exception.GetType().Name,
                                                  message: exception.Message,
                                                  stacktrace: exception.StackTrace));
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(result);
      }));

      //      app.UseHttpsRedirection();  // falls das mal auf https laeuft
      app.UseMvc();
      app.UseDefaultFiles();  // wg. index.html
      app.UseStaticFiles(new StaticFileOptions()); // -> wwwroot
    }
  }
}
