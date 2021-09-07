using AutoMapper;
using CriThink.Server.Application.Administration.ViewModels;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Application.Automapper
{
    /// <summary>
    /// Mapper for the <see cref="User" /> entity
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //CreateMap<User, UserGetDetailsViewModel>()
            //    .ForMember(dest =>
            //        dest.UserId, opt => opt.MapFrom(src => src.Id.ToString()))
            //    .ForMember(dest =>
            //        dest.AccessFailedCount, opt => opt.MapFrom(src => src.AccessFailedCount))
            //    .ForMember(dest =>
            //        dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            //    .ForMember(dest =>
            //        dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
            //    .ForMember(dest =>
            //        dest.IsLockoutEnabled, opt => opt.MapFrom(src => src.LockoutEnabled))
            //    .ForMember(dest =>
            //        dest.LockoutEnd, opt => opt.MapFrom(src => src.LockoutEnd))
            //    .ForMember(dest =>
            //        dest.UserEmail, opt => opt.MapFrom(src => src.Email))
            //    .ForMember(dest =>
            //        dest.UserName, opt => opt.MapFrom(src => src.UserName))
            //    .ForMember(dest =>
            //        dest.Roles, opt => opt.Ignore());

            CreateMap<User, UserGetViewModel>()
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
