using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using hb.SbsdbServer.Model;
using hb.SbsdbServer.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Services; 

public class ExternalService: IExternalService {
    private readonly SbsdbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ExternalService> _log;

    private const string Lf = $"\n";
    
    public ExternalService(SbsdbContext context, IConfiguration configuration, ILogger<ExternalService> log) {
        _dbContext = context;
        _configuration = configuration;
        _log = log;
    }
    
    /**
     * Thin Client IPs importieren
     *
     */
    public string ImportThinClientIPs() {
        var tcRegex = _configuration.GetValue<string>("ThinClientIPs:regex");
        // Pfad zu den TC-Dateien, relativ zur Webapp
        var importPath = _configuration.GetValue<string>("ThinClientIPs:importPath");
        var result = "";
        
        var vlans = _dbContext.Vlan.ToList();
        try {
            foreach (var fileName in Directory.GetFiles(importPath, "*")) {
                var text = File.ReadAllText(fileName);
                var tc = Regex.Match(text, tcRegex);
                if (tc.Success) {
                    // Datei enthaelt IP + Hostname
                    string ip = tc.Groups[1].Value;
                    string host = tc.Groups[2].Value;
                    try {
                        // exception, falls kein Datensatz gefunden
                        var ap = _dbContext.Ap
                            .Include(a => a.Hw)
                            .ThenInclude(h => h.Mac)
                            .ThenInclude(m => m.Vlan)
                            .First(ap => ap.Apname.ToUpper() == host.ToUpper());
                        result += ChangeIp(ap, ip, vlans);
                    }
                    catch {
                        // Hostname nicht in DB
                        result += $"ERROR - {host} nicht in DB (evtl. Datei auf TFTP-Share loeschen)." + Lf;
                    }
                }
            }
        }
        catch (Exception e) {
            // IO-Fehler beim Dateizugriff
            var err = $"ERROR - Fehler beim Einlesen der TFTP-Dateien: {e.Message}";
            result += err + Lf;
            _log.LogError("{E}",err);
        }

        _log.LogDebug("Import Thin Clients-Job executed");
        return result;
    }

    /**
     * Logs des Thin-Client-Imports holen
     *
     */
    public string GetTcLogs() {
        var logfile = _configuration.GetValue<string>("ThinClientIPs:logfile");
        try {
            return File.ReadAllText(logfile);
        } catch(Exception e) {
            _log.LogError("Fehler beim Einlesen von {Log}: {Msg}", logfile, e.Message);
            return "-- kein Protokoll vorhanden --";
        }
    }
    
    private string ChangeIp(Ap ap, string ip, List<Vlan> vlans) {
        var result = "";
        bool pri = false;
        foreach (var hw in ap.Hw) {
            // primaere HW suchen
            if (hw.Pri) {
                pri = true;
                if (hw.Mac.Count >= 0) { 
                    // erste gefundene IP-Adresse verwenden
                    // Falls der Thin mehreren MACs haette, koennte hier nicht entschieden
                    // werden, welche MAC betroffen ist (das kann nur klappen, wenn die
                    // Auswertung auf dem TC auch die Schnittstelle + MAC liefert)
                    var mac = hw.Mac.First();
                    var ipOld = mac.Ip + mac.Vlan?.Ip ?? 0;
                    var ipNew = IpHelper.GetIp(ip);
                    if (ipOld != ipNew) {
                        // IP hat sich geaendert
                        foreach (var vlan in vlans) {
                            // passendes VLAN suchen
                            var maxIp = Convert.ToUInt32(Math.Pow(2, IpHelper.GetHostBits((uint)vlan.Netmask))) - 1;
                            if (ipNew > vlan.Ip && ipNew - vlan.Ip < maxIp) { 
                                // neue IP eintragen
                                mac.Ip = ipNew - vlan.Ip;
                                mac.VlanId = vlan.Id;
                                _dbContext.Mac.Update(mac);
                                _dbContext.SaveChanges();
                                result += $"Success - {ap.Apname}: aendere IP von {IpHelper.GetIpString((uint)ipOld)} zu {ip}" + Lf;
                            }
                        }
                    }
                    else {
                        // alles gut, nichts zu tun
                    }
                } else {
                    result += $"ERROR - {ap.Apname} hat keine MAC-Adresse." + Lf;
                }
            }
        }

        if (!pri) {
            result += $"ERROR - {ap.Apname} hat keine primaere Hardware." + Lf;
        }

        return result;
    }
}
