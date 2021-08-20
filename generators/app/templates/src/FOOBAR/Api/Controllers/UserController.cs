using FOOBAR.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Digipolis.Auth.Services;

namespace FOOBAR.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Get user data.
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserModel), 200)]
        // [AuthorizeWith(Permission = "put-your-privilege-here")]
        public IActionResult GetUserdata()
        {
            return Ok(new UserModel { Name = _authService.User?.Identity?.Name });
        }

        /// <summary>
        /// Logout.
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [Authorize]
        public async Task<IActionResult> LogoutAsync()
        {
            await _authService.Logout();
            return NoContent();
        }
    }
}
