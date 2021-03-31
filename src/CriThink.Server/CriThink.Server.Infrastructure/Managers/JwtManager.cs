using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Identity;
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

        public async Task<JwtTokenResponse> GenerateUserTokenAsync(User user)
        {
            var secretKey = _configuration["Jwt-SecretKey"];
            var audience = _configuration["Jwt-Audience"];
            var issuer = _configuration["Jwt-Issuer"];
            var expirationFromNow = _configuration["Jwt-ExpirationInHours"];

            var hasExpiration = double.TryParse(expirationFromNow, out var expirationInHours);
            if (!hasExpiration)
            {
                expirationInHours = 0.5;
                _logger?.LogCritical(new SecretNotFoundException("Token duration.", nameof(IdentityService)), "Used default token duration.");
            }

            // Get user claims
            var claims = await _userRepository.GetUserClaimsAsync(user).ConfigureAwait(false);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            // Get user role's claims
            var userRoles = await _userRepository.GetUserRolesAsync(user).ConfigureAwait(false);
            foreach (var userRole in userRoles)
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole));

            var token = new JwtBuilder()
                .AddAudience(audience)
                .AddClaims(claims)
                .AddIssuer(issuer)
                .AddSecurityKey(signingKey)
                .AddExpireDate(expirationInHours)
                .AddSubject(user.Email)
                .Build();

            var tokenString = _jwtTokenHandler.WriteToken(token);
            return new JwtTokenResponse
            {
                ExpirationDate = token.ValidTo,
                Token = tokenString
            };
        }
    }
}
