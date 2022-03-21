using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace CriThink.Server.Infrastructure.ExtensionMethods
{
    public static class ClaimsExtensions
    {
        public static Guid GetId(this IPrincipal user)
        {
            var value = GetClaimValue(user, ClaimTypes.NameIdentifier);
            return Guid.Parse(value);
        }

        public static string GetEmail(this IPrincipal user) =>
            GetClaimValue(user, ClaimTypes.Email);

        public static string GetFullName(this IPrincipal user) =>
            $"{GetGivenName(user)}_{GetFamilyName(user)}";

        public static string GetGivenName(this IPrincipal user) =>
            GetClaimValue(user, ClaimTypes.GivenName);

        public static string GetFamilyName(this IPrincipal user) =>
            GetClaimValue(user, ClaimTypes.Surname);

        public static string GetAvatar(this IPrincipal user) =>
            GetClaimValue(user, "avatar");

        public static IEnumerable<string> GetRoles(this IPrincipal user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var values = ((ClaimsPrincipal) user).FindAll(ClaimTypes.Role).Select(c => c.Value);
            return values;
        }

        public static string GetClaimValue(this IPrincipal user, string claimType)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var value = ((ClaimsPrincipal) user).FindFirst(claimType)?.Value;
            return value;
        }
    }
}
