using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.Admin
{
    public class RoleUpdateNameRequest
    {
        [JsonPropertyName("oldName")]
        [MinLength(3)]
        [Required]
        public string OldName { get; set; }

        [JsonPropertyName("newName")]
        [MinLength(3)]
        [Required]
        public string NewName { get; set; }
    }
}
