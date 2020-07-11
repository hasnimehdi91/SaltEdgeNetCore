using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class CustomerLockedException : Exception
    {
        public CustomerLockedException()
        {
        }

        public CustomerLockedException(string message) : base(message)
        {
        }
    }
}