using OrionLib.CommonContracts;

namespace OrionLib.QueryContext
{
    public class QueryContextRequestDto
    {
        public EntityQueryDto[] Entities { get; private set; }

        public QueryContextRequestDto(string type)
        {
            Entities = new[]
            {
                EntityQueryDto.QueryAllEntitiesOfType(type)
            };
        }
    }
}