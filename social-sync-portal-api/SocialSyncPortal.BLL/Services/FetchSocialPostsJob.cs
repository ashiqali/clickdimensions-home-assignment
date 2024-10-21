using Microsoft.EntityFrameworkCore;
using SocialSyncPortal.DAL.Entities;
using Quartz;
using SocialSyncPortal.DAL.DataContext;
using SocialSyncPortal.BLL.Services.IServices;

namespace SocialSyncPortal.BLL.Services
{
    public class FetchSocialPostsJob : IJob
    {
        private readonly IRedditService _redditClient;
        private readonly ITumblrService _tumblrClient;
        private readonly SocialSyncPortalDbContext _dbContext;

        private static List<SocialPost> _socialPosts = new List<SocialPost>();

        public FetchSocialPostsJob(IRedditService redditClient, ITumblrService tumblrClient, SocialSyncPortalDbContext dbContext)
        {
            _redditClient = redditClient;
            _tumblrClient = tumblrClient;
            _dbContext = dbContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // Fetch data from Reddit and Tumblr
                var redditPosts = await _redditClient.GetSocialPostsAsync();
                var tumblrPosts = await _tumblrClient.GetSocialPostsAsync();

                // Combine both sources of posts and store them
                var posts = redditPosts.Concat(tumblrPosts).ToList();

                // Avoid duplicating posts in the database (check by unique ID)
                foreach (var post in posts)
                {
                    var exists = await _dbContext.SocialPosts.AnyAsync(p => p.Id == post.Id);
                    //if (!exists)
                    //{
                        _dbContext.SocialPosts.Add(post);
                    //}
                }

                // Save posts to database
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle errors and log them
                Console.WriteLine($"Error in FetchSocialPostsJob: {ex.Message}");
            }

        }
    }

}
