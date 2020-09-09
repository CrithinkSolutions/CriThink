using AutoMapper;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Web.MapperProfiles
{
    /// <summary>
    /// Mapper for the <see cref="User" /> entity
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserSignUpRequest, UserCompleteSignUpRequest>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
