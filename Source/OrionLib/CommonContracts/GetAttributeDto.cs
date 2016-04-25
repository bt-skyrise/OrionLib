using System.Collections.Generic;
using System.Linq;

namespace OrionLib.CommonContracts
{
    public class GetAttributeDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public List<GetMetadataDto> Metadatas { get; set; }
    }

    public static class GetAttributeDtoExtensions
    {
        public static OrionAttribute ToEntityAttribute(this GetAttributeDto getAttribute)
        {
            return new OrionAttribute(
                getAttribute.Name, 
                getAttribute.Type, 
                getAttribute.Value, 
                getAttribute.Metadatas?.Select(m=> m.ToEntityAttributeMetadata()));
        }
    }
}