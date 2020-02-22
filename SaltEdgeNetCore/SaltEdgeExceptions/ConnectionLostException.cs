using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ConnectionLostException : Exception
    {
        public ConnectionLostException()
        {
        }

        public ConnectionLostException(string message) : base(message)
        {
        }
    }
}