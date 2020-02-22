using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException()
        {
        }

        public CustomerNotFoundException(string message) : base(message)
        {
        }
    }
}