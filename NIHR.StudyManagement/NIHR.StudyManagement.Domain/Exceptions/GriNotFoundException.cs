using System;

namespace NIHR.StudyManagement.Domain.Exceptions
{
    public class GriNotFoundException : Exception
    {
        public GriNotFoundException()
        {
        }

        public GriNotFoundException(string message)
            : base(message)
        {
        }

        public GriNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
