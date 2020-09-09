namespace SaltEdgeNetCore
{
    public class SaltEdgeOptions
    {
        /// <summary>
        /// SSl private key for signing requests
        /// NOTE: this option is ignored when live mode is set to false
        /// </summary>
        public string PrivateKeyPath { get; set; }


        /// <summary>
        /// SSl public key for signing requests
        /// </summary>
        public string PublicKeyPath { get; set; }

        /// <summary>
        /// Live mode to let the library sign the request before sending it
        /// Note: By setting this option to true a private key must be set to sign the requests 
        /// </summary>
        public bool LiveMode { get; set; }

        /// <summary>
        /// Expiration in minute by default it is set to one minute
        /// </summary>
        public int WithExpiration { get; set; }

        /// <summary>
        /// Salt Edge App-Id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// SaltEdge secret
        /// </summary>
        public string Secret { get; set; }
    }
}