using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrionLib.QueryContext
{
    public static class QueryingAllEntities
    {
        private class OrionEntityPager
        {
            private readonly OrionQuery _orionQuery;

            private OrionQueryResult _result;

            public OrionEntityPager(OrionQuery orionQuery)
            {
                _orionQuery = orionQuery;
            }

            public bool ShouldGetNextPage => IsFirstPage || _result.AnyLeft;

            public async Task<IEnumerable<OrionEntity>> GetNextPageAsync()
            {
                _result =  await _orionQuery.GetPageAsync(CurrentOffset, OrionQuery.MaxLimit);

                return _result.Entities;
            }

            private int CurrentOffset => IsFirstPage ? 0 : _result.NextPageOffset;

            private bool IsFirstPage => _result == null;
        }

        public static async Task<IEnumerable<OrionEntity>> GetAllAsync(this OrionQuery orionQuery)
        {
            var pager = new OrionEntityPager(orionQuery);
            var  entities = new List<OrionEntity>();

            while (pager.ShouldGetNextPage)
            {
                var pageEntities = await pager.GetNextPageAsync();
                entities.AddRange(pageEntities);
            }

            return entities;
        }
    }
}