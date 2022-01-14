using System.Collections.Generic;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {

    /**
    * Controller fuer alle Hilfstabellen
    */
    public class SvzController : AbstractControllerBase<SvzController> {
        private readonly IExtProgRepository _extProgRepository;
        private readonly AuthorizationHelper _auth;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<SvzController> _log;

        public SvzController(IExtProgRepository service, AuthorizationHelper auth, IHubContext<NotificationHub> hub, ILogger<SvzController> log) {
            _extProgRepository = service;
            _auth = auth;
            _log = log;
            _hub = hub;
        }
        
        // --- ExtProg ---
        
        [HttpGet]
        [ActionName("extprog/all")]
        public ActionResult<List<ExtProg>> All() {
            return _extProgRepository.GetAll();
        }
    }
}
