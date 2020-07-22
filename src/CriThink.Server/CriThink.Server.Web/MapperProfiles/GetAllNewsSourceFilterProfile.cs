using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsSource.Requests;
using CriThink.Server.Core.Queries;

namespace CriThink.Server.Web.MapperProfiles
{
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
