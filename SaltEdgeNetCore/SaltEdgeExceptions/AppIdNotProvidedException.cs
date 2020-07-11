using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AppIdNotProvidedException : Exception
    {
        public AppIdNotProvidedException()
        {
        }

        public AppIdNotProvidedException(string message) : base(message)
        {
        }
    }
}