using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class FetchingTimeoutException : Exception
    {
        public FetchingTimeoutException()
        {
        }

        public FetchingTimeoutException(string message) : base(message)
        {
        }
    }
}