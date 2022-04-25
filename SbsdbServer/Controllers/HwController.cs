using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class HwController : AbstractControllerBase<HwController> {
        private readonly IHwService _hwService;
        private readonly AuthorizationHelper _auth;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<HwController> _log;

        public HwController(IHwService service, AuthorizationHelper auth, IHubContext<NotificationHub> hub, ILogger<HwController> log) {
            _hwService = service;
            _auth = auth;
            _log = log;
            _hub = hub;
        }

        [HttpGet]
        [ActionName("all")]
        public ActionResult<List<Hardware>> All() {
            return _hwService.GetAll();
        }

        [HttpGet("{id}")]
        [ActionName("id")]
        public ActionResult<List<Hardware>> GetHwById(long id) {
            return _hwService.GetHardware(id);
        }
        
        [HttpGet("{page}/{pagesize}")]
        public ActionResult<List<Hardware>> Page(int page, int pagesize) {
            return _hwService.GetPage(page, pagesize);
        }
        
        [HttpGet]
        [ActionName("count")]
        public ActionResult<int> Count() {
            return _hwService.GetCount();
        }
        
        [HttpPost]
        [ActionName("changehw")]
        public ActionResult<HwTransport> HwChange([FromBody] EditHwTransport chg) {
            
            if (_auth.IsAdmin(User)) {
                var hw = _hwService.ChangeHw(chg);
                if (hw != null) {
                    // Aenderungen an alle Clients senden  
                    _log.LogDebug("HwChange done, trigger notification");
                    _hub.Clients.All.SendAsync(NotificationHub.HwChangeEvent, hw);
                }
                return Ok();
            }
            return StatusCode(401);
        }

        [HttpPost]
        [ActionName("addhw")]
        public ActionResult<AddHwTransport> AddHw([FromBody] NewHwTransport nhw) {
            if (_auth.IsAdmin(User)) {
                var hws = _hwService.AddHw(nhw);
                if (hws != null) {
                    _log.LogDebug("AddHw done, trigger notification");
                    _hub.Clients.All.SendAsync(NotificationHub.AddHwEvent, hws);
                }
                return Ok();
            }
            return StatusCode(401);
        }

        [HttpGet("{id}")]
        [ActionName("hwhistoryfor")]
        public List<HwHistory> GetHwHistoryFor(long id) {
            return _hwService.GetHwHistoryFor(id);
        }

    }
}
