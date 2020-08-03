using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Providers.DomainAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer;

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

            CreateMap<NewsAnalysisScore, AnalysisEvaluation>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsAnalysisScore.Trustworthy, AnalysisEvaluation.Good)
                    .MapValue(NewsAnalysisScore.NotTrusted, AnalysisEvaluation.Bad)
                    .MapValue(NewsAnalysisScore.Unknown, AnalysisEvaluation.Unknown));
        }
    }
}
