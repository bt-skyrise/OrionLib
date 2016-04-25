using System;
using System.Threading.Tasks;
using OrionLib.Subscribe.ContextSubscribing;
using OrionLib.Subscribe.Notifying;
using OrionLib.Subscribe.Renewing;

namespace OrionLib.Subscribe.Management
{
    public class OrionSubscriptionManager
    {
        interface ISubscriptionState
        {
            Task CreateAsync();
            Task UpdateAsync();
            void HandleNotification(string notificationContent);
        }

        public class NotCreatedSubscription : ISubscriptionState
        {
            private readonly OrionSubscriptionManager _context;
            private readonly SubscribeContextRequest _subscribeContext;

            public NotCreatedSubscription(OrionSubscriptionManager context, SubscribeContextRequest subscribeContext)
            {
                _context = context;
                _subscribeContext = subscribeContext;
            }

            public async Task CreateAsync()
            {
                var queryResult = await _subscribeContext.ExecuteAsync();
               var subscriptionId = queryResult.GetSubscriptionId();

                _context.SetCreatedState(subscriptionId);
            }

            public Task UpdateAsync()
            {
                throw new InvalidOperationException("Subscription not created");
            }

            public void HandleNotification(string notificationContent)
            {
                //todo: logging
                #region Long comment
                /* It is possible that in the very short period between subscribing to the orion and set the created state with new subscription id we get some event from orion
                 * We just ignore it for now maybe some queueing should be consider or at least logging in the future
                 */
                #endregion Long comment
            }
        }

        public class CreatedSubscription : ISubscriptionState
        {
            private readonly string _subscriptionId;
            private readonly UpdateSubscriptionRequest _updateSubscription;
            private readonly OrionNotificationFactory _orionNotificationFactory;

            public CreatedSubscription(string subscriptionId, UpdateSubscriptionRequest updateSubscription, OrionNotificationFactory orionNotificationFactory)
            {
                _subscriptionId = subscriptionId;
                _updateSubscription = updateSubscription;
                _orionNotificationFactory = orionNotificationFactory;
            }

            public Task CreateAsync()
            {
                throw new InvalidOperationException("Subscription already created");
            }

            public async Task UpdateAsync()
            {
                await _updateSubscription.ExecuteAsync(_subscriptionId);
            }

            public void HandleNotification(string notificationContent)
            {
                _orionNotificationFactory
                    .CreateFromResponse(notificationContent)
                    .HandleForSubscription(_subscriptionId);
            }
        }

        private readonly UpdateSubscriptionRequest _updateSubscription;
        private readonly SubscribeContextRequest _subscribeContext;
        private readonly OrionNotificationFactory _orionNotificationFactory;

        private ISubscriptionState _subscriptionState;

        public OrionSubscriptionManager(UpdateSubscriptionRequest updateSubscription, SubscribeContextRequest subscribeContext, OrionNotificationFactory orionNotificationFactory)
        {
            _updateSubscription = updateSubscription;
            _subscribeContext = subscribeContext;
            _orionNotificationFactory = orionNotificationFactory;

            SetNotCreatedState();
        }

        private void SetNotCreatedState()
        {
            _subscriptionState = new NotCreatedSubscription(this, _subscribeContext);
        }

        private void SetCreatedState(string subscriptionId)
        {
            _subscriptionState = new CreatedSubscription(subscriptionId, _updateSubscription, _orionNotificationFactory);
        }

        public async Task CreateAsync()
        {
            await _subscriptionState.CreateAsync();
        }

        public async Task UpdateAsync()
        {
            await _subscriptionState.UpdateAsync();
        }

        public void HandleNotification(string notificationContent)
        {
            _subscriptionState.HandleNotification(notificationContent);
        }
    }
}