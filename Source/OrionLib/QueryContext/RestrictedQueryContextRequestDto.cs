namespace OrionLib.QueryContext
{
    public class RestrictedQueryContextRequestDto : QueryContextRequestDto
    {
        public QueryRestrictionDto Restriction { get; private set; }

        public RestrictedQueryContextRequestDto(string type, QueryRestrictionScopeDto restrictionScope) : base(type)
        {
            Restriction = QueryRestrictionDto.WithOneRestrictionScope(restrictionScope);
        }
    }
}
