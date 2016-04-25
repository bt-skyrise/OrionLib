using OrionLib.QueryContext.QueryAttributes;

namespace OrionLib.QueryContext
{
    public class OrionQueryFactory
    {
        private readonly QueryContextRequestFactory _queryContextRequestFactory;

        public OrionQueryFactory(
            QueryContextRequestFactory queryContextRequestFactory)
        {
            _queryContextRequestFactory = queryContextRequestFactory;
        }

        public OrionQuery CreateForSpecificType(string entityType)
        {
            var queryContextRequest = _queryContextRequestFactory.Create();
            var orionQueryRestrictionsCollection = new OrionQueryRestrictionsCollection(queryContextRequest);
            return new OrionQuery(orionQueryRestrictionsCollection, entityType);
        }
        public OrionQuery CreateForAllTypes()
        {
            var queryContextRequest = _queryContextRequestFactory.Create();
            var orionQueryRestrictionsCollection = new OrionQueryRestrictionsCollection(queryContextRequest);
            return new OrionQuery(orionQueryRestrictionsCollection, "");
        }
    }
}
