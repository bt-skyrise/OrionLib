using System;
using System.Security.Policy;

namespace OrionLib.Subscribe.Creation
{
    public class OrionSubscriptionBuilder<TSubscription>
    {
        private readonly ISubscriptionFactory<TSubscription> _subscriptionFactory;

        private readonly Url _notificationAddress;
        private readonly string _entityName;
        private readonly string[] _observedAttributes;

        private EntityChanged _onEntityChanged = entities => { };
        private TimeSpan _subscriptionDuration = TimeSpan.FromMinutes(15);
        private TimeSpan _subscriptionRenewingTimeout = TimeSpan.FromMinutes(1);

        public OrionSubscriptionBuilder(ISubscriptionFactory<TSubscription> subscriptionFactory,
            Url notificationAddress, string entityName, params string[] observedAttributes)
        {
            _subscriptionFactory = subscriptionFactory;

            _notificationAddress = notificationAddress;
            _entityName = entityName;
            _observedAttributes = observedAttributes;
        }

        public OrionSubscriptionBuilder<TSubscription> OnEntityChanged(EntityChanged onEntityChanged)
        {
            _onEntityChanged = onEntityChanged;

            return this;
        }

        public OrionSubscriptionBuilder<TSubscription> WithSubscriptionTiming(TimeSpan duration, TimeSpan renewingTimeout)
        {
            _subscriptionDuration = duration;
            _subscriptionRenewingTimeout = renewingTimeout;

            return this;
        }

        public TSubscription Create()
        {
            return _subscriptionFactory.Create(new OrionSubscriptionConfiguration(
                entityType: _entityName,
                observedAttributes: _observedAttributes,
                subscriptionDuration: _subscriptionDuration,
                subscriptionRenewingTimeout: _subscriptionRenewingTimeout,
                notificationAddress: _notificationAddress,
                onEntityChanged: _onEntityChanged));
        }
    }
}