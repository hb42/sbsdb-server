﻿using System.Collections.Generic;
using hb.Common.Version;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class TestController : AbstractControllerBase<TestController> {
        private readonly TestService _testService;

        private readonly VersionResource _version;

        public TestController(VersionResource ver, TestService ts) {
            _version = ver;
            _testService = ts;
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

            return _version.Package();
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
