using System;

namespace CriThink.Server.Application.Administration.ViewModels
{
    public class NotificationRequestGetViewModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string RequestedAt { get; set; }

        public string RequestedLink { get; set; }

        public int RequestCount { get; set; }
    }
}
