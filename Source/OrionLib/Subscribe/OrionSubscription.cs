using System.Threading.Tasks;
using OrionLib.Subscribe.Management;
using OrionLib.Subscribe.Renewing;

namespace OrionLib.Subscribe
{
    public class OrionSubscription : IOrionSubscription
    {
        private readonly OrionSubscriptionManager _orionSubscriptionManager;
        private readonly SubscriptionRenewer _subscriptionRenewer;

        public OrionSubscription(OrionSubscriptionManager orionSubscriptionManager, SubscriptionRenewer subscriptionRenewer)
        {
            _orionSubscriptionManager = orionSubscriptionManager;
            _subscriptionRenewer = subscriptionRenewer;
        }

        public async Task Start()
        {
             await _orionSubscriptionManager.CreateAsync();
            _subscriptionRenewer.StartRenewing();
        }

        public void NotifyAboutOrionEvent(string requestContent)
        {
            _orionSubscriptionManager.HandleNotification(requestContent);
        }

        public void Dispose()
        {
            _subscriptionRenewer.Dispose();
        }
    }
}