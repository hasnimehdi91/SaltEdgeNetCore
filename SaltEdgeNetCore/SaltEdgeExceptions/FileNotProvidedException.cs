using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class FileNotProvidedException : Exception
    {
        public FileNotProvidedException()
        {
        }

        public FileNotProvidedException(string message) : base(message)
        {
        }
    }
}