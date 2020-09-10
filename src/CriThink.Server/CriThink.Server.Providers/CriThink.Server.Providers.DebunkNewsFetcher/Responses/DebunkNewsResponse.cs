// ReSharper disable once CheckNamespace
namespace CriThink.Server.Providers.DebunkNewsFetcher
{
    public class DebunkNewsResponse
    {
        public DebunkNewsResponse(string title, string link)
        {
            Title = title;
            Link = link;
        }

        public string Title { get; }

        public string Link { get; }
    }
}
