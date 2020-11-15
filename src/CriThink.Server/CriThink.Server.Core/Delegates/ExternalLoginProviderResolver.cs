using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Core.Models.LoginProviders;

namespace CriThink.Server.Core.Delegates
{
    public delegate IExternalLoginProvider ExternalLoginProviderResolver(ExternalLoginProvider externalProvider);
}
