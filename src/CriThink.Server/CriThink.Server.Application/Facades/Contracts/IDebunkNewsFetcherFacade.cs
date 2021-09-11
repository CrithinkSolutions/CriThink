using System;
using System.Threading.Tasks;
using CriThink.Server.Providers.DebunkingNewsFetcher;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Application.Facades
{
    public interface IDebunkNewsFetcherFacade
    {
        Task<DebunkingNewsProviderResult[]> FetchDebunkingNewsAsync(DateTime lastFetchingTimeStamp);
    }
}
