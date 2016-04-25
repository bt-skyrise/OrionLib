using System;
using System.Security.Policy;
using System.Threading.Tasks;
using OrionLib.Subscribe;
using OrionLib.Subscribe.Creation;
using OrionLib.Subscribe.SelfHosting;
using OrionLib.QueryContext;

namespace OrionLib
{
    public interface IOrion
    {
        Task RemoveEntityAsync(string type, string id);
        Task CreateOrUpdateEntityAsync(OrionEntity orionEntity);
        Task<OrionEntity> GetEntityAsync(string type, string id);
        Task<OrionEntity> TryGetEntityAsync(string type, string id);

        Task UpdateAttributeAsync(string type, string id, string attributeName, string newValue);
        Task RemoveAttributesAsync(string type, string id, params string[] attributeNames);

        OrionQuery QueryEntities(string type);
        OrionQuery QueryEntities();

        OrionSubscriptionBuilder<IOrionSubscription> CreateSubscription(Url notificationAddress, string entityName, params string[] observedAttributes);
        OrionSubscriptionBuilder<ISelfHostedOrionSubscription> CreateSelfHostedSubscription(Url notificationAddress, Url listenerAddress, string entityName, params string[] observedAttributes);
    }
}