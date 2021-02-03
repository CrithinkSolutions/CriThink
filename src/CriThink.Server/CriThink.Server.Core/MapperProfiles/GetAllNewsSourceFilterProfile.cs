using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Core.Queries;

namespace CriThink.Server.Core.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class GetAllNewsSourceFilterProfile : Profile
    {
        public GetAllNewsSourceFilterProfile()
        {
            CreateMap<NewsSourceGetAllFilterRequest, GetAllNewsSourceFilter>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsSourceGetAllFilterRequest.None, GetAllNewsSourceFilter.All)
                    .MapValue(NewsSourceGetAllFilterRequest.Good, GetAllNewsSourceFilter.Whitelist)
                    .MapValue(NewsSourceGetAllFilterRequest.Bad, GetAllNewsSourceFilter.Blacklist));
        }
    }
}
