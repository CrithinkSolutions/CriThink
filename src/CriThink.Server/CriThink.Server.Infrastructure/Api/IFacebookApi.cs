﻿using System.Threading.Tasks;
using CriThink.Server.Core.Models.DTOs.Facebook;
using Refit;

namespace CriThink.Server.Infrastructure.Api
{
    public interface IFacebookApi
    {
        [Get("/debug_token?input_token={userToken}&access_token={accessToken}")]
        Task<FacebookTokenResponse> ValidateTokenAsync(string userToken, string accessToken);

        [Get("/me?fields=id,first_name,last_name,name,picture,email&access_token={accessToken}")]
        Task<FacebookUserInfoDetail> GetUserDetailsAsync(string userId, string accessToken);
    }
}
