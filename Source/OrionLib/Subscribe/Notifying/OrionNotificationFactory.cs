using Newtonsoft.Json;

namespace OrionLib.Subscribe.Notifying
{
    public class OrionNotificationFactory
    {
        private readonly OrionSubscriptionConfiguration _configuration;

        public OrionNotificationFactory(OrionSubscriptionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OrionNotification CreateFromResponse(string notificationResponse)
        {
            var notificationData = JsonConvert.DeserializeObject<OrionNotificationDto>(notificationResponse);

            return new OrionNotification(
                onEntityChanged: _configuration.OnEntityChanged,
                changedEntities: notificationData.GetEntities(),
                subscriptionId: notificationData.SubscriptionId);
        }
    }
}