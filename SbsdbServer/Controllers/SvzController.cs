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
        private readonly ISvzRepository _svzRepository;
        private readonly AuthorizationHelper _auth;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<SvzController> _log;

        public SvzController(ISvzRepository svzRepo, AuthorizationHelper auth, IHubContext<NotificationHub> hub, ILogger<SvzController> log) {
            _svzRepository = svzRepo;
            _auth = auth;
            _log = log;
            _hub = hub;
        }
        
        // --- Adresse ---
        
        // TODO ViewModel.adresse fehlt
        // [HttpGet] 
        // [ActionName("adresse/all")]
        // public ActionResult<List<Adr>> Adressen() {
        //     return _svzRepository.GetAdressen();
        // }
        
        // --- ApKategorie ---
        
        [HttpGet] 
        [ActionName("apkategorie/all")]
        public ActionResult<List<ApKategorie>> ApKat() {
            return _svzRepository.GetApKat();
        }

        // --- ApTyp ---
        
        [HttpGet] 
        [ActionName("aptyp/all")]
        public ActionResult<List<ApTyp>> ApTypes() {
            return _svzRepository.GetApTypes();
        }

        [HttpPost]
        [ActionName("aptyp/change")]
        public ActionResult<List<ApTyp>> ChangeAptyp([FromBody] EditAptypTransport chg) {
            if (_auth.IsAdmin(User)) {
                var result =_svzRepository.ChangeAptyp(chg);
                if (result != null) {
                    // Aenderungen an alle Clients senden  
                    _hub.Clients.All.SendAsync(NotificationHub.AptypChangeEvent, chg);
                }
                return Ok();
            }
            return StatusCode(401);
        }

        // --- ExtProg ---
        
        [HttpGet]
        [ActionName("extprog/all")]
        public ActionResult<List<ExtProg>> Extprogs() {
            return _svzRepository.GetExtprog();
        }
        
        [HttpPost]
        [ActionName("extprog/change")]
        public ActionResult<List<ExtProg>> ChangeExtprog([FromBody] EditExtprogTransport chg) {
            if (_auth.IsAdmin(User)) {
                if (_svzRepository.ChangeExtprog(chg)) {
                    // Aenderungen an alle Clients senden  
                    _hub.Clients.All.SendAsync(NotificationHub.ExtProgChangeEvent, null);
                }
                return Ok();
            }
            return StatusCode(401);
        }
        
        // --- HwTyp ---
        
        [HttpGet] 
        [ActionName("hwtyp/all")]
        public ActionResult<List<HwTyp>> HwTypes() {
            return _svzRepository.GetHwTypes();
        }

        // --- Oe ---
        
        // TODO ViewModel.oe fehlt
        // [HttpGet] 
        // [ActionName("oe/all")]
        // public ActionResult<List<Oe>> Oes() {
        //     return _svzRepository.GetOes();
        // }

        // --- TagTyp ---
        
        [HttpGet] 
        [ActionName("tagtyp/all")]
        public ActionResult<List<TagTyp>> TagTypes() {
            return _svzRepository.GetTagTypes();
        }
        
        // --- Vlan ---
        
        [HttpGet] 
        [ActionName("vlan/all")]
        public ActionResult<List<Vlan>> Vlans() {
            return _svzRepository.GetVlans();
        }

    }
    
}
