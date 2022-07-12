using hb.Common.Version;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers; 

public class ExternalController : AbstractControllerBase<ExternalController> {
    private readonly IExternalService _service;
    private readonly VersionResource _version;
    private readonly AuthorizationHelper _auth;

    public ExternalController(IExternalService ext, VersionResource ver, AuthorizationHelper auth) {
        _version = ver;
        _auth = auth;
        _service = ext;
    }

    /**
     * Die IPs der Thin Clients importieren
     *
     * Die IP-Adresen werden beim Start der TCs ausgelesen und via TFTP auf einer
     * Share auf der NAS abgelegt. Ein Scheduler-Job (-> cron/ImportThinClientIPs.ps1)
     * holt die Dateien in ein Verzeicnis unter dem Webapp-Root (der Pfad steht in
     * config_internal.json -> ThinClientIPs.importPath).
     * Anschliessend ruft der Scheduler-Job diesen Endpunkt auf, der die Verarbeitung
     * der Dateien startet.
     *
     * Voraussetzungen: - der Job laeuft unter SYSTEM (wird hier abgeprueft) und fuer den Job
     *                    muss "mit hoechsten Berechtigungen ausfuehren" eingestellt sein.
     *                  - Als Format der Dateien von den TCs wird "<IP-Adresse> <Hostname>"
     *                    am Anfang der Datei erwartet.
     */
    [HttpGet]
    [ActionName("importtcips")]
    public ActionResult<string> ImportThinClientIPs() {
        var u = AuthorizationHelper.GetUserId(User);
        if (u == "SYSTEM") {
            return _service.ImportThinClientIPs();
        } else {
            return StatusCode(401);
        }
    }

}
