using AutoMapper;
using CriThink.Server.Core.Entities;
using CriThink.Web.Models.DTOs.IdentityProvider;

namespace CriThink.Server.Web.MapperProfiles
{
    /// <summary>
    /// Mapper for the <see cref="User" /> entity
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserSignUpRequest, UserCompleteSignUpRequest>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
