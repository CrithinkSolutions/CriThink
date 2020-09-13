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
                    .MapValue(NewsSourceClassification.Satirical, NewsSourceAuthenticity.Satirical)
                    .MapValue(NewsSourceClassification.FakeNews, NewsSourceAuthenticity.FakeNews)
                    .MapValue(NewsSourceClassification.Conspiracist, NewsSourceAuthenticity.Conspiracist)
                    .MapValue(NewsSourceClassification.Reliable, NewsSourceAuthenticity.Reliable));
        }
    }
}
