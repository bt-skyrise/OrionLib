using System.Collections.Generic;
using OrionLib.CommonContracts;

namespace OrionLib.Subscribe.Notifying
{
    public class OrionNotificationDto
    {
        public List<ContextResponseDto> ContextResponses { get; set; }
        public string SubscriptionId { get; set; }
    }

    public static class OrionNotificationExtensions
    {
        public static OrionEntity[] GetEntities(this OrionNotificationDto orionNotificationDto)
        {
            return orionNotificationDto.ContextResponses.GetValidEntities();
        }
    }
}