using System.Threading.Tasks;
using OrionLib.Communication;

namespace OrionLib.UpdateAttribute
{
    public class UpdateAttributeRequest
    {
        private readonly OrionRequestFactory _orionRequestFactory;

        public UpdateAttributeRequest(OrionRequestFactory orionRequestFactory)
        {
            _orionRequestFactory = orionRequestFactory;
        }

        public async Task ExecuteAsync(string type, string id, string attributeName, string newValue)
        {
            var requestBody = new UpdateAttributeRequestDto(newValue);

            var resource = OrionResources.GetAttributeResource(type, id, attributeName);

            using (var request = _orionRequestFactory.Build(OrionRequest.Method.POST, resource))
            {
                await request
                   .AddBody(requestBody)
                   .ExecuteAsync();
            }
        }
    }
}