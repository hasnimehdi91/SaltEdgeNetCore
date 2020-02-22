using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ClientRestrictedException : Exception
    {
        public ClientRestrictedException()
        {
        }

        public ClientRestrictedException(string message) : base(message)
        {
        }
    }
}