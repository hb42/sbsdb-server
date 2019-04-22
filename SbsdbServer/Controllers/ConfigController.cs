using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace hb.SbsdbServer.Controllers {
    public class ConfigController : AbstractControllerBase<ConfigController> {

        private readonly IConfigService _configService;
        
        public ConfigController(IConfigService config) {
            _configService = config;
        }
        
        [HttpGet]
        public object Version() {
            return _configService.GetVersion();
        }
    }
}
