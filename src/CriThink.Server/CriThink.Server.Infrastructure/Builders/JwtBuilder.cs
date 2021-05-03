using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using CriThink.Common.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace CriThink.Server.Infrastructure.Builders
{
    internal class JwtBuilder
    {
        private string _issuer;
        private string _audience;
        private SymmetricSecurityKey _securityKey;
        private TimeSpan _expirationFromNow;

        private IList<Claim> _claims;

        public JwtBuilder AddIssuer(string issuer)
        {
            _issuer = issuer;
            return this;
        }

        public JwtBuilder AddAudience(string audience)
        {
            _audience = audience;
            return this;
        }

        public JwtBuilder AddSecurityKey(SymmetricSecurityKey key)
        {
            _securityKey = key;
            return this;
        }

        public JwtBuilder AddClaims(IList<Claim> claims)
        {
            _claims = claims;
            return this;
        }

        public JwtBuilder AddExpireDateFromNowUtc(TimeSpan hoursFromNow)
        {
            _expirationFromNow = hoursFromNow;
            return this;
        }

        public JwtSecurityToken Build()
        {
            var now = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Iat, DateTimeExtensions.SerializeDateTime(now), ClaimValueTypes.Integer64)
            }.Union(_claims);

            return new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_expirationFromNow),
                signingCredentials: new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
