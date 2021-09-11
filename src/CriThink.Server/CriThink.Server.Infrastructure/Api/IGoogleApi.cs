﻿using System.Threading.Tasks;
using CriThink.Server.Domain.Models.DTOs.Google;
using Refit;

namespace CriThink.Server.Infrastructure.Api
{
    public interface IGoogleApi
    {
        [Get("/tokeninfo?id_token={token}")]
        Task<GoogleTokenInfo> GetUserDetailsAsync(string token);
    }
}