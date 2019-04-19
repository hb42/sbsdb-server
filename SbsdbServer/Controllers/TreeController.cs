using System.Collections.Generic;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace hb.SbsdbServer.Controllers {
    public class TreeController : AbstractControllerBase<TreeController> {
        private readonly ITreeService _treeService;

        public TreeController(ITreeService ts) {
            _treeService = ts;
        }

        [HttpGet]
        [ActionName("oe")]
        public ActionResult<IEnumerable<object>> OeTree() {
            var tree = _treeService.GetOeTree();
            return Ok(tree);
        }

        [HttpGet]
        [ActionName("bst")]
        public ActionResult<IEnumerable<object>> BstTree() {
            var tree = _treeService.GetBstTree();
            return Ok(tree);
        }

        [HttpGet]
        [ActionName("vlan")]
        public ActionResult<IEnumerable<object>> VlanTree() {
            var tree = _treeService.GetVlanTree();
            return Ok(tree);
        }
    }
}
