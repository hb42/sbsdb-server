using System.Linq;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hb.SbsdbServer.Controllers {
    public class UserController : AbstractControllerBase<UserController> {
        private readonly IUserService _userService;

        public UserController(IUserService userservice) {
            _userService = userservice;
        }

        [HttpGet]
        public UserSession Get() {
            return _userService.GetUser(GetUserId());
        }

        [HttpPost]
        public void Set([FromBody] UserSession user) {
            _userService.SetUser(GetUserId(), user);
        }

        [Authorize(Roles = Const.ROLE_ADMIN)]
        [HttpDelete("{id}")]
        public void Delete(long id) {
            _userService.DeleteUser(id);
        }

        private string GetUserId() {
            var u = User.Identity.Name.Split(@"\");
            return u[u.Count() - 1].ToUpper();
        }
    }
}
