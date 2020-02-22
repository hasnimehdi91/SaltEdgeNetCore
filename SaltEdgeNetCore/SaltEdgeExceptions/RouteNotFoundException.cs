using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class RouteNotFoundException : Exception
    {
        public RouteNotFoundException()
        {
        }

        public RouteNotFoundException(string message) : base(message)
        {
        }
    }
}