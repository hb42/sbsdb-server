using System.Linq;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class UserController : AbstractControllerBase<UserController> {
        private readonly IUserService _userService;

        public UserController(IUserService userservice) {
            _userService = userservice;
        }

        [HttpGet]
        public UserSession Get() {
            UserSession user = _userService.GetUser(GetUserId());
            if (User.IsInRole(Const.ROLE_ADMIN)) {
                user.IsAdmin = true;
                user.IsReadonly = true;
                user.IsHotline = true;
            } else if (User.IsInRole(Const.ROLE_READONLY)) {
                user.IsAdmin = false;
                user.IsReadonly = true;
                user.IsHotline = true;
            } else if (User.IsInRole(Const.ROLE_HOTLINE)) {
                user.IsAdmin = false;
                user.IsReadonly = false;
                user.IsHotline = true;
            }
            else {
                user.IsAdmin = false;
                user.IsReadonly = false;
                user.IsHotline = false;
            }
            return user;
        }

        [HttpPost]
        public void Set([FromBody] UserSession user) {
            user.IsAdmin = false;
            user.IsReadonly = false;
            user.IsHotline = false;
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
