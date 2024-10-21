namespace SocialSyncPortal.DTO.DTOs.SocialPost
{
    public class SocialPostDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public DateTime PublishedDate { get; set; }
        public int Popularity { get; set; }
    }
}
