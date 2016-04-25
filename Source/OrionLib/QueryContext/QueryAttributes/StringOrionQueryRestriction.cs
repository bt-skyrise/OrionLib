namespace OrionLib.QueryContext.QueryAttributes
{
    public abstract class StringOrionQueryRestriction : IOrionQueryRestriction
    {
        public const string OrionQueryType = "FIWARE::StringQuery";

        public string RestrictionType => OrionQueryType;

        public abstract string RestrictionValue
        {
            get;
        }
    }
}
