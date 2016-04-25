namespace OrionLib.QueryContext.QueryAttributes
{
    public interface IOrionQueryRestriction
    {
        string RestrictionType { get; }
        string RestrictionValue { get; }
    }
}
