using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionLib.CommonContracts
{
    public class GetMetadataDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public static class GetMetadataDtoExtensions
    {
        public static OrionAttributeMetadata ToEntityAttributeMetadata(this GetMetadataDto getMetadata)
        {
            return new OrionAttributeMetadata(
                getMetadata.Name,
                getMetadata.Type,
                getMetadata.Value);
        }
    }
}
