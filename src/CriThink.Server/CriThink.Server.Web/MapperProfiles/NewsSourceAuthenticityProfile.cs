using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Web.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class NewsSourceAuthenticityProfile : Profile
    {
        public NewsSourceAuthenticityProfile()
        {
            CreateMap<NewsSourceClassification, NewsSourceAuthencity>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsSourceClassification.Satiric, NewsSourceAuthencity.Satiric)
                    .MapValue(NewsSourceClassification.Fake, NewsSourceAuthencity.Fake)
                    .MapValue(NewsSourceClassification.Cospiracy, NewsSourceAuthencity.Cospiracy)
                    .MapValue(NewsSourceClassification.Trusted, NewsSourceAuthencity.Trusted));
        }
    }
}
