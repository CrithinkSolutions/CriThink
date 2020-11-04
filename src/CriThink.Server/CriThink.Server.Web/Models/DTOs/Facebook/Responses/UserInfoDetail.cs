using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CriThink.Server.Web.Models.DTOs.Facebook
{
    public class UserInfoDetail
    {
        
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        
        [JsonPropertyName("picture")]
        public Picture Picture { get; set; }
        
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class Picture
    {
        [JsonPropertyName("data")]
        public UserInfoData Data { get; set; }
    }

    public class UserInfoData
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }
        
        [JsonPropertyName("is_silhouette")]
        public bool IsSilhouette { get; set; }
        
        [JsonPropertyName("url")]
        public string Url { get; set; }
        
        [JsonPropertyName("width")]
        public int Width { get; set; }
    }

}
