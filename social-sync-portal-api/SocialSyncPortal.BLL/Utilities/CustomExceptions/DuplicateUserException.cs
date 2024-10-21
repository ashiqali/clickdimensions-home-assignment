using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialSyncPortal.BLL.Utilities.CustomExceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException()
        {
        }

        public DuplicateUserException(string message)
            : base(message)
        {
        }

        public DuplicateUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
