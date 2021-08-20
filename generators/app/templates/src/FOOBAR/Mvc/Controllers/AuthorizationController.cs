using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Auth.Services;

namespace FOOBAR.Mvc.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IPermissionService _permissionService;

        public AuthorizationController(IAuthService authService, IPermissionService permissionService)
        {
            _authService = authService ?? throw new ArgumentNullException($"{nameof(AuthorizationController)}.Ctr parameter {nameof(authService)} cannot be null.");
            _permissionService = permissionService ?? throw new ArgumentNullException($"{nameof(AuthorizationController)}.Ctr parameter {nameof(permissionService)} cannot be null.");
        }

        [HttpGet("haspermission")]
        public async Task<bool> HasPermission([FromQuery] string permission)
        {
            try
            {
                var rolesAndPermissions = await _permissionService.GetRolesAndPermissions();
                var activePermissions = rolesAndPermissions.permissions.Select(x => x.ToUpper());
                return activePermissions.Contains(permission.ToUpper());
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpGet("haspermissionin")]
        public async Task<bool> HasPermissionIn([FromQuery] IEnumerable<string> permissions)
        {
            var rolesAndPermissions = await _permissionService.GetRolesAndPermissions();
            var activePermissions = rolesAndPermissions.permissions.Select(x => x.ToUpper());
            var perms = permissions.Select(x => x.ToUpper());
            return activePermissions.Any(p => perms.Contains(p));
        }

        [HttpGet("hasallpermissions")]
        public async Task<bool> HasAllPermissions([FromQuery] IEnumerable<string> permissions)
        {
            var rolesAndPermissions = await _permissionService.GetRolesAndPermissions();
            var activePermissions = rolesAndPermissions.permissions.Select(x => x.ToUpper());
            var perms = permissions.Select(x => x.ToUpper());
            return perms.All(p => activePermissions.Contains(p));
        }
    }
}
