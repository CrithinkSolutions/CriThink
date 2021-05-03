using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Infrastructure.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CriThink.Server.Infrastructure.Managers
{
    internal class JwtManager : IJwtManager
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<JwtManager> _logger;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler;

        public JwtManager(IConfiguration configuration, IUserRepository userRepository, ILogger<JwtManager> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtTokenHandler = new JwtSecurityTokenHandler();
            _logger = logger;
        }

        public string GenerateToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var secretKey = GetSecretKey();
            var audience = GetAudience();
            var issuer = GetIssuer();

            try
            {
                var principals = _jwtTokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateLifetime = false // we check expired tokens here
                }, out _);

                return principals;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "The given JWT token is invalid");
                return null;
            }
        }

        public async Task<JwtTokenResponse> GenerateUserJwtTokenAsync(User user)
        {
            var secretKey = GetSecretKey();
            var audience = GetAudience();
            var issuer = GetIssuer();
            var expirationFromNow = GetJwtExpiration();

            var claims = await _userRepository.GetUserClaimsAsync(user).ConfigureAwait(false);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtBuilder()
                .AddAudience(audience)
                .AddClaims(claims)
                .AddIssuer(issuer)
                .AddSecurityKey(signingKey)
                .AddExpireDateFromNowUtc(expirationFromNow)
                .Build();

            var tokenString = _jwtTokenHandler.WriteToken(token);
            return new JwtTokenResponse
            {
                ExpirationDate = token.ValidTo,
                Token = tokenString
            };
        }

        public TimeSpan GetDefaultRefreshTokenLifetime() =>
            GetRefreshExpiration();

        private string GetSecretKey() =>
            _configuration["Jwt-SecretKey"];

        private string GetAudience() =>
            _configuration["Jwt-Audience"];

        private string GetIssuer() =>
            _configuration["Jwt-Issuer"];

        private TimeSpan GetJwtExpiration() =>
            _configuration.GetValue<TimeSpan>("Jwt-ExpirationFromNow");

        private TimeSpan GetRefreshExpiration() =>
            _configuration.GetValue<TimeSpan>("Refresh-ExpirationFromNow");
    }
}
