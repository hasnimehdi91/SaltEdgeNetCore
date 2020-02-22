using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ValueOutOfRangeException : Exception
    {
        public ValueOutOfRangeException()
        {
        }

        public ValueOutOfRangeException(string message) : base(message)
        {
        }
    }
}