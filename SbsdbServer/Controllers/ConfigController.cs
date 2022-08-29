using System.IO;
using System.Text;
using System.Threading.Tasks;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class ConfigController : AbstractControllerBase<ConfigController> {

        private readonly IConfigService _configService;
        
        public ConfigController(IConfigService config) {
            _configService = config;
        }
        
        [HttpGet]
        public ActionResult<object> Version() {
            return Ok(_configService.GetVersion());
        }

        [HttpGet("{config}")]
        public ActionResult<string> Get(string config) {
            return Ok(_configService.GetConfig(config));
        }

        [HttpPost("{config}")]
        public ActionResult<string> Set(string config) {
            /* Die POST-Data sollen explizit nicht als JSON interpretiert werden, sondern 
             * als String. Den Request mit text/plain zu senden hilft nichts, weil das hier
             * nicht erkannt wird (-> 415 unknown media type).
             * Statt dessen als JSON senden, aber hier nur die RAW-Daten auslesen. Dafuer duerfen
             * die Daten nicht als Parameter ([FromBody] string value) uebernommen werden.
             */
            Task<string> value;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8)) {  
                value = reader.ReadToEndAsync();
            }
            Log.LogDebug("POST config={Conf} /value={Val}", config, value.ToString());
            return Ok(_configService.SetConfig(config, value.Result));
        }

        [HttpDelete("{config}")]
        public ActionResult<string> Del(string config) {
            _configService.DelConfig(config);
            return Ok();
        }
    }
}
