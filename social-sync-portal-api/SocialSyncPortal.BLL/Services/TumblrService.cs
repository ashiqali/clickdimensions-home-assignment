using Microsoft.Extensions.Configuration;
using RestSharp;
using SocialSyncPortal.BLL.Services.IServices;
using SocialSyncPortal.DAL.Entities;
using SocialSyncPortal.DTO.DTOs.Job;

namespace SocialSyncPortal.BLL.Services
{
    public class TumblrService : ITumblrService
    {
        private readonly RestClient _client;
        private readonly string _apiKey;

        public TumblrService(IConfiguration configuration)
        {
            _client = new RestClient(configuration["TumblrSettings:ClientUrl"]);
            _apiKey = configuration["TumblrSettings:ApiKey"];
        }

        public async Task<List<SocialPost>> GetSocialPostsAsync()
        {
            var request = new RestRequest();
            request.AddQueryParameter("api_key", _apiKey);
            var response = await _client.ExecuteAsync<TumblrResponseDTO>(request);

            if (!response.IsSuccessful || response.Data?.Response?.Posts == null)
            {
                throw new Exception("Failed to fetch Tumblr posts.");
            }

            return response.Data.Response.Posts.Select(p => new SocialPost
            {
                Id = p.Id.ToString(),
                Title = string.IsNullOrEmpty(p.Summary) ? "Title not available" : p.Summary, 
                Author = string.IsNullOrEmpty(p.Reblog_Key) ? "Author not available" : p.Reblog_Key,
                Source = "Tumblr",
                Popularity = p.Note_Count,
                PublishedDate = DateTimeOffset.FromUnixTimeSeconds(p.Timestamp).DateTime
            }).ToList();
        }
    }
}
