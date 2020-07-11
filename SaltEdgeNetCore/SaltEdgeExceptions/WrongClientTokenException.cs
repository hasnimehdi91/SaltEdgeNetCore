using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class WrongClientTokenException : Exception
    {
        public WrongClientTokenException()
        {
        }

        public WrongClientTokenException(string message) : base(message)
        {
        }
    }
}