namespace SaltEdgeNetCore.Client.Endpoints
{
    public class SaltEdgeEndpointsV4
    {
        private SaltEdgeEndpointsV4(string value) { Value = value; }
        
        public string Value { get; }
        
        public static SaltEdgeEndpointsV4 CountryList => new SaltEdgeEndpointsV4("countries");
    }
}