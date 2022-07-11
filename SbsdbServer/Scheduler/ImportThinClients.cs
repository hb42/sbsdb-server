using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using hb.SbsdbServer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;

namespace hb.SbsdbServer.Scheduler; 

/**
 * Thin Client IPs importieren
 *
 * Wird vom Quartz-Scheduler gestartet. Config und Start in Startup.cs,
 * die Konfiguration ist in config_internal.json abgelegt ("ThinClientIPs").
 *
 * Voraussetzungen: Damit das externe Script ausreichende Recht fuer "net use" hat
 *                  muss der Anwendugnspool unter dem Netzwerkdienst ausgefuehrt werden:
 *                     Anwendungspool -> Erweiterte Einstellungen -> Identität = Networkservice
 *                  Ausserdem muss die Anwendung durchlaufen:
 *                     Anwendungspool -> Erweiterte Einstellungen -> Startmodus = AlwaysRunning
 */
public class ImportThinClients: IJob {
    private readonly SbsdbContext _dbContext;
    private readonly ILogger<ImportThinClients> _log;
    
    private readonly string _copyScript;
    private readonly string _filePath;
    private readonly string _tcRegex; // = @"^\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s*([\w]*)";

    public ImportThinClients(SbsdbContext context, IConfiguration configuration, ILogger<ImportThinClients> log) {
        _dbContext = context;
        _log = log;
        _copyScript = @".\config\" + configuration.GetValue<string>("ThinClientIPs:copyScript");
        _filePath = @".\config\" + configuration.GetValue<string>("ThinClientIPs:localPath");
        _tcRegex = configuration.GetValue<string>("ThinClientIPs:regex");
    }
    
    public Task Execute(IJobExecutionContext context) {

        if (fetchFiles()) {
            // TODO catch div io excep
            foreach (var fileName in Directory.GetFiles(_filePath, "*")) {
                var text = File.ReadAllText(fileName);
                var tc = Regex.Match(text, _tcRegex);
                if (tc.Success) {
                    string ip = tc.Groups[1].Value;
                    string host = tc.Groups[2].Value;
                    _log.LogDebug("found " + ip + " = " + host);
                    try {
                        var ap = _dbContext.Ap
                            .Include(a => a.Hw)
                            .ThenInclude(h => h.Mac)
                            .ThenInclude(m => m.Vlan)
                            .First(ap => ap.Apname.ToUpper() == host.ToUpper());
                        bool pri = false;
                        foreach (var hw in ap.Hw) {
                            if (hw.Pri) {
                                pri = true;
                                if (hw.Mac.Count >= 0) {
                                    var mac = hw.Mac.First();
                                    var ip4 = mac.Ip;
                                    var ipLong = mac.Ip + mac.Vlan.Ip;
                                    // TODO IP-Helper
                                    _log.LogDebug($"  4.Oktett: {ip4}");
                                } else {
                                    _log.LogDebug("  no IPs");
                                }
                            } 
                        }
                        if (!pri) {
                            _log.LogDebug("  no pri HW");    
                        }
                    } catch {
                      _log.LogDebug($"  ERROR: {host} not found");  
                    }
                    /*
                     * - get vlan list
                     * - foreach file
                     * - if valid
                     * -   find AP (AP->HW->MAC->VLAN, aptyp == TC)
                     * -   if AP check IP
                     * -     if !IP change IP/ new IP
                     *              
                     */
                }
            }

            _log.LogDebug("Import Thin Clients-Job executed");
            return Task.CompletedTask;
        }
        else {
            _log.LogError("Fehler beim Kopieren der TFTP-Dateien.");
            return Task.FromCanceled(CancellationToken.None);
        }
    }

    /**
     * Dateien von TFTP-Server holen
     *
     * Direkter Zugriff auf eine NAS-Share funktioniert nicht (jedenfalls nicht ohne grossen Aufwand).
     * Was funktioniert, ist er Zugriff ueber ein Script mittels "net use". Deshalb werden die
     * benoetigten Dateien per Script in ein Verzeichnis unterhalb der Webanwendung kopiert und
     * von da eingelesen.
     *
     * Damit das klappt muss der Anwendungspool unter "Netzwerkdienst" laufen.
     */
    private bool fetchFiles() {
        bool rc;
        using (var process = new Process()) {
            process.StartInfo.FileName = @"powershell.exe";
            process.StartInfo.Arguments = _copyScript;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.OutputDataReceived += (sender, data) => _log.LogDebug(data.Data);
            process.ErrorDataReceived += (sender, data) => _log.LogError(data.Data);
            _log.LogDebug("Copy TFTP files starting");
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            rc = process.WaitForExit(1000 * 120);     // wait up to 2 minutes
            var result = rc ? "OK" : "ERROR";
            _log.LogDebug($"Copy TFTP files ends with {result}");
        }
        return rc;
    }
}
