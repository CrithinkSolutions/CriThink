using System.Collections.Generic;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class NotificationRequestGetAllResponse
    {
        public NotificationRequestGetAllResponse() { }

        public NotificationRequestGetAllResponse(IEnumerable<NotificationRequestGetResponse> notificationCollection, bool hasNextPage)
        {
            NotificationCollection = new List<NotificationRequestGetResponse>(notificationCollection).AsReadOnly();
            HasNextPage = hasNextPage;
        }

        [JsonPropertyName("notificationCollection")]
        public IReadOnlyCollection<NotificationRequestGetResponse> NotificationCollection { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
    }
}
