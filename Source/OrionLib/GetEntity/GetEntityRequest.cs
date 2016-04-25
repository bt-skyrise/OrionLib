using System.Threading.Tasks;
using OrionLib.Communication;

namespace OrionLib.GetEntity
{
    public class GetEntityRequest
    {
        private readonly OrionRequestFactory _orionRequestFactory;

        public GetEntityRequest(OrionRequestFactory orionRequestFactory)
        {
            _orionRequestFactory = orionRequestFactory;
        }

        public async Task<OrionEntity> ExecuteAsync(string type, string id)
        {
            var resource = OrionResources.GetEntityResource(type, id);

            using (var request = _orionRequestFactory.Build(OrionRequest.Method.GET, resource))
            {
                var response = await request
                .ExecuteAsync<GetEntityResponseDto>();

                return response.ToEntity();
            }
        }
    }
}