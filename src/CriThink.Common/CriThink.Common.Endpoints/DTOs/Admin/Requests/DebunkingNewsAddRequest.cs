using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class DebunkingNewsAddRequest
    {
        [Required]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [Required]
        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [Required]
        [Url]
        [JsonPropertyName("uri")]
        public string Link { get; set; }

        [Required]
        [Url]
        [JsonPropertyName("imageLink")]
        public string ImageLink { get; set; }

        [Required]
        [JsonPropertyName("publisherId")]
        public Guid PublisherId { get; set; }

        [Required]
        [JsonPropertyName("keywords")]
        public IReadOnlyCollection<string> Keywords { get; set; }
    }
}
