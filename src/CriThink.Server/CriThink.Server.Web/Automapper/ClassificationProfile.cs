using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;

namespace CriThink.Server.Web.Automapper
{
    // ReSharper disable once UnusedMember.Global
    public class ClassificationProfile : Profile
    {
        public ClassificationProfile()
        {
            CreateMap<NewsSourceAuthenticityViewModel, NewsSourceAuthenticity>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsSourceAuthenticityViewModel.Unknown, NewsSourceAuthenticity.Unknown)
                    .MapValue(NewsSourceAuthenticityViewModel.Conspiracist, NewsSourceAuthenticity.Conspiracist)
                    .MapValue(NewsSourceAuthenticityViewModel.FakeNews, NewsSourceAuthenticity.FakeNews)
                    .MapValue(NewsSourceAuthenticityViewModel.Reliable, NewsSourceAuthenticity.Reliable)
                    .MapValue(NewsSourceAuthenticityViewModel.Satirical, NewsSourceAuthenticity.Satirical)
                    .MapValue(NewsSourceAuthenticityViewModel.Suspicious, NewsSourceAuthenticity.Suspicious)
                    .MapValue(NewsSourceAuthenticityViewModel.SocialMedia, NewsSourceAuthenticity.SocialMedia));
        }
    }
}
