namespace SocialSyncPortal.BLL.Utilities.CustomExceptions
{
    public class SocialPostNotFoundException : Exception
    {
        public SocialPostNotFoundException()
        {
        }

        public SocialPostNotFoundException(string message)
            : base(message)
        {
        }

        public SocialPostNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
