using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AttemptNotFoundException : Exception
    {
        public AttemptNotFoundException()
        {
        }

        public AttemptNotFoundException(string message) : base(message)
        {
        }
    }
}