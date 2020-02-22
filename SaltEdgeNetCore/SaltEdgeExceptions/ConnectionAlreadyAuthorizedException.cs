using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ConnectionAlreadyAuthorizedException : Exception
    {
        public ConnectionAlreadyAuthorizedException()
        {
        }

        public ConnectionAlreadyAuthorizedException(string message) : base(message)
        {
        }
    }
}