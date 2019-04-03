using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace hb.SbsdbServer {
  public class Program {

    protected const string NLOG_CONF = "./config/nlog.config";
    // Die folgende Datei enthaelt Internas, wie z.B. Passwoeerter.
    // Nicht auf Github replizieren! (.gitignore -> **/config/config_*.json)
    protected const string PRIVATE_CONF = "./config/config_internal.json";

    public static void Main(string[] args) {
      // -- Nlog als Logging-Framework einbauen
      // NLog: setup the logger first to catch all errors
      var logger = NLogBuilder.ConfigureNLog(NLOG_CONF).GetCurrentClassLogger();
      try {
        logger.Debug("init main");
          // -- Host starten
        CreateWebHostBuilder(args).Build().Run();
      } catch (Exception ex) {
        //NLog: catch setup errors
        logger.Error(ex, "Stopped program because of exception");
        throw;
      } finally {
        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        NLog.LogManager.Shutdown();
      }
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
      return WebHost.CreateDefaultBuilder(args)
                    .CaptureStartupErrors(true)
                    .UseSetting("detailedErrors", "true")

                     //-- zusaetzliche config-Datei laden 
                     // -> wird in IConfguration integriert  
                    .ConfigureAppConfiguration((hostingContext, config) => {
                       config.SetBasePath(Directory.GetCurrentDirectory());
                       config.AddJsonFile(PRIVATE_CONF, optional: false, reloadOnChange: false);
                     })
                    .UseStartup<Startup>()
                     // -- NLog
                    .ConfigureLogging(logging => {
                       logging.ClearProviders();
                       // -- LogLevel werden in nlog.config definiert
                       // -> auch in appsettings.json auf max setzen
                       logging.SetMinimumLevel(LogLevel.Trace);
                     })
                    .UseNLog();
    }
  }
}
