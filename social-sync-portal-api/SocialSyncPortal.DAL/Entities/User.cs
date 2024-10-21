using System.ComponentModel.DataAnnotations;

namespace SocialSyncPortal.DAL.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(20)]
    public string Surname { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

}
