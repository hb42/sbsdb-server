using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Services {
    public class AuthorizationHelper {
      private readonly IConfiguration _configuration;
      
        public AuthorizationHelper(IConfiguration configuration) {
            _configuration = configuration;
        }

        /**
         * User-ID aus dem Principal auslesen (beruecksichtig domain\user und user@domain)
         * 
         * Falls der Principal undefiniert ist, wird das eine Exception ausloesen.
         * Das ist beabsichtigt, denn wenn die Anwendung keinen User finden kann,
         * ist etwas grundlegend falsch gelaufen.
         */
        public static string GetUserId(ClaimsPrincipal user) {
            var name = user.Identity.Name.ToUpper();
            string uid;
            if (name.Contains(@"\")) {
                uid = name.Split(@"\")[Index.FromEnd(1)];
            } else if (name.Contains("@")) {
                uid = name.Split("@")[0];
            }
            else {
                uid = name;
            }
            return uid;
        }
        
        /**
         * Hat der Beutzer Adminrechte?
         */
        public bool IsAdmin(ClaimsPrincipal user) {
#if TESTSYSTEM
            // Testsystem: die Werte sind hier fest eingetragen, bei technischen Aenderungen ggf. anpassen
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                // Win10 + IIS kann die SID aus dem Kerberos-Ticket nicht in
                // den Gruppennamen ueebersetzen
                // (SID ermittelen: PS> Get-LocalGroup | select name,sid)
                return user.IsInRole("S-1-5-21-2177293706-942526630-1309965019-1013");
            }
            else {
                // Docker-Linux-Images: (1) Samba als AD-DC, (2) ASP.NET Core-Runtime + App
                // der Samba-KDC liefert im Kerberos-Ticket keine Gruppen, daher mit
                // mehreren Benutzern arbeiten (Ticket wird auf der aufrufenden Maschine
                // per kinit <user>@<domain> geholt).
                return "SBSDBADM" == GetUserId(user);
            }
#else
            // AD: hier sind keine Verrenkungen noetig
            return user.IsInRole(_configuration["AdminRole"]);
#endif
        }

        // /**
        //  * Hat der Benutzer Adminrechte?
        //  * Wird indirekt 
        //  */
        // public bool IsAdmin(AuthorizationHandlerContext ctx) {
        //     DefaultHttpContext res = ctx.Resource as DefaultHttpContext;
        //     Console.WriteLine("ctx.resource.user.id.name: " + res.User.Identity?.Name);
        //     Console.WriteLine("Identity.authenticated: " + ctx.User.Identity.IsAuthenticated);
        //
        //     if (!res.User.Identity.IsAuthenticated) {
        //         Console.WriteLine(" null");
        //         return false;
        //     } 
        //     
        //     return IsAdmin(res.User);
        // }
    }
}
