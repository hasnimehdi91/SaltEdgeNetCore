using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class CategorizationLimitReachedException : Exception
    {
        public CategorizationLimitReachedException()
        {
        }

        public CategorizationLimitReachedException(string message) : base(message)
        {
        }
    }
}