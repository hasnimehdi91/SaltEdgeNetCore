using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ConnectionNotFoundException : Exception
    {
        public ConnectionNotFoundException()
        {
        }

        public ConnectionNotFoundException(string message) : base(message)
        {
        }
    }
}