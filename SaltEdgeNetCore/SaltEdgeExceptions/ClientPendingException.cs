using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ClientPendingException : Exception
    {
        public ClientPendingException()
        {
        }

        public ClientPendingException(string message) : base(message)
        {
        }
    }
}