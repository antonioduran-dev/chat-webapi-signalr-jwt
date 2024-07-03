using DB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.App.DTOs;
using WebApi.App.Services;

namespace WebApi.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly IAccessService _accessService;

        public AccessController(IAccessService accessService)
        {
            _accessService = accessService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserDTO model)
        {
            var user = await _accessService.Register(model);

            if (user != null)
                return StatusCode(StatusCodes.Status200OK, new {isSuccess = true});
            else
                return StatusCode(StatusCodes.Status400BadRequest, new { isSuccess = false });

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDTO model)
        {
            var userToken = await _accessService.Login(model);

            if (userToken != null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = userToken });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { isSuccess = false, token = "" });
        }
    }
}
