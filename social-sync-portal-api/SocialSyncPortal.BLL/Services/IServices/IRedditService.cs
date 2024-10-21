using SocialSyncPortal.DAL.Entities;

namespace SocialSyncPortal.BLL.Services.IServices
{
    public interface IRedditService
    {
        Task<List<SocialPost>> GetSocialPostsAsync();
    }
}
