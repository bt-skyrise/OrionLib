using System.Collections.Generic;
using System.Linq;

namespace OrionLib.CommonContracts
{
    public class ContextResponseDto
    {
        public ContextElementDto ContextElement { get; set; }
        public OrionStatusDto StatusCode { get; set; }
    }

    public static class ContextResponseDtoExtensions
    {
        public static bool IsValid(this ContextResponseDto contextResponse)
        {
            return contextResponse.StatusCode.IsValid();
        }

        public static OrionEntity ToEntity(this ContextResponseDto contextResponse)
        {
            return contextResponse.ContextElement.ToEntity();
        }

        public static OrionEntity[] GetValidEntities(this IEnumerable<ContextResponseDto> queryContextResponse)
        {
            return queryContextResponse
                .Where(response => response.IsValid())
                .Select(response => response.ToEntity())
                .ToArray();
        }
    }
}