using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace hb.SbsdbServer.Controllers {
    public class HwController : AbstractControllerBase<HwController> {
        private readonly IHwService _hwService;

        public HwController(IHwService service) {
            _hwService = service;
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
    }
}
