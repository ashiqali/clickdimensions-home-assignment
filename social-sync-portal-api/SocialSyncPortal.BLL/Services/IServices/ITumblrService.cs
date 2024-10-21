using SocialSyncPortal.DAL.Entities;
namespace SocialSyncPortal.BLL.Services.IServices
{
    public interface ITumblrService
    {
        Task<List<SocialPost>> GetSocialPostsAsync();
    }
}
