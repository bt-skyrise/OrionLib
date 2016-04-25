namespace OrionLib.CommonContracts
{
    public class OrionStatusDto
    {
        public string Code { get; set; }
        public string ReasonPhrase { get; set; }
        public string Details { get; set; }

        public static OrionStatusDto Ok()
        {
            return new OrionStatusDto
            {
                Code = "200",
                ReasonPhrase = "OK"
            };
        }

        public static OrionStatusDto ErrorWithEmtpyDetails()
        {
            return new OrionStatusDto
            {
                Code = "200",
                ReasonPhrase = "OK",
                Details = "Count: 0"
            };
        }
    }

    public static class OrionStatusDtoExtensions
    {
        public static bool IsValid(this OrionStatusDto orionStatus)
        {
            // Orion can return multiple valid status codes, e. g. 200 or 204.
            return orionStatus.Code.StartsWith("2");
        }

        public static void ThrowIfNotValid(this OrionStatusDto orionStatus)
        {
            if (!orionStatus.IsValid())
            {
                throw new OrionException(orionStatus);
            }
        }

        public static string Serialize(this OrionStatusDto orionStatus)
        {
            return $"OrionStatus(code: '{orionStatus.Code}', reasonPhrase: '{orionStatus.ReasonPhrase}', details: '{orionStatus.Details}')";
        }
    }
}