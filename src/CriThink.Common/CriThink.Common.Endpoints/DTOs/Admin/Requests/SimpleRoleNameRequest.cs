using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class SimpleRoleNameRequest
    {
        [JsonPropertyName("name")]
        [MinLength(3)]
        [Required]
        public string Name { get; set; }
    }
}
