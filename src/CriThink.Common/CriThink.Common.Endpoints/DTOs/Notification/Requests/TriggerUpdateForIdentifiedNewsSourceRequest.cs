using CriThink.Common.Endpoints.DTOs.NewsSource;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Notification
{
    public class TriggerUpdateForIdentifiedNewsSourceRequest
    {
        public string Domain { get; set; }

        public NewsSourceClassification Classification { get; set; }
    }
}
