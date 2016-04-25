using System;
using System.Security.Policy;

namespace OrionLib
{
    public class OrionConfiguration
    {
        public Url OrionUrl { get; private set; }
        public string Service { get; private set; }
        public string ServicePath { get; private set; }
        public string ComponentAuthToken { get; private set; }

        public OrionConfiguration(Url orionUrl, string service = "", string servicePath = "/", string componentAuthToken = null)
        {
            if (orionUrl == null) throw new ArgumentNullException(nameof(orionUrl));
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (servicePath == null) throw new ArgumentNullException(nameof(servicePath));
            
            if (!servicePath.StartsWith("/"))
            {
                throw new ArgumentOutOfRangeException(nameof(servicePath), "Service path must start with '/'.");
            }

            OrionUrl = orionUrl;
            ComponentAuthToken = componentAuthToken;
            Service = service;
            ServicePath = servicePath;
        }
    }
}