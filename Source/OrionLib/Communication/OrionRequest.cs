using System;
using System.CodeDom;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OrionLib.CommonContracts;

namespace OrionLib.Communication
{
    public class OrionRequest : IDisposable
    {
        private class QueryParameters
        {
            public string Resource { get; private set; }
            private bool _firstParameter = true;

            public QueryParameters(string resource)
            {
                Resource = resource;
            }

            public void Add(string name, string value)
            {
                if (_firstParameter)
                {
                    Resource += $"?{name}={value}";
                    _firstParameter = false;
                }
                else
                {
                    Resource += $"&{name}={value}";
                }
                
            }
        }

        public enum Method
        {
            GET, POST, DELETE
        }

        private readonly Method _method;
        private readonly HttpClient _client;
        private readonly QueryParameters _query;
        private readonly JsonNetSerializer _serializer = new JsonNetSerializer();

        private object _body;

        public OrionRequest(OrionConfiguration orionConfiguration, Method method, string resource)
        {
            _method = method;
            _query = new QueryParameters(resource);

            _client = new HttpClient
            {
                BaseAddress = new Uri(orionConfiguration.OrionUrl.Value)
            };

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Fiware-ServicePath", orionConfiguration.ServicePath);
            _client.DefaultRequestHeaders.Add("Fiware-Service", orionConfiguration.Service);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            SetPepProxyAuthToken(orionConfiguration);
        }

        private void SetPepProxyAuthToken(OrionConfiguration orionConfiguration)
        {
            if (orionConfiguration.ComponentAuthToken != null)
            {
                _client.DefaultRequestHeaders.Add("X-Auth-Token", orionConfiguration.ComponentAuthToken);
            }
        }


        public OrionRequest AddBody(object body)
        {
            _body = body;
            return this;
        }

        public OrionRequest WithPageLimit(int pageLimit)
        {
            _query.Add("limit", pageLimit.ToString());
            return this;
        }

        public OrionRequest WithPageOffset(int pageOffset)
        {
            _query.Add("offset", pageOffset.ToString());
            return this;
        }

        public OrionRequest WithCountDetails(bool countDetails)
        {
            if (countDetails)
            {
                _query.Add("details", "on");
            }

            return this;
        }

        public async Task ExecuteAsync()
        {
            await ExecuteAsync<OrionResponseDto>();
        }


        public async Task<TResponse> ExecuteAsync<TResponse>()
            where TResponse : OrionResponseDto, new()
        {
            HttpResponseMessage response = null;

            if (_method == Method.GET)
            {
                 response = await _client.GetAsync(_query.Resource);
            }

            if (_method == Method.POST)
            {
                response = await _client.PostAsync(_query.Resource, 
                    new StringContent(_serializer.Serialize(_body), Encoding.UTF8, "application/json"));
            }

            if (_method == Method.DELETE)
            {
                response = await _client.DeleteAsync(_query.Resource);
            }

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            TResponse result = _serializer.Deserialize<TResponse>(stringContent);

            HandleOrionErrors(result);

            return result;
        }

        private static void HandleOrionErrors<TResponse>(TResponse response) where TResponse : OrionResponseDto, new()
        {
            var orionError = response.TryGetError();

            if (orionError != null)
            {
                throw new OrionException(orionError);
            }
        }

        public void Dispose()
        {
           _client.Dispose();
        }
    }
}