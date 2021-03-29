using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace hb.SbsdbServer.Controllers {
    public class HwKonfigController : AbstractControllerBase<HwKonfigController> {
        private readonly IHwKonfigService _hwKonfigService;

        public HwKonfigController(IHwKonfigService service) {
            _hwKonfigService = service;
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
        
    }
}
