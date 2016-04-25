using System.Collections.Generic;
using System.Linq;

namespace OrionLib.QueryContext.QueryAttributes
{
    public class OrionQueryRestrictionsCollection
    {
        private const string RestrictionsDelimiter = ";";
        private readonly List<IOrionQueryRestriction> _queryRestrictions = new List<IOrionQueryRestriction>();
        private readonly QueryContextRequest _queryContextRequest;

        public OrionQueryRestrictionsCollection(
            QueryContextRequest queryContextRequest)
        {
            _queryContextRequest = queryContextRequest;
        }

        public void Add(IOrionQueryRestriction orionQueryRestriction)
        {
            _queryRestrictions.Add(orionQueryRestriction);
        }

        public QueryContextRequest PrepareQueryContextRequest()
        {
            if (_queryRestrictions.Any())
            {
                return _queryContextRequest
                    .WithRestrictionScope(BuildUpRestrictionScope());
            }

            return _queryContextRequest;
        }

        private QueryRestrictionScopeDto BuildUpRestrictionScope()
        {
            return new QueryRestrictionScopeDto(
                   StringOrionQueryRestriction.OrionQueryType,
                   string.Join(RestrictionsDelimiter, _queryRestrictions.Select(restr=>restr.RestrictionValue)));
        }
    }
}
