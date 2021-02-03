using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Helpers;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Core.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class NewsSourceProfile : Profile
    {
        public NewsSourceProfile()
        {
            CreateMap<GetAllNewsSourceQueryResponse, NewsSourceGetResponse>()
                .ForMember(dest => dest.Uri,
                    opt => opt.MapFrom(src => UriHelper.GetHostNameFromUri(src.Uri)))
                .ForMember(dest => dest.NewsSourceClassification,
                    opt => opt.MapFrom(src => src.SourceAuthencity));

            CreateMap<SearchNewsSourceQueryResponse, NewsSourceSearchResponse>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Classification,
                    opt => opt.MapFrom((src, subDest, dest, ctx) => ctx.Mapper.Map<NewsSourceAuthenticity, NewsSourceClassification>(src.SourceAuthenticity)));
        }
    }
}
