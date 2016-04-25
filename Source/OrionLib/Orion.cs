using System;
using System.Security.Policy;
using System.Threading.Tasks;
using OrionLib.CreateOrUpdateEntity;
using OrionLib.GetEntity;
using OrionLib.QueryContext;
using OrionLib.RemoveAttribute;
using OrionLib.RemoveEntity;
using OrionLib.Subscribe;
using OrionLib.Subscribe.Creation;
using OrionLib.Subscribe.SelfHosting;
using OrionLib.UpdateAttribute;

namespace OrionLib
{
    public class Orion : IOrion
    {
        private readonly CreateOrUpdateEntityRequest _createOrUpdateEntityRequest;
        private readonly GetEntityRequest _getEntityRequest;
        private readonly TryGetEntityRequest _tryGetEntityRequest;
        private readonly UpdateAttributeRequest _updateAttributeRequest;
        private readonly SubscriptionFactory _subscriptionFactory;
        private readonly OrionQueryFactory _orionQueryFactory;
        private readonly RemoveEntityRequest _removeEntityRequest;
        private readonly RemoveAttributesRequest _removeAttributesRequest;

        public Orion(
            CreateOrUpdateEntityRequest createOrUpdateEntityRequest,
            GetEntityRequest getEntityRequest,
            TryGetEntityRequest tryGetEntityRequest,
            UpdateAttributeRequest updateAttributeRequest,
            SubscriptionFactory subscriptionFactory,
            OrionQueryFactory orionQueryFactory,
            RemoveEntityRequest removeEntityRequest,
            RemoveAttributesRequest removeAttributesRequest)
        {
            _createOrUpdateEntityRequest = createOrUpdateEntityRequest;
            _getEntityRequest = getEntityRequest;
            _tryGetEntityRequest = tryGetEntityRequest;
            _updateAttributeRequest = updateAttributeRequest;
            _subscriptionFactory = subscriptionFactory;
            _orionQueryFactory = orionQueryFactory;
            _removeEntityRequest = removeEntityRequest;
            _removeAttributesRequest = removeAttributesRequest;
        }

        public OrionSubscriptionBuilder<IOrionSubscription> CreateSubscription(Url notificationAddress, string entityName, params string[] observedAttributes)
        {
            return new OrionSubscriptionBuilder<IOrionSubscription>(
                _subscriptionFactory,
                notificationAddress,
                entityName,
                observedAttributes);
        }

        public OrionSubscriptionBuilder<ISelfHostedOrionSubscription> CreateSelfHostedSubscription(Url notificationAddress, Url listenerAddress, string entityName, params string[] observedAttributes)
        {
            return new OrionSubscriptionBuilder<ISelfHostedOrionSubscription>(
                new SelfHostedSubscriptionFactory(listenerAddress, _subscriptionFactory),
                notificationAddress,
                entityName,
                observedAttributes);
        }

        public async Task RemoveEntityAsync(string type, string id)
        {
            await _removeEntityRequest.ExecuteAsync(type, id);
        }

        public async Task CreateOrUpdateEntityAsync(OrionEntity orionEntity)
        {
            await _createOrUpdateEntityRequest.ExecuteAsync(orionEntity);
        }

        public async Task<OrionEntity> GetEntityAsync(string type, string id)
        {
            return await _getEntityRequest.ExecuteAsync(type, id);
        }

        public async Task<OrionEntity> TryGetEntityAsync(string type, string id)
        {
            return await _tryGetEntityRequest.ExecuteAsync(type, id);
        }

        public async Task UpdateAttributeAsync(string type, string id, string attributeName, string newValue)
        {
            await _updateAttributeRequest.ExecuteAsync(type, id, attributeName, newValue);
        }

        public async Task RemoveAttributesAsync(string type, string id, params string[] attributeNames)
        {
            await _removeAttributesRequest.ExecuteAsync(type, id, attributeNames);
        }

        public OrionQuery QueryEntities(string type)
        {
            return _orionQueryFactory.CreateForSpecificType(type);
        }

        public OrionQuery QueryEntities()
        {
            return _orionQueryFactory.CreateForAllTypes();
        }
    }
}