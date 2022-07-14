using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using hb.Common.Version;
using hb.SbsdbServer.Model;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class TestController : AbstractControllerBase<TestController> {
        private readonly TestService _testService;
        private readonly VersionResource _version;
        private readonly SbsdbContext _dbContext;
        private readonly AuthorizationHelper _auth;
        
        public TestController(VersionResource ver, TestService ts, SbsdbContext context, AuthorizationHelper auth) {
            _version = ver;
            _testService = ts;
            _dbContext = context;
            _auth = auth;
            //user = configuration.GetSection("Groups").GetValue<string>("test");
        }


        // AuthorizeAttribute 
        //   [Authorize(Roles = Const.ROLE_USER + "," + Const.ROLE_ADMIN)]  // nur fuer Rolle(n) erlauben (mehrere: "role1,role2")
        // Problem mit Attributen ist, dass nur Konstante verwendet werden koennen:
        //     (Roles = "role1, role2")
        //   oder
        //     public const string role1 = "role1";
        //     -> (Roles = role1) 
        // (der Wert wird beim Kompilieren eingesetzt!)
        //
        // Wenn die verwendeten Rollen nicht im Programm als const deklariert werden sollen, bleibt
        // nur in der fn die Rollen zu pruefen 
        //     if (!User.IsInRole(role1) && !User.IsInRole(...)) {
        //       //return Forbid(); -> dafuer muesste AuthenticationScheme etc. definiert werden
        //       return StatusCode(403);  // forbidden
        //     }
        [HttpGet]
        [ActionName("all")]
        public ActionResult<IEnumerable<Arbeitsplatz>> Get() {
            //user = configuration.GetSection("Groups").GetValue<string>("admin");
            //if (!User.IsInRole(user)) {
            //  return StatusCode(403);
            //}

            Log.LogDebug("start query");
            var aps = _testService.GetAps("2W0");
            Log.LogDebug("end query");

            // NTLM-User
            /*
            try {
              LOG.LogDebug("User.Identity.AuthenticationType=" + User.Identity.AuthenticationType);
            } catch {
              LOG.LogDebug("error reading AuthentictionType");
            }
            LOG.LogDebug("User.Identity.Name" + User.Identity.Name);  // <domain>\<user>
            LOG.LogDebug("User Type=" + User.GetType().FullName);  //  System.Security.Principal.WindowsPrincipal
            LOG.LogDebug("User.Indentity Type=" + User.Identity.GetType().FullName);  // System.Security.Principal.WindowsIdentity
      
            var winident = User.Identity as System.Security.Principal.WindowsIdentity;
            foreach (var g in winident.Groups) {
              LOG.LogDebug("group " + g.Translate(typeof(System.Security.Principal.NTAccount)).Value);  // <domain>\<group>
            }
            */
            return Ok(aps);
        }

        //       [Authorize(Roles = Const.ROLE_ADMIN)]  // -> status 403 if !in role
        [HttpGet("{id}")]
        [ActionName("nr")]
        public ActionResult<string> Get(int id) {
            Log.LogDebug("GET ws/test/nr/" + id);

            //return StatusCode(404, new { Name = "whatever" });

            return "single value #" + id;
        }

        [HttpGet]
        [ActionName("test")]
        public ActionResult<object> GetTest() {

           // return _dbContext.Aptyp
           //      .AsEnumerable()
           //      .GroupJoin(_dbContext.Ap, 
           //          aptyp => aptyp.Id, 
           //          ap => ap.AptypId, 
           //          (typ, aps) => new {
           //              Id = typ.Id,
           //              Bez = typ.Bezeichnung,
           //              count = aps.Count()
           //          })
           //      .ToList();
           //
           
           Log.LogDebug($"TestService /ws/test/test");
           
           // IpHelper-Routinen testen
           
           string result = "";
           const string lf = $"\n";
           string ipStr = "5.77.203.129";
           string nm1Str = "255.255.255.0";
           uint nm2 = 25;
           uint nm3 = 23;

           var ip = IpHelper.GetIp(ipStr);
           var nm1long = IpHelper.GetIp(nm1Str);
           var ipRet = IpHelper.GetIpString(ip);
           var nm2long = IpHelper.GetNetmask(nm2);
           var nm1 = IpHelper.GetNetmaskBits(nm1long);
           var nm3long = IpHelper.GetNetmask(nm3);
           var nm2Str = IpHelper.GetIpString(nm2long);
           var nm3Str = IpHelper.GetIpString(nm3long);
           var bytes1 = IpHelper.GetHostBytes(nm1);
           var bytes2 = IpHelper.GetHostBytes(nm2);
           var bytes3 = IpHelper.GetHostBytes(nm3);
           var host1 = IpHelper.GetHostIp(ip, nm1);
           var host2 = IpHelper.GetHostIp(ip, nm2);
           var host3 = IpHelper.GetHostIp(ip, nm3);
           var min1 = IpHelper.GetHostIpMin(ip, nm1);
           var min2 = IpHelper.GetHostIpMin(ip, nm2);
           var min3 = IpHelper.GetHostIpMin(ip, nm3);
           var max1 = IpHelper.GetHostIpMax(ip, nm1);
           var max2 = IpHelper.GetHostIpMax(ip, nm2);
           var max3 = IpHelper.GetHostIpMax(ip, nm3);
           var host3Str = IpHelper.GetPartialIpString(ip, bytes3); 
           var min3Str = IpHelper.GetHostIpMinString(ip, nm3);
           var max3Str = IpHelper.GetHostIpMaxString(ip, nm3);
           var min2Str = IpHelper.GetHostIpMinString(ip, nm2);
           var max2Str = IpHelper.GetHostIpMaxString(ip, nm2);
           var min1Str = IpHelper.GetHostIpMinString(ip, nm1);
           var max1Str = IpHelper.GetHostIpMaxString(ip, nm1);

           result += $"IP: {ipStr} = {ip} = {ipRet}" + lf;
           result += $"Netmask 1: {nm1Str} = /{nm1} = {nm1long}" + lf;
           result += $"  host: {host1} bytes: {bytes1}" + lf;
           result += $"  minIP: {min1} = {min1Str} maxIP: {max1} = {max1Str}" + lf;
           result += $"Netmask 2: {nm2Str} = /{nm2} = {nm2long}" + lf;
           result += $"  host: {host2} bytes: {bytes2}" + lf;
           result += $"  minIP: {min2} = {min2Str} maxIP: {max2} = {max2Str}" + lf;
           result += $"Netmask 3: {nm3Str} = /{nm3} = {nm3long}" + lf;
           result += $"  host: {host3} bytes: {bytes3} partial: {host3Str}" + lf;
           result += $"  minIP: {min3} = {min3Str} maxIP: {max3} = {max3Str}" + lf;
           
           return result;
        }

        [HttpGet]
        [ActionName("migration")]
        public ActionResult<string> Migrate() {
            Log.LogDebug("v4 Migration");
            // Aktion laeuft zu lange => Client bekommt 502
            _testService.Migrate();
            return Ok("OK");
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] string value) {
            Log.LogDebug("POST " + value);

            return Ok(value + "!");
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
            Log.LogDebug("PUT " + value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id) {
            Log.LogDebug("DELETE " + id);
        }
    }
}
