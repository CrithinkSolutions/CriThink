using System;
using System.Threading.Tasks;
using CriThink.Client.Core.Services.Contracts;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

namespace CriThink.Client.Core.Services
{
    internal class IdentityService : IIdentityService
    {
        public async Task<string> LoginUserAsync(UserLoginRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return string.Empty; ;
        }
    }

    internal class RestRepository : IRestRepository
    {

        public RestRepository()
        {

        }
    }

    public interface IRestRepository
    {

    }

}
