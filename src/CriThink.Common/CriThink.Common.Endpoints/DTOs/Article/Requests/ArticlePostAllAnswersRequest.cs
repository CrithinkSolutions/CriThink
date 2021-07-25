using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.Article
{
    public class ArticlePostAllAnswersRequest
    {
        [JsonPropertyName("questions")]
        public IList<ArticlePostAnswersRequest> Questions { get; set; }

        [JsonPropertyName("newsLink")]
        public string NewsLink { get; set; }
    }
}
