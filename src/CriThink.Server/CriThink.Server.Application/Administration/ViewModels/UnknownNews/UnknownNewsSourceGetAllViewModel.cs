using System.Collections.Generic;

namespace CriThink.Server.Application.Administration.ViewModels
{
    public class UnknownNewsSourceGetAllViewModel
    {
        public UnknownNewsSourceGetAllViewModel() { }

        public UnknownNewsSourceGetAllViewModel(IEnumerable<UnknownNewsSourceGetViewModel> collection, bool hasNextPage)
        {
            UnknownNewsSourceCollection = new List<UnknownNewsSourceGetViewModel>(collection).AsReadOnly();
            HasNextPage = hasNextPage;
        }

        public IReadOnlyCollection<UnknownNewsSourceGetViewModel> UnknownNewsSourceCollection { get; set; }

        public bool HasNextPage { get; set; }
    }
}
