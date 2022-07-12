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
                            if (hw.Mac.Count >= 0) {
                                var mac = hw.Mac.First();
                                var ip4 = mac.Ip;
                                var ipLong = mac.Ip + mac.Vlan.Ip;
                                // TODO IP-Helper
                                result += $"  4.Oktett: {ip4}" + lf;
                            } else {
                                result += "  no IPs" + lf;
                            }
                        }
                    }

                    if (!pri) {
                        result += "  no pri HW" + lf;
                    }
                } catch {
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

// export class IpHelper {
//   public static NULL_MAC = "000000000000";
//
//   private static macString = /^(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})$/;
//   private static macCheck =
//     /^\s*([\da-fA-F]{2})[-:.]?([\da-fA-F]{2})[-:.]?([\da-fA-F]{2})[-:.]?([\da-fA-F]{2})[-:.]?([\da-fA-F]{2})[-:.]?([\da-fA-F]{2})\s*$/;
//   private static ipString =
//     /^\s*(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\s*$/;
//   private static ipStringPart =
//     /^\s*(\d|[01]?\d\d|2[0-4]\d|25[0-5])(?:\.(\d|[01]?\d\d|2[0-4]\d|25[0-5]))?(?:\.(\d|[01]?\d\d|2[0-4]\d|25[0-5]))?(?:\.(\d|[01]?\d\d|2[0-4]\d|25[0-5]))?\s*$/;
//
//   /**
//    * IP-String in die numerische Darstellung umrechnen
//    *
//    * @param ip
//    */
//   public static getIp(ip: string): number | null {
//     return this.getIpNumber(ip, this.ipString);
//   }
//
//   /**
//    * Teil-IP-String in die numerische Darstellung umrechnen
//    * (fuer Host-Teil, wenn netmask < 24,
//    *  z.B. "2.13" = 2*256 + 13 = 525)
//    *
//    * @param ip
//    */
//   public static getIpPartial(ip: string): number | null {
//     return this.getIpNumber(ip, this.ipStringPart);
//   }
//
//   private static getIpNumber(ip: string, regex: RegExp): number | null {
//     const nums: number[] = [];
//     const ips = regex.exec(ip);
//     if (ips) {
//       for (let i = 4; i > 0; i--) {
//         if (ips[i] !== undefined) {
//           nums.push(Number.parseInt(ips[i], 10));
//         }
//       }
//       return nums.reduce((prev, n, idx) => (prev += n * 256 ** idx), 0);
//     } else {
//       return null;
//     }
//   }
//
//   /**
//    * MAC-String aus DB fuer die Darstellung aufbereiten
//    *
//    * @param mac
//    */
//   public static getMacString(mac: string): string {
//     // kein match => Eingabe-String
//     return mac.replace(this.macString, "$1:$2:$3:$4:$5:$6").toUpperCase();
//   }
//
//   /**
//    * Eingegebene MAC-Adresse ueberpruefen und bei Erfolg ohne Sonderzeichen
//    * (':', '-', '.') zuruecklieferen. Im Fehlerfall wird null geliefert.
//    *
//    * @param mac
//    */
//   public static checkMacString(mac: string): string | null {
//     if (mac == null || mac === "0") {
//       return IpHelper.NULL_MAC;
//     }
//     if (this.macCheck.test(mac)) {
//       return mac.replace(this.macCheck, "$1$2$3$4$5$6").toUpperCase();
//     } else {
//       return null;
//     }
//   }
//
//   /**
//    * IP aus 32bit-Int in Stringdarstellung umrechnen
//    *
//    * @param ip
//    */
//   public static getIpString(ip: number): string {
//     return IpHelper.getPartialIpString(ip, 4);
//   }
//
//   /**
//    * Teil der IP-Adresse in Stringdarstellung umwandeln
//    *
//    * @param ip
//    * @param bytes
//    */
//   public static getPartialIpString(ip: number, bytes: number): string {
//     let rc: string;
//     for (let i = bytes; i > 0; i--) {
//       const ipbyte = ip & 0xff;
//       ip = ip >>> 8;
//       rc = rc ? `${ipbyte}.${rc}` : `${ipbyte}`;
//     }
//     return rc;
//   }
//
//   /**
//    * Netmask als 32bit-Integer
//    *
//    * @param netmask
//    */
//   public static getNetmask(netmask: number): number {
//     if (netmask <= 32) {
//       const host = 32 - netmask;
//       const low = host ** 2 - 1;
//       return 0xffff_ffff ^ low;
//     } else {
//       return netmask;
//     }
//   }
//
//   /**
//    * Signifikante Bits der Netmask
//    * (z.B. 255.255.255.0 -> 24)
//    * Fuer eine ungueltige Netmask wird 0 geliefert.
//    *
//    * @param netmask
//    */
//   public static getNetmaskBits(netmask: number): number {
//     const bits = IpHelper.getHostBits(netmask);
//     return bits ? 32 - bits : 0;
//   }
//
//   /**
//    * Signifikante Bits des Hostteils der Netmask
//    * (32 - NetmaskBits)
//    * Fuer eine ungueltige Netmask wird 0 geliefert.
//    *
//    * @param netmask
//    */
//   public static getHostBits(netmask: number): number {
//     if (netmask <= 32) {
//       return netmask;
//     } else {
//       const bits = Math.log2(~netmask + 1);
//       return Number.isInteger(bits) ? bits : 0;
//     }
//   }
//
//   /**
//    * Bytes fuer den Host-Teil
//    * (fuer netmasks < 24)
//    *
//    * @param netmask
//    */
//   public static getHostBytes(netmask: number): number {
//     const bits = IpHelper.getHostBits(netmask);
//     return Math.trunc((bits - 1) / 8) + 1;
//   }
//
//   /**
//    * Minimaler Wert fuer den Hostteil der IP-Adresse
//    * (fuer die erste gueltige Adresse 1 addieren)
//    *
//    * @param net
//    * @param netmask
//    */
//   public static getHostIpMin(net: number, netmask: number): number {
//     const bytes = IpHelper.getHostBytes(netmask);
//     const mask = 256 ** bytes - 1;
//     return net & mask;
//   }
//
//   /**
//    * Maximaler Wert fuer den Hostteil der IP-Adresse
//    * (fuer die letzte gueltige Adresse 1 subtrahieren)
//    *
//    * @param net
//    * @param netmask
//    */
//   public static getHostIpMax(net: number, netmask: number): number {
//     const hostbits = IpHelper.getHostBits(netmask);
//     const min = IpHelper.getHostIpMin(net, netmask);
//     return 2 ** hostbits - 1 + min;
//   }
//
//   /**
//    * Host-Teil der Adresse als int
//    *  z.B. 5.77.42.120/24  -> 120 (Netz: 5.77.42.0)
//    *       5.77.200.45/25  -> 25  (Netz: 5.77.200.0)
//    *       5.77.200.129/25 -> 1   (Netz: 5.77.200.128)
//    *  => die vollstaendige Adresse ist die Addition von Netz- und Host-Teil
//    *
//    * @param host
//    * @param netmask
//    */
//   public static getHostIp(host: number, netmask: number): number {
//     netmask = IpHelper.getNetmask(netmask); // falls nur die Bit-Zahl geliefert wird
//     return host & ~netmask;
//   }
// }

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
