namespace SaltEdgeNetCore.Client.Endpoints
{
    public class SaltEdgeEndpointsV5
    {
        private SaltEdgeEndpointsV5(string value) { Value = value; }
        
        public string Value { get; }
        
        public static SaltEdgeEndpointsV5 CountryList => new SaltEdgeEndpointsV5("countries");
    }
}