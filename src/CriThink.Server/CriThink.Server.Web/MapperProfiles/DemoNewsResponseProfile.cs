using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Web.MapperProfiles
{
    public class DemoNewsResponseProfile : Profile
    {
        public DemoNewsResponseProfile()
        {
            CreateMap<GetAllDemoNewsQueryResponse, DemoNewsResponse>()
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest =>
                    dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest =>
                    dest.Url, opt => opt.MapFrom(src => src.Uri.AbsoluteUri));
        }
    }
}
