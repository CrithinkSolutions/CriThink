using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Infrastructure.Services;

namespace CriThink.Server.Infrastructure.Delegates
{
    public delegate IExternalLoginProvider ExternalLoginProviderResolver(ExternalLoginProvider externalProvider);
}
