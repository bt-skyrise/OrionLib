using System;
using System.Linq;
using System.Security.Policy;

namespace OrionLib.Subscribe
{
    public class OrionSubscriptionConfiguration
    {
        public string EntityType { get; private set; }
        public string[] ObservedAttributes { get; private set; }

        public TimeSpan SubscriptionDuration { get; private set; }
        public TimeSpan SubscriptionRenewingTimeout { get; private set; }

        public Url NotificationAddress { get; private set; }
        public EntityChanged OnEntityChanged { get; private set; }

        public OrionSubscriptionConfiguration(string entityType, string[] observedAttributes, TimeSpan subscriptionDuration, 
            TimeSpan subscriptionRenewingTimeout, Url notificationAddress, EntityChanged onEntityChanged)
        {
            if (!observedAttributes.Any())
            {
                throw new ArgumentException("At least one attribute has to be observed.", nameof(observedAttributes));
            }

            if (subscriptionRenewingTimeout > subscriptionDuration)
            {
                throw new ArgumentException("Renewing timeout must be lesser than subscription duration.");
            }

            EntityType = entityType;
            ObservedAttributes = observedAttributes;
            SubscriptionDuration = subscriptionDuration;
            SubscriptionRenewingTimeout = subscriptionRenewingTimeout;
            NotificationAddress = notificationAddress;
            OnEntityChanged = onEntityChanged;
        }
    }
}