using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceGetAllRequest
    {
        [Required]
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [Required]
        [JsonPropertyName("pageIndex")]
        public int PageIndex { get; set; }

        [JsonPropertyName("filter")]
        public NewsSourceGetAllFilterRequest Filter { get; set; }
    }
}
