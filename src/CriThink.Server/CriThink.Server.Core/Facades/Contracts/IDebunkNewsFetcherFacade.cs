using System.Threading.Tasks;
using CriThink.Server.Providers.DebunkNewsFetcher;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Core.Facades
{
    public interface IDebunkNewsFetcherFacade
    {
        Task<DebunkingNewsProviderResult[]> FetchDebunkingNewsAsync();
    }
}
