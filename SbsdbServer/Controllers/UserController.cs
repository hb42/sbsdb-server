using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hb.SbsdbServer.Controllers {

  public class UserController : AbstractControllerBase<UserController> {

    private readonly IUserService userService;

    public UserController(IUserService userservice) {
      userService = userservice;
    }

    [HttpGet]
    public UserSession Get() {
      return userService.GetUser(GetUserId());
    }

    [HttpPost]
    public void Set([FromBody]UserSession user) {
      userService.SetUser(GetUserId(), user);
    }

    [Authorize(Roles = Const.ROLE_ADMIN)]
    [HttpDelete("{id}")]
    public void Delete(long id) {
      userService.DeleteUser(id);
    }

    private string GetUserId() {
      string[] u = User.Identity.Name.Split(@"\");
      return u[u.Count() - 1].ToUpper();
    }

  }
}
