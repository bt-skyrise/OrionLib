using System;
using System.Linq;
using System.Security.Policy;

namespace OrionLib.IntegrationTests
{
    public static class TestConfiguration
    {
        public static Url OrionAddress => new Url(OrionTestSettings.Default.OrionAddress);

        public static Url SubscriptionAddress => new Url(OrionTestSettings.Default.SubscriptionAddress);

        public static OrionConfiguration GetOrionConfiguration()
        {
            return new OrionConfiguration(OrionAddress, "tests");
        }
    }
}