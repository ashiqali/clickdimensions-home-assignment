namespace SocialSyncPortal.DTO.DTOs.Jwt;

public class RefreshTokenToReturnDTO
{
    public string Username { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
