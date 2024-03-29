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
        
        // --- ApKategorie ---
        
        [HttpGet] 
        [ActionName("apkategorie/all")]
        public ActionResult<List<ApKategorie>> ApKat() {
            return _svzRepository.GetApKat();
        }

        [HttpPost]
        [ActionName("apkategorie/change")]
        public ActionResult<List<ApKategorie>> ChangeApkategorie([FromBody] EditApkategorieTransport chg) {
            if (_auth.IsAdmin(User)) {
                var result =_svzRepository.ChangeApkategorie(chg);
                if (result != null) {
                    // Aenderungen an alle Clients senden  
                    _hub.Clients.All.SendAsync(NotificationHub.ApkategorieChangeEvent, chg);
                }
                return Ok();
            }
            return StatusCode(401);
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
        
        [HttpPost]
        [ActionName("hwtyp/change")]
        public ActionResult<List<HwTyp>> ChangeAptyp([FromBody] EditHwtypTransport chg) {
            if (_auth.IsAdmin(User)) {
                var result =_svzRepository.ChangeHwtyp(chg);
                if (result != null) {
                    // Aenderungen an alle Clients senden  
                    _hub.Clients.All.SendAsync(NotificationHub.HwtypChangeEvent, chg);
                }
                return Ok();
            }
            return StatusCode(401);
        }
        
        // --- TagTyp ---
        
        [HttpGet] 
        [ActionName("tagtyp/all")]
        public ActionResult<List<TagTyp>> TagTypes() {
            return _svzRepository.GetTagTypes();
        }
        
        [HttpPost]
        [ActionName("tagtyp/change")]
        public ActionResult<List<TagTyp>> ChangeTagtyp([FromBody] EditTagtypTransport chg) {
            if (_auth.IsAdmin(User)) {
                var result =_svzRepository.ChangeTagtyp(chg);
                if (result != null) {
                    // Aenderungen an alle Clients senden  
                    _hub.Clients.All.SendAsync(NotificationHub.TagtypChangeEvent, chg);
                }
                return Ok();
            }
            return StatusCode(401);
        }

        // --- Vlan ---
        
        [HttpGet] 
        [ActionName("vlan/all")]
        public ActionResult<List<Vlan>> Vlans() {
            return _svzRepository.GetVlans();
        }

        [HttpPost]
        [ActionName("vlan/change")]
        public ActionResult<List<Vlan>> ChangeVlan([FromBody] EditVlanTransport chg) {
            if (_auth.IsAdmin(User)) {
                var result =_svzRepository.ChangeVlan(chg);
                if (result != null) {
                    // Aenderungen an alle Clients senden  
                    _hub.Clients.All.SendAsync(NotificationHub.VlanChangeEvent, chg);
                }
                return Ok();
            }
            return StatusCode(401);        
        }

    }
    
}
