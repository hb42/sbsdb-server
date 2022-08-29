using System;
using System.Text.RegularExpressions;

namespace hb.SbsdbServer.Services; 

public class IpHelper {
    public static string NULL_MAC = "000000000000";
    
    private const string IpString =
        @"^\s*(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\.(\d|[01]?\d\d|2[0-4]\d|25[0-5])\s*$";

    private const string IpStringPart =
        @"^\s*(\d|[01]?\d\d|2[0-4]\d|25[0-5])(?:\.(\d|[01]?\d\d|2[0-4]\d|25[0-5]))?(?:\.(\d|[01]?\d\d|2[0-4]\d|25[0-5]))?(?:\.(\d|[01]?\d\d|2[0-4]\d|25[0-5]))?\s*$";

    /**
     * IP-String in die numerische Darstellung umrechnen
     *
     * @param ip
     */
    public static uint GetIp(string ip) {
        return GetIpNumber(ip, IpString);
    }

    private static uint GetIpNumber(string ip, string regex) {
        uint result = 0;
        var ips = Regex.Match(ip, regex);
        if (ips.Success) {
            int pow = 0;
            for (var i = 4; i > 0; i--) {
                result += Convert.ToUInt32(ips.Groups[i].Value) * Convert.ToUInt32(Math.Pow(256, pow++));
            }
        }
        return result;
    }
    
   /**
    * IP aus 32bit-Int in Stringdarstellung umrechnen
    *
    * @param ip
    */
    public static string GetIpString(uint ip) {
        return GetPartialIpString(ip, 4);
    }

   /**
    * Teil der IP-Adresse in Stringdarstellung umwandeln
    *
    * @param ip
    * @param bytes
    */
    public static string GetPartialIpString(uint ip, uint bytes) {
        string rc = "";
        for (var i = bytes; i > 0; i--) {
            uint ipbyte = ip & 0xff;
            ip >>= 8;  // FIXME sollte so funktionieren, da ip uint ist,
                       //       mit C# v11 ist auch >>> als unsigned shift right moeglich
                       //       => aendern nach Umstellung auf .NET 7
            rc = rc.Length > 0 ? $"{ipbyte}.{rc}" : $"{ipbyte}";
        }
        return rc;
    }

   /**
    * Netmask als 32bit-Integer
    *
    * @param netmask
    */
    public static uint GetNetmask(uint netmask) {
       if (netmask <= 32) {
            uint host = 32 - netmask;
            uint low = Convert.ToUInt32(Math.Pow(2, host) - 1);
            return 0xffff_ffff ^ low;
       }
       return netmask;
   }

   /**
    * Signifikante Bits der Netmask
    * (z.B. 255.255.255.0 -> 24)
    * Fuer eine ungueltige Netmask wird 0 geliefert.
    *
    * @param netmask
    */
    public static uint GetNetmaskBits(uint netmask) {
        uint bits = GetHostBits(netmask);
        return 32 - bits;
    }

   /**
    * Signifikante Bits des Hostteils der Netmask
    * (32 - NetmaskBits)
    * Fuer eine ungueltige Netmask wird 0 geliefert.
    *
    * @param netmask
    */
    public static uint GetHostBits(uint netmask) {
        if (netmask <= 32) {
            return 32 - netmask;
        } else {
            uint bits = Convert.ToUInt32(Math.Log2(~netmask + 1));
            return bits;
        }
    }

   /**
    * Bytes fuer den Host-Teil
    * (fuer netmasks lt 24)
    *
    * @param netmask
    */
    public static uint GetHostBytes(uint netmask) {
        uint bits = GetHostBits(netmask);
        return Convert.ToUInt32(decimal.Truncate((bits - 1) / 8) + 1);
    }

   /**
    * Minimaler Wert fuer den Hostteil der IP-Adresse
    * (fuer die erste gueltige Adresse 1 addieren)
    *
    * @param net
    * @param netmask
    */ 
    public static uint GetHostIpMin(uint net, uint netmask) {
        uint bytes = GetHostBytes(netmask);
        uint mask = Convert.ToUInt32(Math.Pow(256, bytes) - 1);
        net &= GetNetmask(netmask);
        return net & mask;
    }

   /**
    * Maximaler Wert fuer den Hostteil der IP-Adresse
    * (fuer die letzte gueltige Adresse 1 subtrahieren)
    *
    * @param net
    * @param netmask
    */
    public static uint GetHostIpMax(uint net, uint netmask) {
        uint hostbits = GetHostBits(netmask);
        uint min = GetHostIpMin(net, netmask);
        return  Convert.ToUInt32(Math.Pow(2, hostbits)) - 1 + min;
   }

   /**
   * Minimaler Wert fuer den Hostteil der IP-Adresse als String
   *
   * @param net
   * @param netmask
   */
    public static string GetHostIpMinString(uint net, uint netmask) {
        return GetPartialIpString(GetHostIpMin(net, netmask) + 1, GetHostBytes(netmask));
    }

   /**
   * Maximaler Wert fuer den Hostteil der IP-Adresse als String
   *
   * @param net
   * @param netmask
   */
    public static string GetHostIpMaxString(uint net, uint netmask) {
        return GetPartialIpString(GetHostIpMax(net, netmask) - 1, GetHostBytes(netmask));
   }

   /**
    * Host-Teil der Adresse als int
    *  z.B. 5.77.42.120/24  -> 120 (Netz: 5.77.42.0)
    *       5.77.200.45/25  -> 25  (Netz: 5.77.200.0)
    *       5.77.200.129/25 -> 1   (Netz: 5.77.200.128)
    *  => die vollstaendige Adresse ist die Addition von Netz- und Host-Teil
    *
    * @param host
    * @param netmask
    */
    public static uint GetHostIp(uint host, uint netmask) {
        netmask = GetNetmask(netmask); // falls nur die Bit-Zahl geliefert wird
        return host & ~netmask;
   }


}
