using System.Security.Policy;
using OrionLib.Subscribe.SelfHosting;

namespace OrionLib.Subscribe.Creation
{
    public class SelfHostedSubscriptionFactory : ISubscriptionFactory<ISelfHostedOrionSubscription>
    {
        private readonly Url _listenerAddress;
        private readonly SubscriptionFactory _subscriptionFactory;

        public SelfHostedSubscriptionFactory(Url listenerAddress, SubscriptionFactory subscriptionFactory)
        {
            _listenerAddress = listenerAddress;
            _subscriptionFactory = subscriptionFactory;
        }

        public ISelfHostedOrionSubscription Create(OrionSubscriptionConfiguration configuration)
        {
            var subscription = _subscriptionFactory.Create(configuration);

            return new SelfHostedOrionSubscription(subscription, _listenerAddress);
        }
    }
}