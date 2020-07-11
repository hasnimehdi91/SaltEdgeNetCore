using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ConnectionAlreadyProcessingException : Exception
    {
        public ConnectionAlreadyProcessingException()
        {
        }

        public ConnectionAlreadyProcessingException(string message) : base(message)
        {
        }
    }
}