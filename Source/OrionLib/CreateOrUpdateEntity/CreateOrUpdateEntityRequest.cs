using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OrionLib.Communication;

namespace OrionLib.CreateOrUpdateEntity
{
    public class CreateOrUpdateEntityRequest
    {
        private readonly OrionRequestFactory _orionRequestFactory;

        public CreateOrUpdateEntityRequest(OrionRequestFactory orionRequestFactory)
        {
            _orionRequestFactory = orionRequestFactory;
        }

        public async Task ExecuteAsync(OrionEntity orionEntity)
        {
            var resource = OrionResources.GetEntityResource(orionEntity.Type, orionEntity.Id);

            var requestBody = new CreateOrUpdateEntityRequestDto(orionEntity);

            using (var request = _orionRequestFactory.Build(OrionRequest.Method.POST, resource))
            {
                await request
                    .AddBody(requestBody)
                    .ExecuteAsync();
            }
        }
    }
}