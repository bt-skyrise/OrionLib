using OrionLib.Communication;
using OrionLib.Subscribe.ContextSubscribing;
using OrionLib.Subscribe.Management;
using OrionLib.Subscribe.Notifying;
using OrionLib.Subscribe.Renewing;

namespace OrionLib.Subscribe.Creation
{
    public class SubscriptionFactory : ISubscriptionFactory<IOrionSubscription>
    {
        private readonly OrionRequestFactory _orionRequestFactory;

        public SubscriptionFactory(OrionRequestFactory orionRequestFactory)
        {
            _orionRequestFactory = orionRequestFactory;
        }

        public IOrionSubscription Create(OrionSubscriptionConfiguration configuration)
        {
            var orionSubscriptionManager = new OrionSubscriptionManager(
                new UpdateSubscriptionRequest(_orionRequestFactory, configuration),
                new SubscribeContextRequest(_orionRequestFactory, configuration),
                new OrionNotificationFactory(configuration));

            var subscriptionRenewer = new SubscriptionRenewer(configuration, orionSubscriptionManager);

            return new OrionSubscription(orionSubscriptionManager, subscriptionRenewer);
        }
    }
}