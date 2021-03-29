using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace hb.SbsdbServer.Controllers {
    public class BetrstController: AbstractControllerBase<BetrstController> {
        private readonly IBetrstService _betrstService;

        public BetrstController(IBetrstService service) {
            _betrstService = service;
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
        
    }
}