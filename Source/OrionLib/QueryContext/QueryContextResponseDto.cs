using System.Collections.Generic;
using OrionLib.CommonContracts;

namespace OrionLib.QueryContext
{
    public class QueryContextResponseDto : OrionResponseDto
    {
        public List<ContextResponseDto> ContextResponses { get; set; }

        public static QueryContextResponseDto Empty()
        {
            return new QueryContextResponseDto
            {
                ContextResponses = new List<ContextResponseDto>(),
                StatusCode = OrionStatusDto.Ok(),
                ErrorCode = OrionStatusDto.ErrorWithEmtpyDetails()                
            };
        }
    }

    public static class QueryContextResponseDtoExtensions
    {
        public static OrionEntity[] GetEntities(this QueryContextResponseDto queryContextResponseDto)
        {
            return queryContextResponseDto.ContextResponses.GetValidEntities();
        }
    }
}