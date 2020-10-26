using System.Text.Json.Serialization;

namespace CriThink.Client.Core.Models.DTOs
{
    public class BaseResponse<T>
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("result")]
        public T Result { get; set; }
    }
}
