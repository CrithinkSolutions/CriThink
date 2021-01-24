using System.Collections.Generic;

namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class IndexViewModel
    {
        public IEnumerable<NewsSource> NewsSources { get; set; }
    }

    public class NewsSource
    {
        public string Uri { get; set; }

        public Classification Classification { get; set; }
    }
}
