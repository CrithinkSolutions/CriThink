using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
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

            CreateMap<User, UserGetResponse>()
                .ForMember(dest =>
                    dest.UserId, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest =>
                    dest.AccessFailedCount, opt => opt.MapFrom(src => src.AccessFailedCount))
                .ForMember(dest =>
                    dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest =>
                    dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest =>
                    dest.IsLockoutEnabled, opt => opt.MapFrom(src => src.LockoutEnabled))
                .ForMember(dest =>
                    dest.LockoutEnd, opt => opt.MapFrom(src => src.LockoutEnd))
                .ForMember(dest =>
                    dest.UserEmail, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest =>
                    dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest =>
                    dest.Roles, opt => opt.Ignore());

            CreateMap<User, UserGetAllResponse>()
                .ForMember(dest =>
                    dest.UserId, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest =>
                    dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest =>
                    dest.UserEmail, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest =>
                    dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest =>
                    dest.Roles, opt => opt.Ignore());
        }
    }
}
