using System;
using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class ApController : AbstractControllerBase<ApController> {
        private readonly IApService _apService;
        private readonly AuthorizationHelper _auth;

        public ApController(IApService service, AuthorizationHelper auth) {
            _apService = service;
            _auth = auth;
        }

        [HttpGet]
        public ActionResult<List<Arbeitsplatz>> All() {
            return _apService.GetAll();
        }

        [HttpGet("{page}/{pagesize}")]
        public ActionResult<List<Arbeitsplatz>> Page(int page, int pagesize) {
            return _apService.GetPage(page, pagesize);
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

        [HttpPost]
        [ActionName("aps")]
        public ActionResult<List<Arbeitsplatz>> ApQuery([FromBody] string query) {
            return Ok();
        }

        [HttpPost]
        [ActionName("changeap")]
        public ActionResult<ApHw> ApChange([FromBody] EditApTransport chg) {
            if (_auth.IsAdmin(User)) {
                var ap = _apService.ChangeAp(chg);
                if (ap != null) {
                    // TODO SSE(ap)
                }

                return Ok(ap);
            }
            return StatusCode(401);
        }

        [HttpGet]
        [ActionName("count")]
        public ActionResult<int> Count() {
            return _apService.GetCount();
        }
        
        [HttpGet] 
        public ActionResult<List<TagTyp>> TagTypes() {
            return _apService.GetTagTypes();
        }
        
    }
}
