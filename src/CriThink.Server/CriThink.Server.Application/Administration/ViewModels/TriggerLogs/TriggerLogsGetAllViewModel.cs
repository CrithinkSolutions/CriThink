using System.Collections.Generic;

namespace CriThink.Server.Application.Administration.ViewModels
{
    public class TriggerLogsGetAllViewModel
    {
        public TriggerLogsGetAllViewModel() { }

        public TriggerLogsGetAllViewModel(IEnumerable<TriggerLogGetViewModel> logsCollection, bool hasNextPage)
        {
            LogsCollection = new List<TriggerLogGetViewModel>(logsCollection).AsReadOnly();
            HasNextPage = hasNextPage;
        }

        public IReadOnlyCollection<TriggerLogGetViewModel> LogsCollection { get; set; }

        public bool HasNextPage { get; set; }
    }
}
