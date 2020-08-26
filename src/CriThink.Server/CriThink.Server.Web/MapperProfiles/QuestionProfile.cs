using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Web.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<QuestionAddRequest, Question>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
