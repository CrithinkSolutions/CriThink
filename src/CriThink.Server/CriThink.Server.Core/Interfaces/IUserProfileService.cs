using System;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Core.Interfaces
{
    public interface IUserProfileService
    {
        /// <summary>
        /// Update user profile information
        /// </summary>
        /// <param name="request">New information</param>
        /// <returns></returns>
        Task UpdateUserProfileAsync(UserProfileUpdateRequest request);

        /// <summary>
        /// Updates the user avatar
        /// </summary>
        /// <param name="formFile">File uploaded by the user</param>
        /// <returns></returns>
        Task<Uri> UpdateUserAvatarAsync(IFormFile formFile);

        /// <summary>
        /// Updates the user avatar
        /// </summary>
        /// <param name="bytes">File uploaded by the user</param>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        Task<Uri> UpdateUserAvatarAsync(byte[] bytes, string userId);

        /// <summary>
        /// Return all user details
        /// </summary>
        /// <returns></returns>
        Task<UserProfileGetResponse> GetUserProfileAsync();
    }
}
