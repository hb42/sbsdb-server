using System.Linq;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            _userService.SetUser(GetUserId(), user);
        }

        [Authorize(Roles = Const.ROLE_ADMIN)]
        [HttpDelete("{id}")]
        public void Delete(long id) {
            _userService.DeleteUser(id);
        }

        private string GetUserId() {
            var u = User.Identity.Name.Split(@"\");
            Log.LogDebug("***** User *****");
            Log.LogDebug("Name: " + User.Identity.Name);
            Log.LogDebug("AuthenticationType: " + User.Identity.AuthenticationType);
            Log.LogDebug("IsAuthenticated: " + User.Identity.IsAuthenticated);
            Log.LogDebug("inRole: " + User.IsInRole("e077ggx-791-it-service-basis"));
            Log.LogDebug("inRole U: " + User.IsInRole("E077GGX-791-IT-SERVICE-BASIS"));
            foreach (var claim in User.Claims) {
                Log.LogDebug(
                    $"ClaimType:[{claim.Type}], ClaimValue:[{claim.Value}], Issuer:[{claim.Issuer}], OriginalIssuer:[{claim.OriginalIssuer}]");
                foreach (var prop in claim.Properties) {
                    Log.LogDebug($"Property.Key: {prop.Key} = {prop.Value}");
                }
            }
            foreach(var id in User.Identities)
                Log.LogDebug($"Identity.Name: {id.Name}, Label: {id.Label}, AuthenticationType: {id.AuthenticationType}");
            
            return u[u.Count() - 1].ToUpper();
        }
    }
}
