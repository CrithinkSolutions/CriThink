﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
#pragma warning disable CA1056 // Uri properties should not be strings

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceRemoveRequest
    {
        [Url]
        [Required]
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
