using Newtonsoft.Json;

namespace SocialSyncPortal.DTO.DTOs.Job
{
    public class TumblrResponseDTO
    {
        public TumblrMeta Meta { get; set; }
        public TumblrResponseData Response { get; set; }

        public class TumblrMeta
        {
            public int Status { get; set; }
            public string Msg { get; set; }
        }

        public class TumblrResponseData
        {
            public List<TumblrPost> Posts { get; set; }
        }

        public class TumblrPost
        {
            public long Id { get; set; }
            public string Reblog_Key { get; set; }
            public string PostUrl { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
            public int Note_Count { get; set; }
            public long Timestamp { get; set; }
        }
    }
}
