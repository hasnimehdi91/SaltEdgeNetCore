using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class FileNotSavedException : Exception
    {
        public FileNotSavedException()
        {
        }

        public FileNotSavedException(string message) : base(message)
        {
        }
    }
}