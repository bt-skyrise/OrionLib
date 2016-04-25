namespace OrionLib.CreateOrUpdateEntity
{
    public class CreateOrUpdateAttributeMetadataDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public CreateOrUpdateAttributeMetadataDto(OrionAttributeMetadata orionAttributeMetadata)
        {
            Name = orionAttributeMetadata.Name;
            Type = orionAttributeMetadata.Type;
            Value = orionAttributeMetadata.Value;
        }
    }
}