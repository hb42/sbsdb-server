using System.Collections.Generic;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class ApController : AbstractControllerBase<ApController> {
        private readonly IApService _apService;
        private readonly IApRepository _apRepo;
        private readonly AuthorizationHelper _auth;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<ApController> _log;

        public ApController(
            IApService service, 
            IApRepository repo, 
            AuthorizationHelper auth, 
            IHubContext<NotificationHub> hub, 
            ILogger<ApController> log
            ) {
            _apService = service;
            _apRepo = repo;
            _auth = auth;
            _hub = hub;
            _log = log;
        }

        [HttpGet]
        public ActionResult<List<Arbeitsplatz>> All() {
            return _apService.GetAll();
        }

        [HttpGet("{page}/{pagesize}")]
        public ActionResult<List<Arbeitsplatz>> Page(int page, int pagesize) {
            return _apService.GetPage(page, pagesize);
        }

        [HttpGet("{id}")]
        [ActionName("id")]
        public ActionResult<Arbeitsplatz> ApById(long id) {
            return _apService.GetAp(id);
        }

        [HttpGet("{search}")]
        [ActionName("search")]
        public ActionResult<List<Arbeitsplatz>> ApSearch(string search) {
            return _apService.GetAps(search);
        }

        [HttpPost]
        [ActionName("aps")]
        public ActionResult<List<Arbeitsplatz>> ApQuery([FromBody] string query) {
            return Ok();
        }

        [HttpPost]
        [ActionName("changeap")]
        public ActionResult<ApTransport> ApChange([FromBody] EditApTransport chg) {
            if (_auth.IsAdmin(User)) {
                var ap = _apService.ChangeAp(chg);
                if (ap != null) {
                    // Aenderungen an alle Clients senden  
                    _log.LogDebug("ApChange done, trigger notification");
                    _hub.Clients.All.SendAsync(NotificationHub.ApChangeEvent, ap);
                }
                return Ok();
            }
            return StatusCode(401);
        }
        
        [HttpPost]
        [ActionName("changeapmulti")]
        public ActionResult<ApTransport[]> ApChangeMulti([FromBody] EditApTransport[] chg) {
            if (_auth.IsAdmin(User)) {
                var ap = _apRepo.ChangeApMulti(chg);
                if (ap != null) {
                    // Aenderungen an alle Clients senden  
                    _log.LogDebug("ApChangeMulti done, trigger notification");
                    _hub.Clients.All.SendAsync(NotificationHub.ApChangeMultiEvent, ap);
                }
                return Ok();
            }
            return StatusCode(401);
        }

        [HttpPost]
        [ActionName("changeapmove")]
        public ActionResult<ApTransport[]> ApChangeMove([FromBody] EditApTransport[] chg) {
            if (_auth.IsAdmin(User)) {
                var ap = _apRepo.ChangeApMove(chg);
                if (ap != null) {
                    // Aenderungen an alle Clients senden  
                    _log.LogDebug("ApChangeMove done, trigger notification");
                    _hub.Clients.All.SendAsync(NotificationHub.ApChangeMoveEvent, ap);
                }
                return Ok();
            }
            return StatusCode(401);
        }

        [HttpPost]
        [ActionName("changeaptyp")]
        public ActionResult<ApTransport> ApTypChange([FromBody] ChangeAptypTransport chg) {
            if (_auth.IsAdmin(User)) {
                var ap = _apService.ChangeApTyp(chg);
                if (ap != null) {
                    // Aenderungen an alle Clients senden  
                    _log.LogDebug("ApTypChange done, trigger notification");
                    _hub.Clients.All.SendAsync(NotificationHub.ApChangeAptypEvent, ap);
                }
                return Ok();
            }
            return StatusCode(401);
        }

        [HttpGet]
        [ActionName("count")]
        public ActionResult<int> Count() {
            return _apService.GetCount();
        }
        
    }
}
