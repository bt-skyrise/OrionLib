using OrionLib.CommonContracts;
using OrionLib.QueryContext.QueryAttributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrionLib.QueryContext
{
    public class OrionQuery
    {
        public const int MaxLimit = 1000;

        private readonly OrionQueryRestrictionsCollection _orionQueryRestrictionsCollection;
        private readonly string _entityType;

        public OrionQuery(
            OrionQueryRestrictionsCollection orionQueryRestrictionsCollection,
            string entityType)
        {
            _orionQueryRestrictionsCollection = orionQueryRestrictionsCollection;
            _entityType = entityType;
        }

        public async Task<OrionQueryResult> GetPageAsync(int offset, int limit)
        {
            if (limit <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Results limit must be greater than 0 but was {limit}.",
                    nameof(limit));
            }

            if (limit > MaxLimit)
            {
                throw new ArgumentOutOfRangeException(
                    $"Maximum results limit for Orion is {MaxLimit} but was {limit}.",
                    nameof(limit));
            }

            var queryResponse = await ExecuteQueryAsync(offset, limit);
            return Map(queryResponse, offset, limit);
        }

        private async Task<QueryContextResponseDto> ExecuteQueryAsync(int offset, int limit)
        {
            try
            {
                return await _orionQueryRestrictionsCollection
                    .PrepareQueryContextRequest()
                    .WithPagination(offset, limit, true)
                    .QueryContextAsync(_entityType);
            }
            catch (OrionException e)
            {
                if (e.Code == "404")
                {
                    return QueryContextResponseDto.Empty();
                }

                throw;
            }
        }

        private OrionQueryResult Map(QueryContextResponseDto queryResponseDto, int offset, int limit)
        {
            return new OrionQueryResult(
                queryResponseDto.GetEntities(),
                offset,
                limit,
                ExtractCountFromErrorDetails(queryResponseDto.ErrorCode));
        }

        private int ExtractCountFromErrorDetails(OrionStatusDto orionError)
        {
            //expected details format is "Count: {value}"
            return int.Parse(orionError.Details.Split(':').Select(x => x.Trim()).ElementAt(1));
        }

        public async Task<int> CountAsync()
        {
            var queryResult = await GetPageAsync(0, 1);
            return queryResult.AllEntitiesCount;
        }

        public OrionQuery WithAttributeEqualTo(string attribute, bool value)
        {
            if (string.IsNullOrEmpty(attribute)) throw new ArgumentNullException(nameof(attribute));

            _orionQueryRestrictionsCollection.Add(new EqualOrionQueryRestriction(attribute, value));
            return this;
        }

        public OrionQuery WithAttributeEqualTo(string attribute, string value)
        {
            if (string.IsNullOrEmpty(attribute)) throw new ArgumentNullException(nameof(attribute));

            _orionQueryRestrictionsCollection.Add(new EqualOrionQueryRestriction(attribute, value));
            return this;
        }

        public OrionQuery WithAttributeNotEqualTo(string attribute, bool value)
        {
            if (string.IsNullOrEmpty(attribute)) throw new ArgumentNullException(nameof(attribute));

            _orionQueryRestrictionsCollection.Add(new NotEqualOrionQueryRestriction(attribute, value));
            return this;
        }

        public OrionQuery WithAttributeNotEqualTo(string attribute, string value)
        {
            if (string.IsNullOrEmpty(attribute)) throw new ArgumentNullException(nameof(attribute));

            _orionQueryRestrictionsCollection.Add(new NotEqualOrionQueryRestriction(attribute, value));
            return this;
        }
    }
}
