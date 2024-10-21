using SocialSyncPortal.DTO.DTOs.SocialPost;

namespace SocialSyncPortal.BLL.Services.IServices
{
    public interface ISocialPostService
    {
        Task<List<SocialPostDTO>> GetSocialPostsAsync(CancellationToken cancellationToken = default);
        Task<SocialPostDTO> GetSocialPostAsync(string postId, CancellationToken cancellationToken = default);
        Task<SocialPostDTO> AddSocialPostAsync(SocialPostToAddDTO postToAddDTO);
        Task<SocialPostDTO> UpdateSocialPostAsync(SocialPostToUpdateDTO postToUpdateDTO);
        Task DeleteSocialPostAsync(string postId);
    }
}
