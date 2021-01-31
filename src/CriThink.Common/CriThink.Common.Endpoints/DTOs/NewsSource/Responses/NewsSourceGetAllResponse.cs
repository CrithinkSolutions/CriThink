using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace
#pragma warning disable CA1056 // Uri properties should not be strings
namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceGetAllResponse
    {
        public NewsSourceGetAllResponse() { }

        public NewsSourceGetAllResponse(IEnumerable<NewsSourceGetResponse> newsSourceCollection, bool hasNextPage)
        {
            NewsSourcesCollection = new List<NewsSourceGetResponse>(newsSourceCollection).AsReadOnly();
            HasNextPage = hasNextPage;
        }

        [JsonPropertyName("newsSources")]
        public IReadOnlyCollection<NewsSourceGetResponse> NewsSourcesCollection { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
    }
}
