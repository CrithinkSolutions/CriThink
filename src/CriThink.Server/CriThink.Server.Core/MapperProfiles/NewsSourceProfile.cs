using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
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
                    opt => opt.MapFrom(src => src.NewsLink.ToLowerInvariant()))
                .ForMember(dest => dest.NewsSourceClassification,
                    opt => opt.MapFrom(src => src.SourceAuthencity));

            CreateMap<SearchNewsSourceQueryResponse, NewsSourceSearchResponse>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom<LocalizationResolver<SearchNewsSourceQueryResponse, NewsSourceSearchResponse>, string>(src => src.Description))
                .ForMember(dest => dest.Classification,
                    opt => opt.MapFrom((src, subDest, dest, ctx) => ctx.Mapper.Map<NewsSourceAuthenticity, NewsSourceClassification>(src.SourceAuthenticity)));

            CreateMap<GetAllDebunkingNewsByKeywordsQueryResponse, NewsSourceRelatedDebunkingNewsResponse>()
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest =>
                    dest.Publisher, opt => opt.MapFrom(src => src.PublisherName))
                .ForMember(dest =>
                    dest.NewsLink, opt => opt.MapFrom(src => src.NewsLink))
                .ForMember(dest =>
                    dest.PublishingDate, opt => opt.MapFrom(src => src.PublishingDate))
                .ForMember(dest =>
                    dest.Caption, opt => opt.MapFrom(src => src.NewsCaption))
                .ForMember(dest =>
                    dest.NewsImageLink, opt => opt.MapFrom(src => src.NewsImageLink))
                .ForMember(dest =>
                    dest.Title, opt => opt.MapFrom(src => src.Title));
        }
    }
}
