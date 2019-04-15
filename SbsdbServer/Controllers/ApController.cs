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
    [ActionName("ap")]
    public ActionResult<Arbeitsplatz> ApById(long id) {
      return apService.GetAp(id);
    }

    [HttpPost]
    [ActionName("aps")]
    public ActionResult<IEnumerable<object>> ApQuery([FromBody] string query) {

      return Ok();
    }

  }
}