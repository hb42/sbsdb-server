using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using hb.SbsdbServer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Services; 

public class ExternalService: IExternalService {
    private readonly SbsdbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ExternalService> _log;
    
    // private readonly string _tcRegex; // = @"^\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s*([\w]*)";

    public ExternalService(SbsdbContext context, IConfiguration configuration, ILogger<ExternalService> log) {
        _dbContext = context;
        _configuration = configuration;
        _log = log;
    }
    
    /**
     * Thin Client IPs importieren
     *
     * path = Pfad zu den TC-Dateien, relativ zur Webapp
     */
    public string ImportThinClientIPs() {
        var tcRegex = _configuration.GetValue<string>("ThinClientIPs:regex");
        var importPath = _configuration.GetValue<string>("ThinClientIPs:importPath");
        var result = "";
        const string lf = $"\n";
        // TODO catch div io excep
        // TODO Fehlerprotokoll -> wie behandeln?
        foreach (var fileName in Directory.GetFiles(importPath, "*")) {
            var text = File.ReadAllText(fileName);
            var tc = Regex.Match(text, tcRegex);
            if (tc.Success) {
                string ip = tc.Groups[1].Value;
                string host = tc.Groups[2].Value;
                result += "found " + ip + " = " + host + lf;
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
                            if (hw.Mac.Count >= 0) {  // TODO alle MACs abgleichen?
                                var mac = hw.Mac.First();
                                var ip4 = mac.Ip;
                                var ipLong = mac.Ip + mac.Vlan.Ip;
                                // TODO IP-Helper
                                var ipnum = IpHelper.GetIp(ip);
                                if (ipLong != ipnum) {
                                    // TODO change IP (+ ggf. VLAN)
                                    //      foreach vlan if ipnum > vlan.ip
                                    //                      && vlan.ip - ipnum < getHostIpMax(vlan.ip, vlan.netmask)
                                    //                      && vlan.ip - ipnum > getHostIpMin(vlan.ip, vlan.netmask)
                                    //                      then new.vlan = vlan, new.ip = vlan.ip - ipnum
                                    result += $"  not equal: {ipLong} != {ipnum}" + lf;
                                } else {
                                    // alles gut, nichts zu tun
                                    result += $"  equal" + lf;
                                }
                                result += $"  4.Oktett: {ip4}" + lf;
                            } else {
                                // TODO HW aber keine MAC -> mit MAC "00.." anlegen || als Fehler protokollieren
                                result += "  no IPs" + lf;
                            }
                        }
                    }

                    if (!pri) {
                        // TODO no pri -> als Fehler protokollieren
                        result += "  no pri HW" + lf;
                    }
                } catch {
                    // TODO file muesste auf NAS geloescht werden ??
                    result += $"  ERROR: {host} not found" + lf;
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
        return result;
    }
    
    /*
     * Dateien von TFTP-Server holen
     *
     * Direkter Zugriff auf eine NAS-Share funktioniert nicht (jedenfalls nicht ohne grossen Aufwand).
     * Was funktioniert, ist er Zugriff ueber ein Script mittels "net use". Deshalb werden die
     * benoetigten Dateien per Script in ein Verzeichnis unterhalb der Webanwendung kopiert und
     * von da eingelesen.
     *
     * Damit das klappt muss der Anwendungspool unter "Netzwerkdienst" laufen.
     */
    // private bool fetchFiles() {
    //     bool rc;
    //     using (var process = new Process()) {
    //         process.StartInfo.FileName = @"powershell.exe";
    //         process.StartInfo.Arguments = _copyScript;
    //         process.StartInfo.CreateNoWindow = true;
    //         process.StartInfo.UseShellExecute = false;
    //         process.StartInfo.RedirectStandardOutput = true;
    //         process.StartInfo.RedirectStandardError = true;
    //
    //         process.OutputDataReceived += (sender, data) => _log.LogDebug(data.Data);
    //         process.ErrorDataReceived += (sender, data) => _log.LogError(data.Data);
    //         _log.LogDebug("Copy TFTP files starting");
    //         process.Start();
    //         process.BeginOutputReadLine();
    //         process.BeginErrorReadLine();
    //         rc = process.WaitForExit(1000 * 120);     // wait up to 2 minutes
    //         var result = rc ? "OK" : "ERROR";
    //         _log.LogDebug($"Copy TFTP files ends with {result}");
    //     }
    //     return rc;
    // }
}
