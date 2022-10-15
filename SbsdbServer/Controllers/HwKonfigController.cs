using System.Collections.Generic;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class HwKonfigController : AbstractControllerBase<HwKonfigController> {
        private readonly IHwKonfigService _hwKonfigService;
        private readonly IHwKonfigRepository _repo;
        private readonly AuthorizationHelper _auth;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<HwKonfigController> _log;

        public HwKonfigController(IHwKonfigService service,
            IHwKonfigRepository repo,
            AuthorizationHelper auth, 
            IHubContext<NotificationHub> hub, 
            ILogger<HwKonfigController> log) {
            _hwKonfigService = service;
            _repo = repo;
            _auth = auth;
            _log = log;
            _hub = hub;
        }

        [HttpGet]
        [ActionName("all")]
        public ActionResult<List<HwKonfig>> All() {
            return _hwKonfigService.GetAll();
        }

        [HttpGet("{id}")]
        [ActionName("id")]
        public ActionResult<List<HwKonfig>> GetHwKonfigById(long id) {
            return _hwKonfigService.GetHwKonfig(id);
        }
        
        [HttpPost]
        [ActionName("changekonfig")]
        public ActionResult<HwKonfig> ChangeKonfig([FromBody] KonfigChange kc) {
            if (_auth.IsAdmin(User)) {
                var hk = _hwKonfigService.ChangeKonfig(kc);
                if (hk != null) {
                    _hub.Clients.All.SendAsync(NotificationHub.KonfigChangeEvent, hk);
                }
                return Ok();
            }
            return StatusCode(401);
        }
        
        [HttpPost]
        [ActionName("delkonfig")]
        public ActionResult<long> DelKonfig([FromBody] long konfId) {
            if (_auth.IsAdmin(User)) {
                var id = _repo.DelKonfig(konfId);
                if (id != null) {
                    _hub.Clients.All.SendAsync(NotificationHub.KonfigDelEvent, id);
                }
                return Ok();
            }
            return StatusCode(401);
        }
        
        [HttpGet]
        [ActionName("hwkonfiginaussond")]
        public ActionResult<long[]> HwKonfigInAussond() {
            return _repo.HwKonfigInAussond();
        }

    }
}
