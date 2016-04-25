namespace OrionLib.CommonContracts
{
    public class OrionResponseDto
    {
        // Orion communicates request's status either by retutning OrionError, ErrorCode, StatusCode or nothing.

        public OrionStatusDto OrionError { get; set; }
        public OrionStatusDto ErrorCode { get; set; }
        public OrionStatusDto StatusCode { get; set; }
    }

    public static class OrionResponseDtoExtensions
    {
        public static OrionStatusDto TryGetError(this OrionResponseDto orionResponse)
        {
            if (orionResponse.OrionError != null && orionResponse.OrionError.Code != "200")
            {
                return orionResponse.OrionError;
            }

            if (orionResponse.ErrorCode != null && orionResponse.ErrorCode.Code != "200")
            {
                return orionResponse.ErrorCode;
            }

            if (orionResponse.StatusCode != null && !orionResponse.StatusCode.IsValid())
            {
                return orionResponse.StatusCode;
            }

            return null;
        }
    }
}