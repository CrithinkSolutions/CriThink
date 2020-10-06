﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class AdminSignUpRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
