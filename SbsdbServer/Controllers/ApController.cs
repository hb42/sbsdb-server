using System;
using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class ApController : AbstractControllerBase<ApController> {
        private readonly IApService _apService;
        private readonly AuthorizationHelper _auth;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<ApController> _log;

        public ApController(IApService service, AuthorizationHelper auth, IHubContext<NotificationHub> hub, ILogger<ApController> log) {
            _apService = service;
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

        [HttpGet]
        [ActionName("count")]
        public ActionResult<int> Count() {
            return _apService.GetCount();
        }
        
        [HttpGet] 
        public ActionResult<List<TagTyp>> TagTypes() {
            return _apService.GetTagTypes();
        }
        
        [HttpGet] 
        public ActionResult<List<Vlan>> Vlans() {
            return _apService.GetVlans();
        }
        
        [HttpGet] 
        public ActionResult<List<ApTyp>> ApTypes() {
            return _apService.GetApTypes();
        }
        
    }
}
