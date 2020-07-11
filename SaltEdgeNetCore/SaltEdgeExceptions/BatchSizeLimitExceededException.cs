using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class BatchSizeLimitExceededException : Exception
    {
        public BatchSizeLimitExceededException()
        {
        }

        public BatchSizeLimitExceededException(string message) : base(message)
        {
        }
    }
}