using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Web.Interfaces;

namespace CriThink.Server.Web.Delegates
{
    public delegate IExternalLoginProvider ExternalLoginProviderResolver(ExternalLoginProvider externalProvider);
}
