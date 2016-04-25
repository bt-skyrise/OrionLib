namespace OrionLib.Communication
{
    public class OrionRequestFactory
    {
        private readonly OrionConfiguration _orionConfiguration;

        public OrionRequestFactory(OrionConfiguration orionConfiguration)
        {
            _orionConfiguration = orionConfiguration;
        }

        public OrionRequest Build(OrionRequest.Method method, string resource)
        {
            return new OrionRequest(_orionConfiguration, method, resource);
        }
    }
}