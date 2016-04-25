using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrionLib.CommonContracts;
using OrionLib.Communication;
using OrionLib.QueryContext;
using OrionLib.UpdateAttribute;

namespace OrionLib.RemoveAttribute
{
    public class RemoveAttributesRequest
    {
        private readonly OrionRequestFactory _orionRequestFactory;

        public RemoveAttributesRequest(OrionRequestFactory orionRequestFactory)
        {
            _orionRequestFactory = orionRequestFactory;
        }

        public async Task ExecuteAsync(string type, string id, IEnumerable<string> attributeNames)
        {
            var requestBody = ContextRequestDto.RemoveAttributesRequest(type, id, attributeNames);

            var resource = OrionResources.GetUpdateContextResource();

            using (var request = _orionRequestFactory.Build(OrionRequest.Method.POST, resource))
            {
                var results = await request
                    .AddBody(requestBody)
                    .ExecuteAsync<QueryContextResponseDto>();

                CheckReponseForErrors(results);
            }
        }

        private static void CheckReponseForErrors(QueryContextResponseDto results)
        {
            foreach (var orionStatusDto in results.ContextResponses.Select(contextElement => contextElement.StatusCode))
            {
                orionStatusDto.ThrowIfNotValid();
            }
        }
    }
}
