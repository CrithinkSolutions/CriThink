using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
#pragma warning disable CA1056 // Uri properties should not be strings

namespace CriThink.Common.Endpoints.DTOs.NewsAnalyzer
{
    public class NewsAnalyzerResponse
    {
        [JsonPropertyName("analysisType")]
        public NewsAnalysisTypeResponse NewsAnalysisType { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("hasError")]
        public bool HasError { get; set; }

        [JsonPropertyName("analysisScore")]
        public int AnalysisScore { get; set; }

        [JsonPropertyName("errorDescription")]
        public string ErrorDescription { get; set; }
    }
}
