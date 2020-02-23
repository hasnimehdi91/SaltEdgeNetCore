namespace SaltEdgeNetCore.Client.Endpoints
{
    public class SaltEdgeEndpointsV5
    {
        private SaltEdgeEndpointsV5(string value) { Value = value; }
        
        public string Value { get; }
        
        public static SaltEdgeEndpointsV5 CountryList => new SaltEdgeEndpointsV5("countries");
        public static SaltEdgeEndpointsV5 Providers => new SaltEdgeEndpointsV5("providers");
        public static SaltEdgeEndpointsV5 Customers => new SaltEdgeEndpointsV5("customers");
        public static SaltEdgeEndpointsV5 ConnectSessions => new SaltEdgeEndpointsV5("connect_sessions");
    }
}