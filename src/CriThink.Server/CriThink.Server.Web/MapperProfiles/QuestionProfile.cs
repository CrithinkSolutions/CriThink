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
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Question, QuestionResponse>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));
        }
    }
}
