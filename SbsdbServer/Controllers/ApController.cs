using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace hb.SbsdbServer.Controllers {
    public class ApController : AbstractControllerBase<ApController> {
        private readonly IApService _apService;

        public ApController(IApService service) {
            _apService = service;
        }

        [HttpGet]
        public ActionResult<List<Arbeitsplatz>> All() {
            return _apService.GetAll();
        }

        [HttpGet("{page}")]
        public ActionResult<List<Arbeitsplatz>> Page(int page) {
            return _apService.GetPage(page);
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

        [HttpGet("{oeid}")]
        [ActionName("oe")]
        public ActionResult<List<Arbeitsplatz>> ApForOe(long oeid) {
            return _apService.ApsForOe(oeid);
        }

        [HttpPost]
        [ActionName("aps")]
        public ActionResult<List<Arbeitsplatz>> ApQuery([FromBody] string query) {
            return Ok();
        }
    }
}
