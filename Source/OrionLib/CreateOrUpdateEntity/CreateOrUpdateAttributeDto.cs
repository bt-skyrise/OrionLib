using System.Collections.Generic;
using System.Linq;

namespace OrionLib.CreateOrUpdateEntity
{
    public class CreateOrUpdateAttributeDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public CreateOrUpdateAttributeMetadataDto[] Metadatas { get; set; } 

        public CreateOrUpdateAttributeDto(OrionAttribute orionAttribute)
        {
            Name = orionAttribute.Name;
            Type = orionAttribute.Type;
            Value = orionAttribute.Value;

            Metadatas = orionAttribute
                .Metadatas
                .Select(m => new CreateOrUpdateAttributeMetadataDto(m))
                .ToArray();
        }
    }
}