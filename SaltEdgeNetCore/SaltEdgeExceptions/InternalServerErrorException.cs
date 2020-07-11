using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException()
        {
        }

        public InternalServerErrorException(string message) : base(message)
        {
        }
    }
}