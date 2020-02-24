namespace SaltEdgeNetCore.Client.Endpoints
{
    public class SaltEdgeEndpointsV5
    {
        private SaltEdgeEndpointsV5(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static SaltEdgeEndpointsV5 CountryList => new SaltEdgeEndpointsV5("countries");
        public static SaltEdgeEndpointsV5 Providers => new SaltEdgeEndpointsV5("providers");
        public static SaltEdgeEndpointsV5 Customers => new SaltEdgeEndpointsV5("customers");
        public static SaltEdgeEndpointsV5 ConnectSessions => new SaltEdgeEndpointsV5("connect_sessions");
        public static SaltEdgeEndpointsV5 OauthProviders => new SaltEdgeEndpointsV5("oauth_providers");
        public static SaltEdgeEndpointsV5 Connections => new SaltEdgeEndpointsV5("connections");
        public static SaltEdgeEndpointsV5 HolderInfo => new SaltEdgeEndpointsV5("holder_info");
        public static SaltEdgeEndpointsV5 Attempts => new SaltEdgeEndpointsV5("attempts");
        public static SaltEdgeEndpointsV5 Accounts => new SaltEdgeEndpointsV5("accounts");
        public static SaltEdgeEndpointsV5 Transactions => new SaltEdgeEndpointsV5("transactions");
        public static SaltEdgeEndpointsV5 Consents => new SaltEdgeEndpointsV5("consents");
        public static SaltEdgeEndpointsV5 Categories => new SaltEdgeEndpointsV5("categories");
        public static SaltEdgeEndpointsV5 Currencies => new SaltEdgeEndpointsV5("currencies");
        public static SaltEdgeEndpointsV5 Assets => new SaltEdgeEndpointsV5("assets");
        public static SaltEdgeEndpointsV5 Rates => new SaltEdgeEndpointsV5("rates");
        public static SaltEdgeEndpointsV5 Merchants => new SaltEdgeEndpointsV5("merchants");
    }
}