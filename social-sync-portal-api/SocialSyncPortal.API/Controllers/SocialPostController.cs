using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SocialSyncPortal.BLL.Services.IServices;
using SocialSyncPortal.BLL.Utilities.CustomExceptions;
using SocialSyncPortal.DTO.DTOs.SocialPost;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SocialSyncPortal.API.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class SocialPostController : ControllerBase
    {
        private readonly ISocialPostService _socialPostService;
        private readonly ILogger<SocialPostController> _logger;

        public SocialPostController(ISocialPostService socialPostService, ILogger<SocialPostController> logger)
        {
            _socialPostService = socialPostService;
            _logger = logger;
        }

        // GET: api/v1/socialpost
        /// <summary>
        /// Get Social Posts
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSocialPosts(CancellationToken cancellationToken)
        {
            try
            {
                var posts = await _socialPostService.GetSocialPostsAsync(cancellationToken);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching posts.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/v1/socialpost/{postId}
        /// <summary>
        /// Get Social Post by postId
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetSocialPost(string postId, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _socialPostService.GetSocialPostAsync(postId, cancellationToken);
                return Ok(post);
            }
            catch (SocialPostNotFoundException ex)
            {
                _logger.LogWarning(ex, "SocialPost with ID {PostId} not found.", postId);
                return NotFound(new { message = "SocialPost not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching post with ID {PostId}.", postId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/v1/socialpost
        /// <summary>
        /// Add Social Post
        /// </summary>
        /// <param name="postToAddDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddSocialPost([FromBody] SocialPostToAddDTO postToAddDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var post = await _socialPostService.AddSocialPostAsync(postToAddDTO);
                return CreatedAtAction(nameof(GetSocialPost), new { postId = post.Id }, post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new post.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/v1/socialpost
        /// <summary>
        /// Update Social Post
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="postToUpdateDTO"></param>
        /// <returns></returns>
        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdateSocialPost(string postId, [FromBody] SocialPostToUpdateDTO postToUpdateDTO)
        {
            if (postId != postToUpdateDTO.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedPost=  await _socialPostService.UpdateSocialPostAsync(postToUpdateDTO);
                return Ok(updatedPost);
            }
            catch (SocialPostNotFoundException ex)
            {
                _logger.LogWarning(ex, "SocialPost with ID {PostId} not found.", postId);
                return NotFound(new { message = "SocialPost not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating post with ID {PostId}.", postId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/v1/socialpost/{postId}
        /// <summary>
        /// Delete Social Post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeleteSocialPost(string postId)
        {
            try
            {
                await _socialPostService.DeleteSocialPostAsync(postId);
                return NoContent();
            }
            catch (SocialPostNotFoundException ex)
            {
                _logger.LogWarning(ex, "SocialPost with ID {PostId} not found.", postId);
                return NotFound(new { message = "SocialPost not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting social post with ID {PostId}.", postId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
