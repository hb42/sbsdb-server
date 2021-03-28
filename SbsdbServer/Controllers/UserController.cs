using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Controllers {
    public class UserController : AbstractControllerBase<UserController> {
        private readonly IUserService _userService;
        private readonly AuthorizationHelper _authorizationHelper;

        public UserController(IUserService userservice, AuthorizationHelper auth) {
            _userService = userservice;
            _authorizationHelper = auth;
        }

        [HttpGet]
        public UserSession Get() {
            UserSession user = _userService.GetUser(AuthorizationHelper.GetUserId(User));
            
            user.IsAdmin = false;
            user.IsReadonly = false;  // TODO sind Unterscheidungen ausser 'Admin' sinnvoll??
            user.IsHotline = false;
            if (_authorizationHelper.IsAdmin(User)) {
                user.IsAdmin = true;
                user.IsReadonly = true;
                user.IsHotline = true;
            }
            return user;
        }

        [HttpPost]
        public void Set([FromBody] UserSession user) {
            _userService.SetUser(AuthorizationHelper.GetUserId(User), user);
        }

        [Authorize(Policy = "adminUser")]
        [HttpDelete("{id}")]
        public void Delete(long id) {
            _userService.DeleteUser(id);
        }

       
        /*
         * Dump Kerberos-Ticket
         */
        private void debug() {
            // string sid = "S-1-5-21-3981708267-1880105098-3983911190-586075"; // e077
            string sid = "S-1-5-21-2177293706-942526630-1309965019-1013"; // loc PS> Get-LocalGroup | select name,sid
            Log.LogDebug("***** User *****");
            Log.LogDebug("Configuration: " + Configuration["AdminRole"]);
            Log.LogDebug("ClaimsType: " + ClaimTypes.Role);
            Log.LogDebug("Name: " + User.Identity.Name);
            Log.LogDebug("AuthenticationType: " + User.Identity.AuthenticationType);
            Log.LogDebug("IsAuthenticated: " + User.Identity.IsAuthenticated);
            Log.LogDebug("inRole: " + User.IsInRole(Configuration["AdminRole"]));
            Log.LogDebug("inRole SID: " + User.IsInRole(sid));
            Log.LogDebug("claims.count: " + User.Claims.Count());
            foreach (var claim in User.Claims) {
                Log.LogDebug(
                    $"ClaimType:[{claim.Type}], ClaimValue:[{claim.Value}], Issuer:[{claim.Issuer}], valueType:[{claim.ValueType}], OriginalIssuer:[{claim.OriginalIssuer}]");
                // foreach (var prop in claim.Properties) {
                //     Log.LogDebug($"Property.Key: {prop.Key} = {prop.Value}");
                // }
            }
            foreach(var id in User.Identities)
                Log.LogDebug($"Identity.Name: {id.Name}, Label: {id.Label}, AuthenticationType: {id.AuthenticationType}");
                    
            // das Folgende funktioniert nur unter Windows!
            // var si = new System.Security.Principal.SecurityIdentifier(sid);
            // var gName = si.Translate(typeof(System.Security.Principal.NTAccount));
            // Log.LogDebug($"GroupName: {gName.Value}");
        }
    }
}
