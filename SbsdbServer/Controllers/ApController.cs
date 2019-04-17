using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hb.SbsdbServer.Controllers;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SbsdbServer.Controllers
{
  public class ApController : AbstractControllerBase<ApController> {

    private readonly IApService apService;

    public ApController(IApService service) {
      apService = service;
    }

    [HttpGet("{id}")]
    [ActionName("id")]
    public ActionResult<Arbeitsplatz> ApById(long id) {
      return apService.GetAp(id);
    }

    [HttpGet("{search}")]
    [ActionName("search")]
    public ActionResult<List<Arbeitsplatz>> ApSearch(string search) {
      return apService.GetAps(search);
    }

    [HttpGet("{oeid}")]
    [ActionName("oe")]
    public ActionResult<List<Arbeitsplatz>> ApForOe(long oeid) {
      return apService.ApsForOe(oeid);
    }

    [HttpPost]
    [ActionName("aps")]
    public ActionResult<List<Arbeitsplatz>> ApQuery([FromBody] string query) {

      return Ok();
    }

  }
}