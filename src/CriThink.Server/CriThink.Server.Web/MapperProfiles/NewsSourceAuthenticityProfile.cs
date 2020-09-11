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
            CreateMap<NewsSourceClassification, NewsSourceAuthenticity>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsSourceClassification.Satiric, NewsSourceAuthenticity.Satiric)
                    .MapValue(NewsSourceClassification.Fake, NewsSourceAuthenticity.Fake)
                    .MapValue(NewsSourceClassification.Cospiracy, NewsSourceAuthenticity.Cospiracy)
                    .MapValue(NewsSourceClassification.Trusted, NewsSourceAuthenticity.Trusted));
        }
    }
}
