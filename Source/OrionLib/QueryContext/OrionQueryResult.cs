using System.Collections.Generic;

namespace OrionLib.QueryContext
{
    public class OrionQueryResult
    {
        public IEnumerable<OrionEntity> Entities { get; }

        public int QueryOffset { get; }
        public int QueryLimit { get; }
        public int AllEntitiesCount { get; }

        public bool AnyLeft => NextPageOffset < AllEntitiesCount;
        public int NextPageOffset => QueryOffset + QueryLimit;

        public OrionQueryResult(
            IEnumerable<OrionEntity> entities,
            int queryOffset,
            int queryLimit,
            int entitiesCount)
        {
            Entities = new List<OrionEntity>(entities);
            QueryOffset = queryOffset;
            QueryLimit = queryLimit;
            AllEntitiesCount = entitiesCount;
        }
    }
}
