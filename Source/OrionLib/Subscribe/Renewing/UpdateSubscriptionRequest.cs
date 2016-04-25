using System.Threading.Tasks;
using OrionLib.CommonContracts;
using OrionLib.Communication;

namespace OrionLib.Subscribe.Renewing
{
    public class UpdateSubscriptionRequest
    {
        private readonly OrionRequestFactory _orionRequestFactory;
        private readonly OrionSubscriptionConfiguration _configuration;

        public UpdateSubscriptionRequest(OrionRequestFactory orionRequestFactory, OrionSubscriptionConfiguration configuration)
        {
            _orionRequestFactory = orionRequestFactory;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(string subscriptionId)
        {
            var requestBody = new UpdateSubscriptionRequestDto(subscriptionId, _configuration.SubscriptionDuration);

            //todo get resurce from dedicated resource class instead of inline value
            using (var request = _orionRequestFactory.Build(OrionRequest.Method.POST, "v1/updateContextSubscription"))
            {
                await request
                .AddBody(requestBody)
                .ExecuteAsync<OrionResponseDto>();
            }
        }
    }
}