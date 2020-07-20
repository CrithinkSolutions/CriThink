using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Web.MapperProfiles
{
    public class NewsSourceAuthenticityProfile : Profile
    {
        public NewsSourceAuthenticityProfile()
        {
            CreateMap<NewsSourceClassification, NewsSourceAuthencity>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsSourceClassification.Authentic, NewsSourceAuthencity.Authentic)
                    .MapValue(NewsSourceClassification.Fake, NewsSourceAuthencity.Fake)
                    .MapValue(NewsSourceClassification.NotTrusted, NewsSourceAuthencity.Untrusted)
                    .MapValue(NewsSourceClassification.Secure, NewsSourceAuthencity.Trusted));
        }
    }
}
