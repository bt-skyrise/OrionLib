using System;
using OrionLib.Utils;

namespace OrionLib.Subscribe.Renewing
{
    public class UpdateSubscriptionRequestDto
    {
        public string SubscriptionId { get; set; }
        public string Duration { get; set; }

        public UpdateSubscriptionRequestDto(string subscriptionId, TimeSpan duration)
        {
            SubscriptionId = subscriptionId;
            Duration = duration.ToIso8601();
        }
    }
}