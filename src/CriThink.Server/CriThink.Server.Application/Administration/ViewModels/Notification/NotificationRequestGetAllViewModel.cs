using System.Collections.Generic;

namespace CriThink.Server.Application.Administration.ViewModels
{
    public class NotificationRequestGetAllViewModel
    {
        public NotificationRequestGetAllViewModel() { }

        public NotificationRequestGetAllViewModel(IEnumerable<NotificationRequestGetViewModel> notificationCollection, bool hasNextPage)
        {
            NotificationCollection = new List<NotificationRequestGetViewModel>(notificationCollection).AsReadOnly();
            HasNextPage = hasNextPage;
        }

        public IReadOnlyCollection<NotificationRequestGetViewModel> NotificationCollection { get; set; }

        public bool HasNextPage { get; set; }
    }
}
