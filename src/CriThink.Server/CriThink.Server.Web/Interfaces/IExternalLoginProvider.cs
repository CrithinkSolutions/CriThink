using System.Threading.Tasks;
using CriThink.Server.Web.Models;

namespace CriThink.Server.Web.Interfaces
{
    public interface IExternalLoginProvider
    {
        Task<ExternalProviderUserInfo> GetUserAccessInfo(string userToken);
    }
}