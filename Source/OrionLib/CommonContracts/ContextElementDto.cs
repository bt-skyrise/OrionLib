using System.Collections.Generic;
using System.Linq;

namespace OrionLib.CommonContracts
{
    public class ContextElementDto
    {
        public string Type { get; set; }
        public string IsPattern { get; set; }
        public string Id { get; set; }
        public List<GetAttributeDto> Attributes { get; set; }
    }

    public static class ContextElementDtoExtensions
    {
        public static OrionEntity ToEntity(this ContextElementDto contextElement)
        {
            var entityAttributes = contextElement.Attributes.Select(attribute => attribute.ToEntityAttribute());

            return new OrionEntity(contextElement.Type, contextElement.Id, entityAttributes);
        }
    }
}