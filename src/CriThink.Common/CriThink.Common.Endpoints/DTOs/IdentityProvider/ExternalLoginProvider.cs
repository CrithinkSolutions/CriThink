using System.Runtime.Serialization;

namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public enum ExternalLoginProvider
    {
        [EnumMember(Value = "Facebook")]
        Facebook = 0,

        [EnumMember(Value = "Google")]
        Google = 1,

        [EnumMember(Value = "Apple")]
        Apple = 2,

        None = 99,
    };
}