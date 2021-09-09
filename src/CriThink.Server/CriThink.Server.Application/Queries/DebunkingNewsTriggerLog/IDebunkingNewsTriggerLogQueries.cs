using System.Threading.Tasks;
using CriThink.Server.Application.Administration.ViewModels;

namespace CriThink.Server.Application.Queries
{
    public interface IDebunkingNewsTriggerLogQueries
    {
        /// <summary>
        /// Get all trigger logs with pagination support
        /// </summary>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <returns></returns>
        Task<TriggerLogsGetAllViewModel> GetAllTriggerLogsAsync(int pageSize, int pageIndex);
    }
}
