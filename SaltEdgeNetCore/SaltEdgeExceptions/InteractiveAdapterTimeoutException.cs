using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InteractiveAdapterTimeoutException : Exception
    {
        public InteractiveAdapterTimeoutException()
        {
        }

        public InteractiveAdapterTimeoutException(string message) : base(message)
        {
        }
    }
}