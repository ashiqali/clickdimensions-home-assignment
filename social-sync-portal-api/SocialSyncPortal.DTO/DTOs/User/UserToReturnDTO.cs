namespace SocialSyncPortal.DTO.DTOs.User;

public class UserToReturnDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
