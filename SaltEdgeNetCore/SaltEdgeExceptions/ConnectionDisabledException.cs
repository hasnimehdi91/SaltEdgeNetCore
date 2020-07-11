using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ConnectionDisabledException : Exception
    {
        public ConnectionDisabledException()
        {
        }

        public ConnectionDisabledException(string message) : base(message)
        {
        }
    }
}