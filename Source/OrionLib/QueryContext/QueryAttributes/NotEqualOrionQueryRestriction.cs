namespace OrionLib.QueryContext.QueryAttributes
{
    public class NotEqualOrionQueryRestriction : StringOrionQueryRestriction
    {
        private readonly string _attributeName;
        private readonly string _value;

        public override string RestrictionValue => _attributeName + "!=" + _value;

        public NotEqualOrionQueryRestriction(string attribute, string value)
        {
            _attributeName = attribute;
            _value = value;
        }

        public NotEqualOrionQueryRestriction(string attribute, bool value)
        {
            _attributeName = attribute;
            _value = value.ToString();
        }
    }
}
