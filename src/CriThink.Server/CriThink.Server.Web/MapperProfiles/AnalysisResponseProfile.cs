using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Providers.DomainAnalyzer;

namespace CriThink.Server.Web.MapperProfiles
{
    public class AnalysisResponseProfile : Profile
    {
        public AnalysisResponseProfile()
        {
            CreateMap<AnalysisResponse, NewsAnalyzerResponse>()
                .ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.Uri.ToString()))
                .ForMember(dest => dest.HasError, opt => opt.MapFrom(src => src.HasError))
                .ForMember(dest => dest.AnalysisEvaluation, opt => opt.MapFrom(src => src.SourceAnalysisScore))
                .ForMember(dest => dest.NewsAnalysisType, (opt) => opt.MapFrom(src => src.Analysis));
        }
    }
}
