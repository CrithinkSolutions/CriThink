using System.Threading.Tasks;
using CriThink.Client.Core.Models.Identity;

namespace CriThink.Client.Core.Repositories
{
    public interface IIdentityRepository
    {
        /// <summary>
        /// Read user access info stored in local storage
        /// </summary>
        /// <returns></returns>
        Task<UserAccess> GetUserAccessAsync();

        /// <summary>
        /// Save user access info locally
        /// </summary>
        /// <param name="userAccess">User access to save</param>
        /// <returns></returns>
        Task SetUserAccessAsync(UserAccess userAccess);

        /// <summary>
        /// Delete user info
        /// </summary>
        void EraseUserInfo();
    }
}