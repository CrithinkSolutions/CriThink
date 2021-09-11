using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Application.Automapper
{
    /// <summary>
    /// Mapper for the <see cref="Entities.UserProfile" /> entity
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<GenderDto, Gender>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(GenderDto.Male, Gender.Male)
                    .MapValue(GenderDto.Female, Gender.Female)
                    .MapValue(GenderDto.GenderFluid, Gender.GenderFluid)
                    .MapValue(GenderDto.GenderNeutral, Gender.GenderNeutral)
                    .MapValue(GenderDto.NonBinary, Gender.NonBinary));

            CreateMap<UserProfileUpdateRequest, Domain.Entities.UserProfile>()
                .ForMember(dest => dest.FamilyName, opt =>
                    opt.MapFrom(src => src.FamilyName))
                .ForMember(dest => dest.GivenName, opt =>
                    opt.MapFrom(src => src.GivenName))
                .ForMember(dest => dest.DateOfBirth, opt =>
                    opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Blog, opt =>
                    opt.MapFrom(src => src.Blog))
                .ForMember(dest => dest.Description, opt =>
                    opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Telegram, opt =>
                    opt.MapFrom(src => src.Telegram))
                .ForMember(dest => dest.Country, opt =>
                    opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Facebook, opt =>
                    opt.MapFrom(src => src.Facebook))
                .ForMember(dest => dest.Gender, opt =>
                    opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Skype, opt =>
                    opt.MapFrom(src => src.Skype))
                .ForMember(dest => dest.Snapchat, opt =>
                    opt.MapFrom(src => src.Snapchat))
                .ForMember(dest => dest.Twitter, opt =>
                    opt.MapFrom(src => src.Twitter))
                .ForMember(dest => dest.Youtube, opt =>
                    opt.MapFrom(src => src.Youtube))
                .ForMember(dest => dest.Instagram, opt =>
                    opt.MapFrom(src => src.Instagram))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredOn, opt => opt.Ignore());

            CreateMap<Domain.Entities.UserProfile, UserProfileGetResponse>()
                .ForMember(dest => dest.FamilyName, opt =>
                    opt.MapFrom(src => src.FamilyName))
                .ForMember(dest => dest.GivenName, opt =>
                    opt.MapFrom(src => src.GivenName))
                .ForMember(dest => dest.DateOfBirth, opt =>
                    opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Blog, opt =>
                    opt.MapFrom(src => src.Blog))
                .ForMember(dest => dest.Description, opt =>
                    opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Telegram, opt =>
                    opt.MapFrom(src => src.Telegram))
                .ForMember(dest => dest.Country, opt =>
                    opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Facebook, opt =>
                    opt.MapFrom(src => src.Facebook))
                .ForMember(dest => dest.Gender, opt =>
                    opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Skype, opt =>
                    opt.MapFrom(src => src.Skype))
                .ForMember(dest => dest.Snapchat, opt =>
                    opt.MapFrom(src => src.Snapchat))
                .ForMember(dest => dest.Twitter, opt =>
                    opt.MapFrom(src => src.Twitter))
                .ForMember(dest => dest.Youtube, opt =>
                    opt.MapFrom(src => src.Youtube))
                .ForMember(dest => dest.Instagram, opt =>
                    opt.MapFrom(src => src.Instagram))
                .ForMember(dest => dest.RegisteredOn, opt =>
                    opt.MapFrom(src => src.RegisteredOn.ToShortDateString()))
                .ForMember(dest => dest.DateOfBirth, opt =>
                    opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.AvatarPath, opt =>
                    opt.MapFrom(src => src.AvatarPath))
                .ForMember(dest => dest.Username, opt =>
                    opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Email, opt =>
                    opt.MapFrom(src => src.User.Email));
        }
    }
}