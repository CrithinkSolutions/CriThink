using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace CriThink.Server.Web.Jwt
{
    /// <summary>
    /// Builder class to generate a JWT token
    /// </summary>
    public class JwtBuilder
    {
        private string _issuer;

        private string _audience;

        private SymmetricSecurityKey _securityKey;

        private string _subject;

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

        public JwtBuilder AddSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public JwtBuilder AddClaims(IList<Claim> claims)
        {
            _claims = claims;
            return this;
        }

        public JwtSecurityToken Build()
        {
            var now = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
            };

            return new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims.Union(_claims),
                notBefore: now,
                expires: now.AddMinutes(10),
                signingCredentials: new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
