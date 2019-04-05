using System;
using Microsoft.Extensions.Logging;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using hb.SbsdbServer.ViewModel;

namespace hb.SbsdbServer.Controllers {

  public class TreeController: AbstractControllerBase<TreeController> {

    private readonly ITreeService treeService;

    public TreeController(ITreeService ts) {
      treeService = ts;
    }

    [HttpGet]
    [ActionName("oe")]
    public ActionResult<IEnumerable<object>> OeTree() {
      var tree = treeService.GetOeTree();
      return Ok(tree);
    }

    [HttpGet]
    [ActionName("bst")]
    public ActionResult<IEnumerable<object>> BstTree() {
      var tree = treeService.GetBstTree();
      return Ok(tree);
    }

  }
}
