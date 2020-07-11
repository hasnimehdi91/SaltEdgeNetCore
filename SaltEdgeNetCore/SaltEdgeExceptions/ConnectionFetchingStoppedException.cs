using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ConnectionFetchingStoppedException : Exception
    {
        public ConnectionFetchingStoppedException()
        {
        }

        public ConnectionFetchingStoppedException(string message) : base(message)
        {
        }
    }
}