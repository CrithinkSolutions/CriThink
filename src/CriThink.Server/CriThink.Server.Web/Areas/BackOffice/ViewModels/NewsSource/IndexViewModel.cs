using System.Collections.Generic;
using System.Linq;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class IndexViewModel
    {
        public IndexViewModel(
            IEnumerable<NewsSourceViewModel> newsSources,
            bool hasNextPage)
        {
            NewsSources = newsSources.ToList();
            HasNextPage = hasNextPage;
        }

        public ICollection<NewsSourceViewModel> NewsSources { get; }

        public bool HasNextPage { get; }
    }
}
