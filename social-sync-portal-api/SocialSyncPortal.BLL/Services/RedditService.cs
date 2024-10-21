using Microsoft.Extensions.Configuration;
using RestSharp;
using SocialSyncPortal.BLL.Services.IServices;
using SocialSyncPortal.DAL.Entities;
using SocialSyncPortal.DTO.DTOs.Job;
using System.Text.Json;

namespace SocialSyncPortal.BLL.Services
{
    public class RedditService : IRedditService
    {
        private readonly RestClient _client;
        private readonly RestClient _authClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _username;
        private readonly string _password;
        private string _accessToken;

        public RedditService(IConfiguration configuration)
        {
            // Load sensitive credentials from configuration            
            _client = new RestClient(configuration["RedditSettings:ClientUrl"]);
            _authClient = new RestClient(configuration["RedditSettings:AuthUrl"]);
            _clientId = configuration["RedditSettings:ClientId"];
            _clientSecret = configuration["RedditSettings:ClientSecret"];
            _username = configuration["RedditSettings:Username"];
            _password = configuration["RedditSettings:Password"];
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var request = new RestRequest("/api/v1/access_token", Method.Post);
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}")));
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", _username);
            request.AddParameter("password", _password);

            var response = await _authClient.ExecuteAsync(request);
            var tokenResponse = JsonSerializer.Deserialize<RedditAuthDTO>(response.Content);

            return tokenResponse.access_token;
        }

        public async Task<List<SocialPost>> GetSocialPostsAsync()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = await GetAccessTokenAsync();
            }

            var request = new RestRequest("/hot", Method.Get);
            request.AddHeader("Authorization", "Bearer " + _accessToken);

            var response = await _client.ExecuteAsync<RedditResponseDTO>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Failed to fetch Reddit posts.");
            }

            return response.Data.Data.Children.Select(c => new SocialPost
            {
                Id = c.Data.Id,
                Title = string.IsNullOrEmpty(c.Data.Title) ? "Title not available" : c.Data.Title,                
                Author = string.IsNullOrEmpty(c.Data.Author_Fullname) ? "Author not available" : c.Data.Author_Fullname,                
                Source = "Reddit",
                Popularity = c.Data.Ups,
                PublishedDate = DateTimeOffset.FromUnixTimeSeconds((long)c.Data.Created).DateTime
            }).ToList();
        }
    }
}
