using AutoMapper;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using CriThink.Server.Application.Administration.ViewModels;
using CriThink.Server.Domain.Entities;

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
            CreateMap<UserSearch, UserProfileGetRecentSearchResponse>()
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.NewsLink, opt => opt.MapFrom(src => src.NewsLink));

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
