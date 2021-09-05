using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Server.Core.Entities;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;

namespace CriThink.Server.Web.Automapper
{
    // ReSharper disable once UnusedMember.Global
    public class ClassificationProfile : Profile
    {
        public ClassificationProfile()
        {
            CreateMap<Classification, NewsSourceAuthenticity>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(Classification.Unknown, NewsSourceAuthenticity.Unknown)
                    .MapValue(Classification.Conspiracist, NewsSourceAuthenticity.Conspiracist)
                    .MapValue(Classification.FakeNews, NewsSourceAuthenticity.FakeNews)
                    .MapValue(Classification.Reliable, NewsSourceAuthenticity.Reliable)
                    .MapValue(Classification.Satirical, NewsSourceAuthenticity.Satirical)
                    .MapValue(Classification.Suspicious, NewsSourceAuthenticity.Suspicious)
                    .MapValue(Classification.SocialMedia, NewsSourceAuthenticity.SocialMedia));
        }
    }
}
