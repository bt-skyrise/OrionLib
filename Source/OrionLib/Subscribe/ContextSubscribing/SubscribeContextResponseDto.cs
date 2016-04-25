using OrionLib.CommonContracts;

namespace OrionLib.Subscribe.ContextSubscribing
{
    public class SubscribeContextResponseDto : OrionResponseDto
    {
        public SubscribeResponseDto SubscribeResponse { get; set; }
    }

    public static class SubscriptionResponseExtensions
    {
        public static string GetSubscriptionId(this SubscribeContextResponseDto subscribeContextResponseDto)
        {
            return subscribeContextResponseDto.SubscribeResponse.SubscriptionId;
        }
    }
}