namespace OrionLib.Subscribe.Notifying
{
    public class NotifyConditionDto
    {
        public string Type { get; private set; }
        public string[] CondValues { get; private set; }

        public static NotifyConditionDto OnAttributesChange(string[] observedAttributes)
        {
            return new NotifyConditionDto
            {
                Type = "ONCHANGE",
                CondValues = observedAttributes
            };
        }
    }
}