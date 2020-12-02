using System.Threading.Tasks;
using System.Collections.Generic;
using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Server.Web.Facades
{
    public interface INewsSourceServiceFacade
    {
        Task<IList<NewsSourceGetAllResponse>> GetAllNewsSourcesAsync();
    }
}