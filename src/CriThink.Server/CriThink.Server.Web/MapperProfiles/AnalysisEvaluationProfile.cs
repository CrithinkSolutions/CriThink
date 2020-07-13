using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Providers.DomainAnalyzer;

namespace CriThink.Server.Web.MapperProfiles
{
    public class AnalysisEvaluationProfile : Profile
    {
        public AnalysisEvaluationProfile()
        {
            CreateMap<SourceAnalysisScore, AnalysisEvaluation>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(SourceAnalysisScore.Trust, AnalysisEvaluation.Good)
                    .MapValue(SourceAnalysisScore.Warning, AnalysisEvaluation.Warning)
                    .MapValue(SourceAnalysisScore.Untrusted, AnalysisEvaluation.Bad)
                    .MapValue(SourceAnalysisScore.Unknown, AnalysisEvaluation.Unknown));
        }
    }
}
