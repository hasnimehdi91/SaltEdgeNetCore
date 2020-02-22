using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ConnectionDuplicatedException : Exception
    {
        public ConnectionDuplicatedException()
        {
        }

        public ConnectionDuplicatedException(string message) : base(message)
        {
        }
    }
}