using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Core.Providers;

namespace CriThink.Server.Web.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class AnalysisTypeProfile : Profile
    {
        public AnalysisTypeProfile()
        {
            CreateMap<NewsAnalysisType, NewsAnalysisTypeResponse>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsAnalysisType.HTTPS, NewsAnalysisTypeResponse.HTTPS)
                    .MapValue(NewsAnalysisType.WhoIs, NewsAnalysisTypeResponse.WhoIs));
        }
    }
}
