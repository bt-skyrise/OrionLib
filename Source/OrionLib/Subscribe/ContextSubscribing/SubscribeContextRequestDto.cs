using System;
using System.Security.Policy;
using OrionLib.CommonContracts;
using OrionLib.Subscribe.Notifying;
using OrionLib.Utils;

namespace OrionLib.Subscribe.ContextSubscribing
{
    public class SubscribeContextRequestDto
    {
        public EntityQueryDto[] Entities { get; private set; }
        public NotifyConditionDto[] NotifyConditions { get; private set; }
        public string Reference { get; private set; }
        public string Duration { get; private set; }
        public string Throttling { get; private set; }

        public SubscribeContextRequestDto(string entityType, string[] observedAttributes, Url notificationAddress, TimeSpan duration)
        {
            Entities = new[]
            {
                EntityQueryDto.QueryAllEntitiesOfType(entityType)
            };

            // We want to receive notifications on attributes' change.
            NotifyConditions = new[]
            {
                NotifyConditionDto.OnAttributesChange(observedAttributes)
            };

            // Orion will fire notification to this address.
            Reference = notificationAddress.Value;

            // Subscription will expire after that time.
            Duration = duration.ToIso8601();

            // We don't need Orion's throttling.
            Throttling = TimeSpan.Zero.ToIso8601();
        }
    }
}