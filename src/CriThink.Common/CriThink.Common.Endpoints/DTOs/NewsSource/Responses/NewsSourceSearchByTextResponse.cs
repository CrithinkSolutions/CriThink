using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceSearchByTextResponse : BaseNewsSourceSearch
    {
        [JsonPropertyName("favIcon")]
        public string FavIcon { get; set; }

        // Used in NewsSourceSearchCommunityViewHolder
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
