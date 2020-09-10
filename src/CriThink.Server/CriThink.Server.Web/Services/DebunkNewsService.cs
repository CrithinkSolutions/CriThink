using System;
using System.Threading.Tasks;
using CriThink.Server.Web.Facades;

namespace CriThink.Server.Web.Services
{
    public class DebunkNewsService : IDebunkNewsService
    {
        private readonly IDebunkNewsFetcherFacade _debunkNewsFetcherFacade;

        public DebunkNewsService(IDebunkNewsFetcherFacade debunkNewsFetcherFacade)
        {
            _debunkNewsFetcherFacade = debunkNewsFetcherFacade ?? throw new ArgumentNullException(nameof(debunkNewsFetcherFacade));
        }

        public async Task UpdateRepositoryAsync()
        {
            await _debunkNewsFetcherFacade.FetchOpenOnlineDebunkNewsAsync().ConfigureAwait(false);
        }
    }

    public interface IDebunkNewsService
    {
        /// <summary>
        /// Update the debunk news repository
        /// </summary>
        /// <returns></returns>
        Task UpdateRepositoryAsync();
    }
}
