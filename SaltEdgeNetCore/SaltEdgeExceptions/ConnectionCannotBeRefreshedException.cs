using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ConnectionCannotBeRefreshedException : Exception
    {
        public ConnectionCannotBeRefreshedException()
        {
        }

        public ConnectionCannotBeRefreshedException(string message) : base(message)
        {
        }
    }
}