using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Providers.DomainAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer;

namespace CriThink.Server.Core.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class AnalysisProviderResultsProfile : Profile
    {
        public AnalysisProviderResultsProfile()
        {
            CreateMap<DomainAnalysisProviderResult, NewsAnalyzerResponse>()
                .ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.Uri.ToString()))
                .ForMember(dest => dest.HasError, opt => opt.MapFrom(src => src.HasError))
                .ForMember(dest => dest.AnalysisScore, opt => opt.MapFrom(src => src.TrustworthinessScore))
                .ForMember(dest => dest.NewsAnalysisType, (opt) => opt.MapFrom(src => src.NewsAnalysisType));

            CreateMap<NewsAnalysisProviderResult, NewsAnalyzerResponse>()
                .ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.Uri.ToString()))
                .ForMember(dest => dest.HasError, opt => opt.MapFrom(src => src.HasError))
                .ForMember(dest => dest.AnalysisScore, opt => opt.MapFrom(src => src.TrustworthinessScore))
                .ForMember(dest => dest.NewsAnalysisType, (opt) => opt.MapFrom(src => src.NewsAnalysisType));
        }
    }
}
