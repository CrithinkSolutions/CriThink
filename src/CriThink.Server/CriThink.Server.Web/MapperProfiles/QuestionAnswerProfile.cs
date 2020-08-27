using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Web.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class QuestionAnswerProfile : Profile
    {
        public QuestionAnswerProfile()
        {
            CreateMap<QuestionAnswerAddRequest, QuestionAnswer>()
                .ForMember(dest =>
                    dest.Question, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.DemoNews, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.IsTrue, opt => opt.MapFrom(src => src.IsPositive))
                .ForMember(dest =>
                    dest.Id, opt => opt.Ignore());
        }
    }
}
