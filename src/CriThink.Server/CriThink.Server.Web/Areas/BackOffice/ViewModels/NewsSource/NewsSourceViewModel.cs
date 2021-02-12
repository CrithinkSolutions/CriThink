#pragma warning disable CA1056 // URI-like properties should not be strings
namespace CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource
{
    public class NewsSourceViewModel
    {
        public string Uri { get; set; }
        public Classification Classification { get; set; }
        public string Description { get; set; }
    }
}
