using System;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class UnknownNewsSourceNotificationRequestsViewModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string RequestedAt { get; set; }

        public string RequestedLink { get; set; }

        public int RequestCount { get; set; }
    }
}