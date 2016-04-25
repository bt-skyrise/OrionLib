using System.Threading.Tasks;
using OrionLib.Communication;
using OrionLib.GetEntity;

namespace OrionLib.RemoveEntity
{
    public class RemoveEntityRequest
    {
        private readonly OrionRequestFactory _orionRequestFactory;

        public RemoveEntityRequest(OrionRequestFactory orionRequestFactory)
        {
            _orionRequestFactory = orionRequestFactory;
        }

        public async Task ExecuteAsync(string type, string id)
        {
            var resource = OrionResources.GetEntityResource(type, id);

            using (var request = _orionRequestFactory.Build(OrionRequest.Method.DELETE, resource))
            {
                await request.ExecuteAsync();
            }
        }
    }
}