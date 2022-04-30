using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class HwKonfigController : AbstractControllerBase<HwKonfigController> {
        private readonly IHwKonfigService _hwKonfigService;
        private readonly AuthorizationHelper _auth;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<HwKonfigController> _log;

        public HwKonfigController(IHwKonfigService service, AuthorizationHelper auth, IHubContext<NotificationHub> hub, ILogger<HwKonfigController> log) {
            _hwKonfigService = service;
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
        
    }
}
