using System.Threading.Tasks;

namespace OrionLib.GetEntity
{
    public class TryGetEntityRequest
    {
        private readonly GetEntityRequest _getEntityRequest;

        public TryGetEntityRequest(GetEntityRequest getEntityRequest)
        {
            _getEntityRequest = getEntityRequest;
        }

        public async Task<OrionEntity> ExecuteAsync(string type, string id)
        {
            try
            {
                return await _getEntityRequest.ExecuteAsync(type, id);
            }
            catch (OrionException e)
            {
                if (e.Code == "404")
                {
                    return null;
                }

                throw;
            }
        }
    }
}