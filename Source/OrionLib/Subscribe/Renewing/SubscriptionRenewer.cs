using System;
using OrionLib.Subscribe.Management;

namespace OrionLib.Subscribe.Renewing
{
    public class SubscriptionRenewer : IDisposable
    {
        private readonly RecurringTask _recurringTask;

        public SubscriptionRenewer(OrionSubscriptionConfiguration configuration, OrionSubscriptionManager orionSubscriptionManager)
        {
            _recurringTask = new RecurringTask(
                interval: configuration.SubscriptionRenewingTimeout,
                action: orionSubscriptionManager.UpdateAsync);
        }

        public void StartRenewing()
        {
            _recurringTask.Start();
        }

        public void Dispose()
        {
            _recurringTask.Dispose();
        }
    }
}