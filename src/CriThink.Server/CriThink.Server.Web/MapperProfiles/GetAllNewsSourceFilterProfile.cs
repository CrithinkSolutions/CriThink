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
            CreateMap<NewsSourceGetAllRequest, GetAllNewsSourceFilter>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsSourceGetAllRequest.None, GetAllNewsSourceFilter.All)
                    .MapValue(NewsSourceGetAllRequest.Good, GetAllNewsSourceFilter.Whitelist)
                    .MapValue(NewsSourceGetAllRequest.Bad, GetAllNewsSourceFilter.Blacklist));
        }
    }
}
