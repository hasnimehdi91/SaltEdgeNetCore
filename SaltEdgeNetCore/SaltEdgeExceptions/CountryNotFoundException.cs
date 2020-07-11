using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class CountryNotFoundException : Exception
    {
        public CountryNotFoundException()
        {
        }

        public CountryNotFoundException(string message) : base(message)
        {
        }
    }
}