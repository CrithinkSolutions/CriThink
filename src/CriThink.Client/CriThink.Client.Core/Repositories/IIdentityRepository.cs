using System.Threading.Tasks;
using CriThink.Client.Core.Models.Identity;

namespace CriThink.Client.Core.Repositories
{
    public interface IIdentityRepository
    {
        /// <summary>
        /// Read user info stored in local storage
        /// </summary>
        /// <returns></returns>
        Task<User> GetUserInfoAsync();

        /// <summary>
        /// Save user info locally
        /// </summary>
        /// <param name="user"><see cref="User"/> to save</param>
        /// <returns></returns>
        Task SetUserInfoAsync(User user);

        /// <summary>
        /// Delete user info
        /// </summary>
        void EraseUserInfo();
    }
}