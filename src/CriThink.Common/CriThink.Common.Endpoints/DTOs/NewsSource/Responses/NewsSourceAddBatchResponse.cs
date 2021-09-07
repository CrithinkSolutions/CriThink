using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CriThink.Common.Endpoints.DTOs.NewsSource
{
    public class NewsSourceAddBatchResponse
    {
        public NewsSourceAddBatchResponse()
        {
            Errors = new Dictionary<string, string>();
        }

        [JsonPropertyName("errors")]
        public Dictionary<string, string> Errors { get; set; }
    }
}
