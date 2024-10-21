using SocialSyncPortal.DTO.DTOs.Jwt;
using SocialSyncPortal.DTO.DTOs.User;

namespace SocialSyncPortal.BLL.Services.IServices;

public interface IAuthService
{
    Task<UserToReturnDTO> LoginAsync(UserToLoginDTO userToLoginDTO);
    Task<UserToReturnDTO> RegisterAsync(UserToRegisterDTO userToRegisterDTO);
    RefreshTokenToReturnDTO RefreshToken(RefreshTokenDTO refreshTokenDTO);
}
