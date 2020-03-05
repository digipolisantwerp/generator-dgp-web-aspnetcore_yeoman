using Digipolis.Authentication.OAuth.Authorization;
using Digipolis.Authentication.OAuth.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOOBAR.Mvc.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IOAuthService _authService;
        private IEnumerable<string> _permissions;

        public AuthorizationController(IOAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException($"{nameof(AuthorizationController)}.Ctr parameter {nameof(authService)} cannot be null.");
            _permissions = _authService.UserPermissions.Select(x => x.ToUpper());
        }

        [HttpGet("haspermission")]
        public bool HasPermission([FromQuery] string permission)
        {
            return _permissions.Contains(permission.ToUpper());
        }

        [HttpGet("haspermissionin")]
        public bool HasPermissionIn([FromQuery] IEnumerable<string> permissions)
        {
            var perms = permissions.Select(x => x.ToUpper());
            return _permissions.Any(p => perms.Contains(p));
        }

        [HttpGet("hasallpermissions")]
        public bool HasAllPermissions([FromQuery] IEnumerable<string> permissions)
        {
            var perms = permissions.Select(x => x.ToUpper());
            return perms.All(p => _permissions.Contains(p));
        }
    }   
}
