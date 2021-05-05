using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UserProfile;

namespace CriThink.Server.Core.Interfaces
{
    public interface IUserProfileService
    {
        Task UpdateUserProfileAsync(UserProfileUpdateRequest request);
    }
}
