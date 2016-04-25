using System.Threading.Tasks;
using OrionLib.Communication;

namespace OrionLib.QueryContext
{
    public class QueryContextRequest
    {
        private readonly OrionRequestFactory _orionRequestFactory;
        private int _offset = 0;
        private int _limit = 1000;
        private bool _countDetails = false;
        private QueryRestrictionScopeDto _restrictionScope = null;

        public QueryContextRequest(OrionRequestFactory orionRequestFactory)
        {
            _orionRequestFactory = orionRequestFactory;
        }

        public QueryContextRequest WithPagination(int offset, int limit, bool countDetails = false)
        {
            _offset = offset;
            _limit = limit;
            _countDetails = countDetails;

            return this;
        }

        public QueryContextRequest WithRestrictionScope(QueryRestrictionScopeDto restrictionScope)
        {
            _restrictionScope = restrictionScope;
            return this;
        }

        public async Task<OrionEntity[]> ExecuteAsync(string type)
        {
            try
            {
                var queryResponse = await QueryContextAsync(type);
                return queryResponse.GetEntities();
            }
            catch (OrionException e)
            {
                if (e.Code == "404")
                {
                    return new OrionEntity[0];
                }

                throw;
            }
        }

        public async Task<QueryContextResponseDto> QueryContextAsync(string type)
        {
            var requestBody = ShouldPerformWithRestrictions() ? 
                new RestrictedQueryContextRequestDto(type, _restrictionScope) :
                new QueryContextRequestDto(type);

            //todo get resource from dedicated class
            using (var request = _orionRequestFactory.Build(OrionRequest.Method.POST, "v1/queryContext"))
            {
                return await request
                    .AddBody(requestBody)
                    .WithPageOffset(_offset)
                    .WithPageLimit(_limit)
                    .WithCountDetails(_countDetails)
                    .ExecuteAsync<QueryContextResponseDto>();
            }
        }

        private bool ShouldPerformWithRestrictions()
        {
            return _restrictionScope != null;
        }
    }
}