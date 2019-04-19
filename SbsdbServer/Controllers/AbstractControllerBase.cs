using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    [Route(Const.API_PATH)]
    [ApiController]
    [Authorize]
    public abstract class AbstractControllerBase<T> : ControllerBase where T : AbstractControllerBase<T> {
        // die wichtigsten Services mittels GetService() holen, 
        // dadurch kann in der konkreten Klasse weiterhin Di via c'tor genutzt werden
        // !! die Services sind c'tor der konkreten Klasse noch nicht verfuegbar !! 

        // Hilfsvariable
        private IConfiguration _config;
        private ILogger<T> _log;

        // NLog Logger
        protected ILogger<T> Log => _log ?? (_log = HttpContext.RequestServices.GetService<ILogger<T>>());

        // Config -> appsettings, etc.
        protected IConfiguration Configuration =>
            _config ?? (_config = HttpContext.RequestServices.GetService<IConfiguration>());
    }
}
