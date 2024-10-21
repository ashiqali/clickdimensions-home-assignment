using System.ComponentModel.DataAnnotations;

namespace SocialSyncPortal.DTO.DTOs.SocialPost
{
    public class SocialPostToAddDTO
    {
        public string? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Source { get; set; }
        public DateTime? PublishedDate { get; set; } = DateTime.UtcNow;
        public int Popularity { get; set; }
    }
}
