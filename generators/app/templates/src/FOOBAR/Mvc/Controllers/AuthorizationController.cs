using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace FOOBAR.Mvc.Controllers
{
	[Route("api/[controller]")]
	public class AuthorizationController : Controller
	{
		private readonly IPermissionService _permissionService;

		public AuthorizationController(IPermissionService permissionService)
		{
			_permissionService = permissionService;
		}


		[HttpGet("haspermission")]
		public async Task<bool> HasPermission([FromQuery] string permission)
		{
			return await _permissionService.HasPermissions(permission);
		}

		[HttpGet("haspermissionin")]
		public async Task<bool> HasPermissionIn([FromQuery] IEnumerable<string> permissions)
		{
			var rolesAndPermissions = await _permissionService.GetRolesAndPermissions();
			var perms = permissions.Select(x => x.ToUpper());
			return rolesAndPermissions.permissions.Any(p => perms.Contains(p.ToUpper()));
		}

		[HttpGet("permissions")]
		public async Task<List<string>> HasPermissionIn()
		{
			var (_, permissions) = await _permissionService.GetRolesAndPermissions();

			return permissions?.Select(p => p.ToLower()).ToList();
		}

		[HttpGet("hasallpermissions")]
		public async Task<bool> HasAllPermissions([FromQuery] IEnumerable<string> permissions)
		{
			var rolesAndPermissions = await _permissionService.GetRolesAndPermissions();
			var perms = permissions.Select(x => x.ToUpper());
			return perms.All(p => rolesAndPermissions.permissions.Contains(p));
		}
	}
}
