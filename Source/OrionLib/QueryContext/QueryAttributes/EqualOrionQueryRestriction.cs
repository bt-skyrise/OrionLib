namespace OrionLib.QueryContext.QueryAttributes
{
    public class EqualOrionQueryRestriction : StringOrionQueryRestriction
    {
        private readonly string _attributeName;
        private readonly string _value;

        public override string RestrictionValue => _attributeName + "==" + _value;

        public EqualOrionQueryRestriction(string attribute, string value)
        {
            _attributeName = attribute;
            _value = value;
        }

        public EqualOrionQueryRestriction(string attribute, bool value)
        {
            _attributeName = attribute;
            _value = value.ToString();
        }
    }
}
