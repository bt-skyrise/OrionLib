using OrionLib.Communication;

namespace OrionLib.QueryContext
{
    public class QueryContextRequestFactory
    {
        private readonly OrionRequestFactory _orionRequestFactory;

        public QueryContextRequestFactory(
            OrionRequestFactory orionRequestFactory)
        {
            _orionRequestFactory = orionRequestFactory;
        }

        public QueryContextRequest Create()
        {
            return new QueryContextRequest(_orionRequestFactory);
        }
    }
}
