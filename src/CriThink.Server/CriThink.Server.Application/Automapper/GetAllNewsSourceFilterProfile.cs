using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Application.Queries;

namespace CriThink.Server.Application.Automapper
{
    // ReSharper disable once UnusedMember.Global
    public class GetAllNewsSourceFilterProfile : Profile
    {
        public GetAllNewsSourceFilterProfile()
        {
            CreateMap<NewsSourceGetAllFilterRequest, NewsSourceAuthenticityFilter>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsSourceGetAllFilterRequest.None, NewsSourceAuthenticityFilter.All)
                    .MapValue(NewsSourceGetAllFilterRequest.Good, NewsSourceAuthenticityFilter.Whitelist)
                    .MapValue(NewsSourceGetAllFilterRequest.Bad, NewsSourceAuthenticityFilter.Blacklist));
        }
    }
}
