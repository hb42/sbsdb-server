using System.Collections.Generic;
using hb.Common.Version;
using hb.SbsdbServer.Services;
using hb.SbsdbServer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {

  public class TestController : AbstractControllerBase<TestController> {

    private readonly VersionResource version;
    private readonly TestService testService;

    public TestController(VersionResource ver, TestService ts) {
      version = ver;
      testService = ts;
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
    public ActionResult<IEnumerable<Ap>> Get() {
      //user = configuration.GetSection("Groups").GetValue<string>("admin");
      //if (!User.IsInRole(user)) {
      //  return StatusCode(403);
      //}

      LOG.LogDebug("start query");
      IEnumerable<Ap> aps = testService.GetAps("2W0");
      LOG.LogDebug("end query");

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
      LOG.LogDebug("GET ws/test/nr/" + id);

      //return StatusCode(404, new { Name = "whatever" });

      return "single value #" + id;

    }

    [HttpPost]
    public ActionResult<string> Post([FromBody] string value) {
      LOG.LogDebug("POST " + value);

      return Ok(value + "!");
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value) {
      LOG.LogDebug("PUT " + value);
    }

    [HttpDelete("{id}")]
    public void Delete(int id) {
      LOG.LogDebug("DELETE " + id);
    }

  }
}
