using System.ComponentModel.DataAnnotations;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class UsernameAvailabilityRequest
    {
        [Required]
        [MinLength(2)]
        public string Username { get; set; }
    }
}
