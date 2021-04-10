using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Data.Settings;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using MvvmCross.Logging;

namespace CriThink.Client.Core.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private const string UserId = "user_id";
        private const string UserUsername = "user_username";
        private const string UserEmail = "user_email";
        private const string UserPassword = "user_password";
        private const string UserToken = "user_token";
        private const string UserTokenExpiration = "user_token_expiration";
        private const string UserLoginProvider = "user_provider";
        private const string UserAvatar = "user_avatar";

        private readonly ISettingsRepository _settingsRepository;
        private readonly IMvxLog _log;

        public IdentityRepository(ISettingsRepository settingsRepository, IMvxLogProvider logProvider)
        {
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _log = logProvider?.GetLogFor<IdentityRepository>();
        }

        public async Task<User> GetUserInfoAsync()
        {
            try
            {
                var userIdTask = GetUserInSettingsSettingAsync(UserId, null);
                var usernameTask = GetUserInSettingsSettingAsync(UserUsername, null);
                var userEmailTask = GetUserInSettingsSettingAsync(UserEmail, null);
                var userPasswordTask = GetUserInSettingsSettingAsync(UserPassword, null);
                var userTokenTask = GetUserInSettingsSettingAsync(UserToken, null);
                var userTokenExpirationTask = GetUserInSettingsSettingAsync(UserTokenExpiration, null);
                var userProviderTask = GetUserInSettingsSettingAsync(UserLoginProvider, null);
                var userAvatarTask = GetUserInSettingsSettingAsync(UserAvatar, null);

                await Task.WhenAll(
                        userIdTask,
                        usernameTask,
                        userEmailTask,
                        userPasswordTask,
                        userTokenTask,
                        userTokenExpirationTask,
                        userProviderTask,
                        userAvatarTask)
                    .ConfigureAwait(false);

                var userId = userIdTask.Result;
                var email = userEmailTask.Result;
                var username = usernameTask.Result;
                var userPassword = userPasswordTask.Result;
                var userToken = userTokenTask.Result;
                var userTokenExpiration = userTokenExpirationTask.Result;
                var userProvider = userProviderTask.Result;
                var userAvatar = userAvatarTask.Result;

                if (userId is null ||
                    email is null ||
                    username is null ||
                    userPassword is null ||
                    userToken is null ||
                    userTokenExpiration is null ||
                    userProvider is null)
                    return null;

                ExternalLoginProvider loginProvider = ExternalLoginProvider.None;
                var isValid = Enum.TryParse(typeof(ExternalLoginProvider), userProvider, true, out var provider);
                if (isValid)
                    loginProvider = (ExternalLoginProvider) provider;
                else
                    _log?.Fatal("Local login provider is not valid. Forced to 'None'");

                return new User(userId, email, username, userPassword, userAvatar, new JwtTokenResponse
                {
                    Token = userToken,
                    ExpirationDate = DateTimeExtensions.DeserializeDateTime(userTokenExpiration),
                }, loginProvider);
            }
            catch (Exception ex)
            {
                _log?.FatalException("Error getting user info", ex);
                throw;
            }
        }

        public async Task<string> GetUserTokenAsync()
        {
            try
            {
                var userToken = await GetUserInSettingsSettingAsync(UserToken).ConfigureAwait(false);
                return userToken;
            }
            catch (Exception ex)
            {
                _log?.FatalException("Error getting user token", ex);
                throw;
            }
        }

        public Task SetUserInfoAsync(string userId, string userEmail, string username, string password, string jwtToken,
            DateTime tokenExpiration, string loginResponseAvatarPath, ExternalLoginProvider loginProvider)
        {
            try
            {
                return Task.WhenAll(
                    UpdateUserInSettingsAsync(UserId, userId),
                    UpdateUserInSettingsAsync(UserUsername, username),
                    UpdateUserInSettingsAsync(UserEmail, userEmail),
                    UpdateUserInSettingsAsync(UserPassword, password),
                    UpdateUserInSettingsAsync(UserToken, jwtToken),
                    UpdateUserInSettingsAsync(UserTokenExpiration, DateTimeExtensions.SerializeDateTime(tokenExpiration)),
                    UpdateUserInSettingsAsync(UserLoginProvider, loginProvider.ToString()),
                    UpdateUserInSettingsAsync(UserAvatar, loginResponseAvatarPath)
                );
            }
            catch (Exception ex)
            {
                _log.ErrorException("Error saving user info", ex, userId);
                throw;
            }
        }

        public void EraseUserInfo()
        {
            try
            {
                EraseUserSettingsAsync(UserId);
                EraseUserSettingsAsync(UserUsername);
                EraseUserSettingsAsync(UserEmail);
                EraseUserSettingsAsync(UserPassword);
                EraseUserSettingsAsync(UserToken);
                EraseUserSettingsAsync(UserTokenExpiration);
                EraseUserSettingsAsync(UserLoginProvider);
                EraseUserSettingsAsync(UserAvatar);
            }
            catch (Exception ex)
            {
                _log?.ErrorException("Error deleting user info", ex);
                throw;
            }
        }

        private Task UpdateUserInSettingsAsync(string key, string value) => _settingsRepository.SavePreferenceAsync(key, value, true);

        private Task<string> GetUserInSettingsSettingAsync(string key, string defaultValue = "") => _settingsRepository.GetPreferenceAsync(key, defaultValue, true);

        private void EraseUserSettingsAsync(string key) => _settingsRepository.RemovePreference(key, true);
    }

    public interface IIdentityRepository
    {
        /// <summary>
        /// Read user info stored in local storage
        /// </summary>
        /// <returns></returns>
        Task<User> GetUserInfoAsync();

        /// <summary>
        /// Get logged user token
        /// </summary>
        /// <returns>User token</returns>
        Task<string> GetUserTokenAsync();

        /// <summary>
        /// Save user info to secure storage asynchronous
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="userEmail">User email</param>
        /// <param name="username">Username</param>
        /// <param name="password"></param>
        /// <param name="jwtToken">User jwt token for authentication</param>
        /// <param name="tokenExpiration">User jwt token expiration</param>
        /// <param name="loginResponseAvatarPath">User avatar path</param>
        /// <param name="loginProvider">Login provider used by the user</param>
        /// <returns></returns>
        Task SetUserInfoAsync(string userId, string userEmail, string username, string password, string jwtToken,
            DateTime tokenExpiration, string loginResponseAvatarPath, ExternalLoginProvider loginProvider);

        /// <summary>
        /// Delete user info
        /// </summary>
        void EraseUserInfo();
    }
}
