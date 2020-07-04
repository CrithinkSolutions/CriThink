using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Web.Exceptions;
using CriThink.Server.Web.Jwt;
using CriThink.Web.Models.DTOs.IdentityProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CriThink.Server.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(UserManager<User> userManager, IConfiguration configuration, ILogger<IdentityService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;

            _jwtTokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<UserSignUpResponse> CreateNewUserAsync(UserSignUpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var userCreationResult = await _userManager.CreateAsync(user, request.Password).ConfigureAwait(false);
            if (!userCreationResult.Succeeded)
            {
                var ex = new IdentityOperationException(userCreationResult);
                _logger?.LogError(ex, "Error creating a new user", user);
                throw ex;
            }

            var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user)
                .ConfigureAwait(false);

            return new UserSignUpResponse
            {
                UserId = user.Id.ToString(),
                UserEmail = user.Email,
                ConfirmationCode = confirmationCode
            };
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = await FindUserAsync(email: request.Email, userName: request.UserName).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"Email: '{request.Email}' - Username: '{request.UserName}'");

            var verificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            await ProcessPasswordVerificationResult(user, verificationResult).ConfigureAwait(false);

            var jwtToken = await GenerateTokenAsync(user).ConfigureAwait(false);
            var response = new UserLoginResponse
            {
                UserId = user.Id.ToString(),
                UserEmail = user.Email,
                UserName = user.UserName,
                JwtToken = jwtToken
            };

            return response;
        }

        public async Task<VerifyUserEmailResponse> VerifyAccountEmailAsync(string userId, string confirmationCode)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrWhiteSpace(confirmationCode))
                throw new ArgumentNullException(nameof(confirmationCode));

            var user = await FindUserAsync(userId: userId).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"UserId: '{userId}'");

            var result = await _userManager.ConfirmEmailAsync(user, confirmationCode).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var ex = new IdentityOperationException(result);
                _logger?.LogError(ex, "Error verifying user email", user, confirmationCode);
                throw ex;
            }

            var jwtToken = await GenerateTokenAsync(user).ConfigureAwait(false);

            return new VerifyUserEmailResponse
            {
                UserId = userId,
                JwtToken = jwtToken,
                UserEmail = user.Email
            };
        }

        public async Task<bool> ChangeUserPasswordAsync(string email, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new ArgumentNullException(nameof(currentPassword));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"User email: '{email}'");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword)
                .ConfigureAwait(false);

            if (result.Succeeded)
                return true;

            var ex = new IdentityOperationException(result);
            _logger?.LogError(ex, "Error changing user password", user);
            throw ex;
        }

        #region Privates

        private async Task<User> FindUserAsync(string email = "", string userName = "", string userId = "")
        {
            if (!string.IsNullOrWhiteSpace(email))
                return await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(userName))
                return await _userManager.FindByNameAsync(userName).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(userId))
                return await _userManager.FindByIdAsync(userId).ConfigureAwait(false);

            return null;
        }

        private async Task<JwtTokenResponse> GenerateTokenAsync(User user)
        {
            var secretKey = _configuration["Jwt-SecretKey"];
            var audience = _configuration["Jwt-Audience"];
            var issuer = _configuration["Jwt-Issuer"];

            var claims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var token = new JwtBuilder()
                .AddAudience(audience)
                .AddClaims(claims)
                .AddIssuer(issuer)
                .AddSecurityKey(signingKey)
                .AddSubject(user.Email)
                .Build();

            var tokenString = _jwtTokenHandler.WriteToken(token);
            return new JwtTokenResponse(tokenString, token.ValidTo);
        }

        private async Task ProcessPasswordVerificationResult(User user, PasswordVerificationResult verificationResult)
        {
            switch (verificationResult)
            {
                case PasswordVerificationResult.Failed:
                    throw new ResourceNotFoundException("Password is not correct");
                case PasswordVerificationResult.SuccessRehashNeeded:
                    await UpdateUserPasswordHashAsync(user).ConfigureAwait(false);
                    break;
            }
        }

        private async Task UpdateUserPasswordHashAsync(User user)
        {
            var result = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);
            if (!result)
            {
                var ex = new InvalidOperationException("Error hashing again user password");
                _logger.LogError(ex, "Rehash needed but failed", user);
            }
        }

        #endregion
    }

    public interface IIdentityService
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="request">DTO with user information</param>
        /// <returns>The operation result</returns>
        Task<UserSignUpResponse> CreateNewUserAsync(UserSignUpRequest request);

        /// <summary>
        /// Login the given user
        /// </summary>
        /// <param name="request">DTO with user information</param>
        /// <returns>The operation result. It contains the token if successful</returns>
        Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request);
        /// <summary>
        /// Verify the user email through the email link
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="confirmationCode"></param>
        /// <returns>The operation result</returns>
        Task<VerifyUserEmailResponse> VerifyAccountEmailAsync(string userId, string confirmationCode);

        /// <summary>
        /// Allow user to change its personal password
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="currentPassword">Current password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Returns true if the password is changed, otherwise false</returns>
        Task<bool> ChangeUserPasswordAsync(string email, string currentPassword, string newPassword);
    }
}
