using System.Security.Policy;

namespace OrionLib.Subscribe.SelfHosting
{
    public class SelfHostedOrionSubscription : ISelfHostedOrionSubscription
    {
        private readonly IOrionSubscription _orionSubscription;
        private readonly OwinSubscriptionListener _subscriptionListener;

        public SelfHostedOrionSubscription(IOrionSubscription orionSubscription, Url lisenerAddress)
        {
            _orionSubscription = orionSubscription;
            _subscriptionListener = new OwinSubscriptionListener(lisenerAddress, orionSubscription);
        }

        public void Start()
        {
            _subscriptionListener.Start();
            _orionSubscription.Start();
        }

        public void Dispose()
        {
            _orionSubscription.Dispose();
            _subscriptionListener.Dispose();
        }
    }
}