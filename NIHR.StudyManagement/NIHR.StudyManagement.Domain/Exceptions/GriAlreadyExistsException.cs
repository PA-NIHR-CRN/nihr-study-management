using System;

namespace NIHR.StudyManagement.Domain.Exceptions
{
    public class GriAlreadyExistsException : Exception
    {
        public GriAlreadyExistsException()
        {
        }

        public GriAlreadyExistsException(string message)
            : base(message)
        {
        }

        public GriAlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
