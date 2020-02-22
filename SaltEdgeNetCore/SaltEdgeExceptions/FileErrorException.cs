using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class FileErrorException : Exception
    {
        public FileErrorException()
        {
        }

        public FileErrorException(string message) : base(message)
        {
        }
    }
}