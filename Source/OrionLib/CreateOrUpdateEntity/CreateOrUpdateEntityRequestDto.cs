using System.Linq;

namespace OrionLib.CreateOrUpdateEntity
{
    public class CreateOrUpdateEntityRequestDto
    {
        public CreateOrUpdateAttributeDto[] Attributes { get; private set; }

        public CreateOrUpdateEntityRequestDto(OrionEntity orionEntity)
        {
            Attributes = orionEntity
                .Attributes
                .Select(orionAttribute => new CreateOrUpdateAttributeDto(orionAttribute))
                .ToArray();
        }
    }
}