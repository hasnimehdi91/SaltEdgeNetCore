using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ConnectionLimitReachedException : Exception
    {
        public ConnectionLimitReachedException()
        {
        }

        public ConnectionLimitReachedException(string message) : base(message)
        {
        }
    }
}