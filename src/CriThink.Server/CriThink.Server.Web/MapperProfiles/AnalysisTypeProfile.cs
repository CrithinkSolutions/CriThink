using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Providers.DomainAnalyzer;

namespace CriThink.Server.Web.MapperProfiles
{
    public class AnalysisTypeProfile : Profile
    {
        public AnalysisTypeProfile()
        {
            CreateMap<AnalysisType, NewsAnalysisType>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(AnalysisType.HTTPS, NewsAnalysisType.HTTPS)
                    .MapValue(AnalysisType.WhoIs, NewsAnalysisType.WhoIs));
        }
    }
}
