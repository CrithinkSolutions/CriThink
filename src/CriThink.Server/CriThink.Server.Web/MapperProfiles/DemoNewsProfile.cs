using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Web.MapperProfiles
{
    public class DemoNewsProfile : Profile
    {
        public DemoNewsProfile()
        {
            CreateMap<DemoNewsAddRequest, DemoNews>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Uri));
        }
    }
}
