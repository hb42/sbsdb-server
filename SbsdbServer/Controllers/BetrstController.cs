using System.Collections.Generic;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace hb.SbsdbServer.Controllers {
    public class BetrstController: AbstractControllerBase<BetrstController> {
        private readonly IBetrstService _betrstService;
        private readonly AuthorizationHelper _auth;
        private readonly IHubContext<NotificationHub> _hub;

        public BetrstController(IBetrstService service, AuthorizationHelper auth, IHubContext<NotificationHub> hub) {
            _betrstService = service;
            _auth = auth;
            _hub = hub;
        }

        [HttpGet]
        [ActionName("all")]
        public ActionResult<List<Betrst>> All() {
            return _betrstService.GetAll();
        }

        [HttpGet("{id}")]
        [ActionName("id")]
        public ActionResult<List<Betrst>> GetBstById(long id) {
            return _betrstService.GetBetrst(id);
        }

        [HttpPost]
        [ActionName("change")]
        public ActionResult<List<Betrst>> ChangeBst([FromBody] EditOeTransport chg) {
            if (_auth.IsAdmin(User)) {
                var result =_betrstService.ChangeBetrst(chg);
                if (result != null) {
                    // Aenderungen an alle Clients senden  
                    _hub.Clients.All.SendAsync(NotificationHub.OeChangeEvent, chg);
                }
                return Ok();
            }
            return StatusCode(401);
        }

        [HttpGet]
        [ActionName("adressen")]
        public ActionResult<List<Adresse>> AllAdressen() {
            return _betrstService.GetAdressen();
        }
        
        [HttpPost]
        [ActionName("chgadresse")]
        public ActionResult<List<Adresse>> ChangeAptyp([FromBody] EditAdresseTransport chg) {
            if (_auth.IsAdmin(User)) {
                var result =_betrstService.ChangeAdresse(chg);
                if (result != null) {
                    // Aenderungen an alle Clients senden  
                    _hub.Clients.All.SendAsync(NotificationHub.AdresseChangeEvent, chg);
                }
                return Ok();
            }
            return StatusCode(401);
        }
        
    }
}
