using AutoMapper;
using SocialSyncPortal.BLL.Utilities.CustomExceptions;
using SocialSyncPortal.BLL.Utilities.Settings;
using SocialSyncPortal.DAL.Entities;
using SocialSyncPortal.DAL.Repositories.IRepositories;
using SocialSyncPortal.DTO.DTOs.Jwt;
using SocialSyncPortal.DTO.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SocialSyncPortal.BLL.Services.IServices;

namespace SocialSyncPortal.BLL.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserRepository userRepository, IMapper mapper, IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<UserToReturnDTO> LoginAsync(UserToLoginDTO userToLoginDTO)
    {
        var user = await _userRepository.GetAsync(u => u.Username == userToLoginDTO.Username.ToLower());

        if (user == null || !VerifyPassword(userToLoginDTO.Password, user.Password))
            throw new UserNotFoundException();

        var userToReturn = _mapper.Map<UserToReturnDTO>(user);
        userToReturn.Token = GenerateToken(user.Id, user.Username);
        userToReturn.RefreshToken = GenerateToken(user.Id, user.Username, true);

        return userToReturn;
    }

    public async Task<UserToReturnDTO> RegisterAsync(UserToRegisterDTO userToRegisterDTO)
    {

        var existingUser = await _userRepository.GetAsync(c => c.Username == userToRegisterDTO.Username);
        if (existingUser != null)
        {
            throw new DuplicateUserException();
        }

        userToRegisterDTO.Username = userToRegisterDTO.Username.ToLower();

        var user = _mapper.Map<User>(userToRegisterDTO);
        user.Password = HashPassword(userToRegisterDTO.Password); // Hash the password

        var addedUser = await _userRepository.AddAsync(user);

        var userToReturn = _mapper.Map<UserToReturnDTO>(addedUser);
        userToReturn.Token = GenerateToken(addedUser.Id, addedUser.Username);
        userToReturn.RefreshToken = GenerateToken(addedUser.Id, addedUser.Username, true);

        return userToReturn;
    }

    public RefreshTokenToReturnDTO RefreshToken(RefreshTokenDTO refreshTokenDTO)
    {
        var claimsPrincipal = GetClaimsPrincipal(refreshTokenDTO.RefreshToken);
        if (claimsPrincipal is null)
            throw new BadRequestException();

        var username = claimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.Name)?.FirstOrDefault()?.Value ?? string.Empty;
        var userId = claimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value ?? string.Empty;
        if (userId == string.Empty || username == string.Empty)
            throw new BadRequestException();

        var refreshToken = GenerateToken(int.Parse(userId), username, true);
        var accessToken = GenerateToken(int.Parse(userId), username);

        return new RefreshTokenToReturnDTO
        {
            Username = username,
            Token = accessToken,
            RefreshToken = refreshToken
        };
    }

    private string GenerateToken(int userId, string username, bool isRefresh = false)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username)
        };

        var tokenSecret = isRefresh ? _jwtSettings.RefreshTokenSecret : _jwtSettings.AccessTokenSecret;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var expirationInMinutes = isRefresh ? _jwtSettings.AccessTokenExpirationMinutes : _jwtSettings.RefreshTokenExpirationMinutes;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetClaimsPrincipal(string refreshToken)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecret)),
            ClockSkew = TimeSpan.Zero
        };
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        try
        {
            return jwtSecurityTokenHandler.ValidateToken(refreshToken, validationParameters, out var _);
        }
        catch (Exception)
        {
            throw new BadRequestException();
        }
    }

    private bool VerifyPassword(string enteredPassword, string storedHash)
    {
        var passwordHasher = new PasswordHasher<User>();
        return passwordHasher.VerifyHashedPassword(null, storedHash, enteredPassword) == PasswordVerificationResult.Success;
    }

    private string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<User>();
        return passwordHasher.HashPassword(null, password);
    }
}