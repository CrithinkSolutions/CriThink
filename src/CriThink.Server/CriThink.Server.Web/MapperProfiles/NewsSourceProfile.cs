using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Web.MapperProfiles
{
    public class NewsSourceProfile : Profile
    {
        public NewsSourceProfile()
        {
            CreateMap<GetAllNewsSourceQueryResponse, NewsSourceGetAllResponse>()
                .ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest => dest.NewsSourceClassification, opt => opt.MapFrom(src => src.SourceAuthencity));
        }
    }
}
