using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Domain.Models.LoginProviders;

namespace CriThink.Server.Domain.Delegates
{
    public delegate IExternalLoginProvider ExternalLoginProviderResolver(ExternalLoginProvider externalProvider);
}
