using OrionLib.Communication;
using OrionLib.CreateOrUpdateEntity;
using OrionLib.GetEntity;
using OrionLib.QueryContext;
using OrionLib.RemoveAttribute;
using OrionLib.RemoveEntity;
using OrionLib.Subscribe.Creation;
using OrionLib.UpdateAttribute;

namespace OrionLib
{
    public class OrionFactory
    {
        public IOrion Create(OrionConfiguration configuration)
        {
            var orionRequestFactory = new OrionRequestFactory(configuration);
            var getEntityRequest = new GetEntityRequest(orionRequestFactory);

            return new Orion(
                new CreateOrUpdateEntityRequest(orionRequestFactory),
                getEntityRequest,
                new TryGetEntityRequest(getEntityRequest),
                new UpdateAttributeRequest(orionRequestFactory),
                new SubscriptionFactory(orionRequestFactory),
                new OrionQueryFactory(
                    new QueryContextRequestFactory(orionRequestFactory)),
                new RemoveEntityRequest(orionRequestFactory),
                new RemoveAttributesRequest(orionRequestFactory));
        }
    }
}