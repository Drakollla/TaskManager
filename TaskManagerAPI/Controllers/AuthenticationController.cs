using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.DTO;

namespace TaskManagerAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationManager _authManager;

        public AuthenticationController(IAuthenticationManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("register")]
        [EnableRateLimiting("AuthRateLimitPolicy")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _authManager.RegisterUser(userForRegistration);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

                return BadRequest(ModelState);
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        [EnableRateLimiting("AuthRateLimitPolicy")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            var tokenDto = await _authManager.ValidateAndCreateToken(user);

            if(tokenDto == null) 
                return Unauthorized();

            return Ok(tokenDto);
        }

        [HttpPost("refresh")]
        [EnableRateLimiting("AuthRateLimitPolicy")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _authManager.RefreshToken(tokenDto);

            return Ok(tokenDtoToReturn);
        }
    }
}