using OrionLib.CommonContracts;

namespace OrionLib.GetEntity
{
    public class GetEntityResponseDto : OrionResponseDto
    {
        public ContextElementDto ContextElement { get; set; }
    }

    public static class GetEntityResponseDtoExtensions
    {
        public static OrionEntity ToEntity(this GetEntityResponseDto responseDto)
        {
            return responseDto.ContextElement.ToEntity();
        }
    }
}