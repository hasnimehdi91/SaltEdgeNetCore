using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ExecutionTimeoutException : Exception
    {
        public ExecutionTimeoutException()
        {
        }

        public ExecutionTimeoutException(string message) : base(message)
        {
        }
    }
}