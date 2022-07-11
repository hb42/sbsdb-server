using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using hb.SbsdbServer.Model;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;

namespace hb.SbsdbServer.Scheduler; 

/**
 * Thin Client IPs importieren
 *
 * Wird vom Quartz-Scheduler gestartet. Config und Start in Startup.cs, der CRON-String
 * fuer den Timer ist in config_internal.json abgelegt ("ThinClientJob").
 */
public class ImportThinClients: IJob {
    private readonly SbsdbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ImportThinClients> _log;
    
    private readonly string tcRegex = @"^\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s*([\w]*)"; 

    public ImportThinClients(SbsdbContext context, IConfiguration configuration, ILogger<ImportThinClients> log) {
        _dbContext = context;
        _configuration = configuration;
        _log = log;
    }
    
    public Task Execute(IJobExecutionContext context) {
        // @"smb:\\tftp:b2gqQaSgGE@\\e077naslif.v998dpve.v998.intern\tftp"
        // /docker/vol/jboss/tftp cifs username=tftp,password=b2gqQaSgGE 0 0
        
        // string tcPath = _configuration["ThinClientPath"];
        // string authtype = _configuration["ThinClientAuthType"];
        // _log.LogDebug("TC-PATH=" + tcPath);
        
        // NetworkCredential theNetworkCredential = new NetworkCredential(@"tftp", "b2gqQaSgGE", "workgroup");
        // CredentialCache theNetCache = new CredentialCache();
        // theNetCache.Add(new Uri(@"\\e077naslif.v998dpve.v998.intern"), authtype, theNetworkCredential);
        // string[] theFolders = Directory.GetDirectories(@"\\e077naslif.v998dpve.v998.intern\tftp");

        // TODO test external
        using (var process = new Process()) {
            // process.StartInfo.FileName = @".\HelloWorld\bin\Debug\helloworld.exe"; // relative path. absolute path works too.
            // process.StartInfo.Arguments = $"{id}";
            process.StartInfo.FileName = @"cmd.exe";
            process.StartInfo.Arguments = @"/c dir";      // print the current working directory information
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.OutputDataReceived += (sender, data) => Console.WriteLine(data.Data);
            process.ErrorDataReceived += (sender, data) => Console.WriteLine(data.Data);
            Console.WriteLine("starting");
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            var exited = process.WaitForExit(1000 * 10);     // (optional) wait up to 10 seconds
            Console.WriteLine($"exit {exited}");
        }
        
        string tcPath = @".\config\tftp";
        // TODO catch div io excep
        foreach (string fileName in Directory.GetFiles(tcPath, "*")) {
            string text = File.ReadAllText(fileName);
            var tc = Regex.Match(text, tcRegex);
            if (tc.Success) {
                string ip = tc.Groups[1].Value;
                string host = tc.Groups[2].Value;
                _log.LogDebug("found " + ip + " = " + host);
            }
        }
        _log.LogDebug("Import Thin Clients-Job executed");
        return Task.CompletedTask;
    }
}
