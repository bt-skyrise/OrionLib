namespace OrionLib.UpdateAttribute
{
    public class UpdateAttributeRequestDto
    {
        public string Value { get; private set; }

        public UpdateAttributeRequestDto(string value)
        {
            Value = value;
        }
    }
}