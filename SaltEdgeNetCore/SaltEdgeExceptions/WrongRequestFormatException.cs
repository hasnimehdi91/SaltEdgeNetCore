using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class WrongRequestFormatException : Exception
    {
        public WrongRequestFormatException()
        {
        }

        public WrongRequestFormatException(string message)
        {
        }
    }
}