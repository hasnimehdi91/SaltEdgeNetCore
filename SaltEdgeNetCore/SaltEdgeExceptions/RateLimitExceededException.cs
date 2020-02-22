using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class RateLimitExceededException : Exception
    {
        public RateLimitExceededException()
        {
        }

        public RateLimitExceededException(string message) : base(message)
        {
        }
    }
}