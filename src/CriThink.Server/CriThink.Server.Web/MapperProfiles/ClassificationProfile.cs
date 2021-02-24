using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;

namespace CriThink.Server.Web.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class ClassificationProfile : Profile
    {
        public ClassificationProfile()
        {
            CreateMap<NewsSourceClassification, Classification>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsSourceClassification.Unknown, Classification.Unknown)
                    .MapValue(NewsSourceClassification.Conspiracist, Classification.Conspiracist)
                    .MapValue(NewsSourceClassification.FakeNews, Classification.FakeNews)
                    .MapValue(NewsSourceClassification.Reliable, Classification.Reliable)
                    .MapValue(NewsSourceClassification.Satirical, Classification.Satirical)
                    .MapValue(NewsSourceClassification.Suspicious, Classification.Suspicious)
                    .MapValue(NewsSourceClassification.SocialMedia, Classification.SocialMedia));
        }
    }
}
