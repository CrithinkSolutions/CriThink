using System.Collections.Generic;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Common.Endpoints.DTOs.Search
{
    public class SearchByTextResponse
    {
        public SearchByTextResponse(
            IEnumerable<NewsSourceRelatedDebunkingNewsResponse> debunkingNews,
            IEnumerable<NewsSourceSearchByTextResponse> newsSources)
        {
            DebunkingNews = debunkingNews;
            NewsSources = newsSources;
        }

        [JsonPropertyName("debunkingNews")]
        public IEnumerable<NewsSourceRelatedDebunkingNewsResponse> DebunkingNews { get; }

        [JsonPropertyName("newsSources")]
        public IEnumerable<NewsSourceSearchByTextResponse> NewsSources { get; }
    }
}
