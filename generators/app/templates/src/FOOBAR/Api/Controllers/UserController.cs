using Digipolis.Authentication.OAuth.Authorization;
using Digipolis.Authentication.OAuth.Services;
using FOOBAR.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace FOOBAR.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class UserController : Controller
    {
        private readonly IOAuthService _oauthService;

        public UserController(IOAuthService oauthService)
        {
            _oauthService = oauthService;
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
            return Ok(new UserModel { Name = _oauthService.User?.Identity?.Name });
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
            await _oauthService.LogoutAsync();
            return NoContent();
        }
    }
}
