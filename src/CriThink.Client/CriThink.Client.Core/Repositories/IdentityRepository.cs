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

        private readonly ISettingsRepository _settingsRepository;
        private readonly IMvxLog _logger;

        public IdentityRepository(ISettingsRepository settingsRepository, IMvxLog logger)
        {
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _logger = logger;
        }

        public async Task<User> GetUserInfoAsync()
        {
            try
            {
                var userIdTask = GetUserInSettingsSettingAsync(UserId);
                var userUsernameTask = GetUserInSettingsSettingAsync(UserUsername);
                var userEmailTask = GetUserInSettingsSettingAsync(UserEmail);
                var userPasswordToken = GetUserInSettingsSettingAsync(UserPassword);
                var userTokenTask = GetUserInSettingsSettingAsync(UserToken);
                var userTokenExpirationTask = GetUserInSettingsSettingAsync(UserTokenExpiration);

                await Task.WhenAll(userIdTask, userUsernameTask, userEmailTask, userPasswordToken, userTokenTask, userTokenExpirationTask).ConfigureAwait(false);

                return new User(userIdTask.Result, userEmailTask.Result, userUsernameTask.Result, userPasswordToken.Result, new JwtTokenResponse
                {
                    Token = userTokenTask.Result,
                    ExpirationDate = DateTimeExtensions.DeserializeDateTime(userTokenExpirationTask.Result)
                });
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Error, () => "Error getting user info", ex);
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
                _logger?.Log(MvxLogLevel.Error, () => "Error getting user token", ex);
                throw;
            }
        }

        public Task SetUserInfoAsync(string userId, string userEmail, string username, string password, string jwtToken, DateTime tokenExpiration)
        {
            try
            {
                return Task.WhenAll(
                    UpdateUserInSettingsAsync(UserId, userId),
                    UpdateUserInSettingsAsync(UserUsername, username),
                    UpdateUserInSettingsAsync(UserEmail, userEmail),
                    UpdateUserInSettingsAsync(UserPassword, password),
                    UpdateUserInSettingsAsync(UserToken, jwtToken),
                    UpdateUserInSettingsAsync(UserTokenExpiration, DateTimeExtensions.SerializeDateTime(tokenExpiration))
                );
            }
            catch (Exception ex)
            {
                _logger?.Log(MvxLogLevel.Error, () => "Error saving user info", ex, userId);
                throw;
            }
        }

        private Task UpdateUserInSettingsAsync(string key, string value) => _settingsRepository.SavePreferenceAsync(key, value, true);

        private Task<string> GetUserInSettingsSettingAsync(string key, string defaultValue = "") => _settingsRepository.GetPreferenceAsync(key, defaultValue, true);
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
        /// <returns></returns>
        Task SetUserInfoAsync(string userId, string userEmail, string username, string password, string jwtToken, DateTime tokenExpiration);
    }
}
