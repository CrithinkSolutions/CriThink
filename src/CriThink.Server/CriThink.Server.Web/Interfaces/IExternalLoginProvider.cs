using System.Threading.Tasks;
using CriThink.Server.Web.Models.DTOs;

namespace CriThink.Server.Web.Interfaces
{
    public interface IExternalLoginProvider
    {
        Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken);
    }
}