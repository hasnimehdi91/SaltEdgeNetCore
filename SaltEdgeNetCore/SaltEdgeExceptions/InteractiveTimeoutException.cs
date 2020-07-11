using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InteractiveTimeoutException : Exception
    {
        public InteractiveTimeoutException()
        {
        }

        public InteractiveTimeoutException(string message) : base(message)
        {
        }
    }
}