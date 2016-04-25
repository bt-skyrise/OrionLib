namespace OrionLib.QueryContext
{
    public class QueryRestrictionScopeDto
    {
        public string Type { get; private set; }
        public string Value { get; private set; }

        public QueryRestrictionScopeDto(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
