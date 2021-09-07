using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Application.Automapper
{
    public class NewsSourceQuestionProfile : Profile
    {
        public NewsSourceQuestionProfile()
        {
            CreateMap<NewsSourcePostQuestion, NewsSourceGetQuestionResponse>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom<LocalizationResolver<NewsSourcePostQuestion, NewsSourceGetQuestionResponse>, string>(src => src.Question));

            CreateMap<GetAllNewsSourceQuestionsQueryResult, NewsSourceGetAllQuestionsResponse>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
        }
    }
}
