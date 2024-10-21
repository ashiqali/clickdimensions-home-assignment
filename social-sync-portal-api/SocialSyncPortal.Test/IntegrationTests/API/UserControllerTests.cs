using SocialSyncPortal.DAL.DataContext;
using SocialSyncPortal.DAL.Entities;
using SocialSyncPortal.DTO.DTOs.User;
using SocialSyncPortal.Test.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SocialSyncPortal.Test.IntegrationTests.API;

public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    private const string baseURL = "https://localhost:44338/";

    public UserControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_WhenAuthTokenIsNotProvided_ReturnsUnauthorized()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<SocialSyncPortalDbContext>();
            db.Database.EnsureCreated();
        }

        // Act
        var response = await _client.GetAsync("api/v1/user");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<object>();
        return passwordHasher.HashPassword(null, password);
    }
}
