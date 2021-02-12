using System.Collections.Generic;

#pragma warning disable CA1056 // URI-like properties should not be strings
namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class IndexViewModel
    {
        public IEnumerable<NewsSource> NewsSources { get; set; }

        public bool HasNextPage { get; set; }
    }

    public class NewsSource
    {
        public string Uri { get; set; }

        public Classification Classification { get; set; }
    }
}
