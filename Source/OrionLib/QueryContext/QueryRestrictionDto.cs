namespace OrionLib.QueryContext
{
    public class QueryRestrictionDto
    {
        public QueryRestrictionScopeDto[] Scopes { get; private set; }

        public static QueryRestrictionDto WithOneRestrictionScope(QueryRestrictionScopeDto restrictionScope)
        {
            return new QueryRestrictionDto
            {
                Scopes = new[] { restrictionScope }
            };
        }
    }
}
