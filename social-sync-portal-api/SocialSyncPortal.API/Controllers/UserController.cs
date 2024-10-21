using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSyncPortal.BLL.Services.IServices;
using SocialSyncPortal.BLL.Utilities.CustomExceptions;
using SocialSyncPortal.DTO.DTOs.User;

namespace SocialSyncPortal.API.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: api/v1/user
        /// <summary>
        /// Get Users
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userService.GetUsersAsync(cancellationToken);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/v1/user/5
        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUser(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.GetUserAsync(userId, cancellationToken);
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning(ex, "User with ID {UserId} not found.", userId);
                return NotFound(new { message = "User not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user with ID {UserId}.", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/v1/user
        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="userToAddDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserToAddDTO userToAddDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.AddUserAsync(userToAddDTO);
                return CreatedAtAction(nameof(GetUser), new { userId = user.Id }, user);
            }
            catch (DuplicateUserException ex)
            {
                _logger.LogWarning("User {Username} already exists", userToAddDTO.Username);
                return BadRequest(new { message = "User already exists" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/v1/user
        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="userToUpdateDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserToUpdateDTO userToUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.UpdateUserAsync(userToUpdateDTO);
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning(ex, "User with ID {UserId} not found.", userToUpdateDTO.Id);
                return NotFound(new { message = "User not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user with ID {UserId}.", userToUpdateDTO.Id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/v1/user/5
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await _userService.DeleteUserAsync(userId);
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning(ex, "User with ID {UserId} not found.", userId);
                return NotFound(new { message = "User not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with ID {UserId}.", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
