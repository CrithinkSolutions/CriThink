using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Article;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Core.MapperProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleQuestion, ArticleGetQuestionResponse>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom<LocalizationResolver<ArticleQuestion, ArticleGetQuestionResponse>, string>(src => src.Question));

            CreateMap<GetAllArticleQuestionsResponse, ArticleGetAllQuestionsResponse>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
        }
    }
}
