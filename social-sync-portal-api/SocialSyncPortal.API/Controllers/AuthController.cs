using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SocialSyncPortal.BLL.Services.IServices;
using SocialSyncPortal.BLL.Utilities.CustomExceptions;
using SocialSyncPortal.DTO.DTOs.Jwt;
using SocialSyncPortal.DTO.DTOs.User;
namespace SocialSyncPortal.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // POST: api/v1/auth/login
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userToLoginDTO"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserToLoginDTO userToLoginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _authService.LoginAsync(userToLoginDTO);
                if (user == null)
                {
                    _logger.LogWarning("Login failed for user {Email}. User not found.", userToLoginDTO.Username);
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning(ex, "User with email {Email} not found.", userToLoginDTO.Username);
                return Unauthorized(new { message = "User not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user {Email}.", userToLoginDTO.Username);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/v1/auth/register
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="userToRegisterDTO"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserToRegisterDTO userToRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var registeredUser = await _authService.RegisterAsync(userToRegisterDTO);
                return CreatedAtAction(nameof(Login), new { email = registeredUser.Username }, registeredUser);
            }
            catch (DuplicateUserException ex)
            {
                _logger.LogWarning("User {Username} already exists", userToRegisterDTO.Username);
                return BadRequest(new { message = "User already exists" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for user {Email}.", userToRegisterDTO.Username);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/v1/auth/refresh
        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="refreshTokenDTO"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newToken = _authService.RefreshToken(refreshTokenDTO);
                if (newToken == null)
                {
                    _logger.LogWarning("Invalid refresh token provided.");
                    return Unauthorized(new { message = "Invalid refresh token" });
                }

                return Ok(newToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing token.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}

