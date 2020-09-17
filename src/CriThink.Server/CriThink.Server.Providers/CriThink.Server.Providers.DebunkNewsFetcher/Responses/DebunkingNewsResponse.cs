// ReSharper disable once CheckNamespace
namespace CriThink.Server.Providers.DebunkNewsFetcher
{
    public class DebunkingNewsResponse
    {
        public DebunkingNewsResponse(string title, string link)
        {
            Title = title;
            Link = link;
        }

        public string Title { get; }

        public string Link { get; }
    }
}
