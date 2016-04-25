namespace OrionLib.Subscribe.Notifying
{
    public class OrionNotification
    {
        private readonly EntityChanged _onEntityChanged;
        private readonly OrionEntity[] _changedEntities;
        private readonly string _subscriptionId;

        public OrionNotification(EntityChanged onEntityChanged, OrionEntity[] changedEntities, string subscriptionId)
        {
            _onEntityChanged = onEntityChanged;
            _changedEntities = changedEntities;
            _subscriptionId = subscriptionId;
        }

        public void HandleForSubscription(string expectedSubscriptionId)
        {
            if (expectedSubscriptionId == _subscriptionId)
            {
                _onEntityChanged(_changedEntities);
            }
        }
    }
}