using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ConnectionFailedException : Exception
    {
        public ConnectionFailedException()
        {
        }

        public ConnectionFailedException(string message) : base(message)
        {
        }
    }
}