using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialSyncPortal.BLL.Services.IServices;
using SocialSyncPortal.BLL.Utilities.CustomExceptions;
using SocialSyncPortal.DAL.Entities;
using SocialSyncPortal.DAL.Repositories.IRepositories;
using SocialSyncPortal.DTO.DTOs.SocialPost;

namespace SocialSyncPortal.BLL.Services
{
    public class SocialPostService : ISocialPostService
    {
        private readonly ISocialPostRepository _socialpostRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SocialPostService> _logger;

        public SocialPostService(ISocialPostRepository socialPostRepository, IMapper mapper, ILogger<SocialPostService> logger)
        {
            _socialpostRepository = socialPostRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<SocialPostDTO>> GetSocialPostsAsync(CancellationToken cancellationToken = default)
        {
            var posts = await _socialpostRepository.GetListAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("List of {Count} posts has been returned", posts.Count);

            return _mapper.Map<List<SocialPostDTO>>(posts);
        }

        public async Task<SocialPostDTO> GetSocialPostAsync(string postId, CancellationToken cancellationToken = default)
        {
            var post = await _socialpostRepository.GetAsync(x => x.Id == postId, cancellationToken);

            if (post == null)
            {
                _logger.LogError("SocialPost with postId = {PostId} was not found", postId);
                throw new SocialPostNotFoundException();
            }

            return _mapper.Map<SocialPostDTO>(post);
        }

        public async Task<SocialPostDTO> AddSocialPostAsync(SocialPostToAddDTO postToAddDTO)
        {
            postToAddDTO.Id = GenerateId();
            var post = await _socialpostRepository.AddAsync(_mapper.Map<SocialPost>(postToAddDTO));
            return _mapper.Map<SocialPostDTO>(post);
        }

        public async Task<SocialPostDTO> UpdateSocialPostAsync(SocialPostToUpdateDTO postToUpdateDTO)
        {
            var post = await _socialpostRepository.GetAsync(x => x.Id == postToUpdateDTO.Id);

            if (post == null)
            {
                _logger.LogError("SocialPost with postId = {PostId} was not found", postToUpdateDTO.Id);
                throw new SocialPostNotFoundException();
            }

            var postToUpdate = _mapper.Map<SocialPost>(postToUpdateDTO);
            return _mapper.Map<SocialPostDTO>(await _socialpostRepository.UpdateAsync(postToUpdate));
        }

        public async Task DeleteSocialPostAsync(string postId)
        {
            var post = await _socialpostRepository.GetAsync(x => x.Id == postId);

            if (post == null)
            {
                _logger.LogError("SocialPost with postId = {PostId} was not found", postId);
                throw new SocialPostNotFoundException();
            }

            await _socialpostRepository.DeleteAsync(post);
        }

        private string GenerateId()
        {
            // Generate a new unique Id (e.g., using a timestamp or a GUID)
            return DateTime.UtcNow.Ticks.ToString();
        }
    }
}
