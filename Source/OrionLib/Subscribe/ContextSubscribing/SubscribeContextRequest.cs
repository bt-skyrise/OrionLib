using System.Threading.Tasks;
using OrionLib.Communication;

namespace OrionLib.Subscribe.ContextSubscribing
{
    public class SubscribeContextRequest
    {
        private readonly OrionRequestFactory _orionRequestFactory;
        private readonly OrionSubscriptionConfiguration _configuration;

        public SubscribeContextRequest(OrionRequestFactory orionRequestFactory, OrionSubscriptionConfiguration configuration)
        {
            _configuration = configuration;
            _orionRequestFactory = orionRequestFactory;
        }

        public async Task<SubscribeContextResponseDto> ExecuteAsync()
        {
            var requestBody = new SubscribeContextRequestDto(
                _configuration.EntityType,
                _configuration.ObservedAttributes,
                _configuration.NotificationAddress,
                _configuration.SubscriptionDuration);

            //todo get resurce from dedicated resource class instead of inline value
            using (var request = _orionRequestFactory.Build(OrionRequest.Method.POST, "v1/subscribeContext"))
            {
                return await request
                    .AddBody(requestBody)
                    .ExecuteAsync<SubscribeContextResponseDto>();
            }
        }
    }
}