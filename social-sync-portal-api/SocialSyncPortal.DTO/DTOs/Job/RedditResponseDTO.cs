namespace SocialSyncPortal.DTO.DTOs.Job
{
    public class RedditResponseDTO
    {
        public RedditData Data { get; set; }

        public class RedditData
        {
            public List<RedditChild> Children { get; set; }
        }

        public class RedditChild
        {
            public RedditPostData Data { get; set; }
        }

        public class RedditPostData
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Author_Fullname { get; set; }
            public int Ups { get; set; }
            public double Created { get; set; }
        }
    }
}
