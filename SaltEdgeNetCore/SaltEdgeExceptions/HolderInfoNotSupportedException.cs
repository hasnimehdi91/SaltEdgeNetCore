using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class HolderInfoNotSupportedException : Exception
    {
        public HolderInfoNotSupportedException()
        {
        }

        public HolderInfoNotSupportedException(string message) : base(message)
        {
        }
    }
}