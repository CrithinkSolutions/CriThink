using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Application.Automapper
{
    // ReSharper disable once UnusedMember.Global
    public class NewsSourceAuthenticityProfile : Profile
    {
        public NewsSourceAuthenticityProfile()
        {
            CreateMap<NewsSourceAuthenticityDto, NewsSourceAuthenticity>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(NewsSourceAuthenticityDto.Satirical, NewsSourceAuthenticity.Satirical)
                    .MapValue(NewsSourceAuthenticityDto.FakeNews, NewsSourceAuthenticity.FakeNews)
                    .MapValue(NewsSourceAuthenticityDto.Conspiracist, NewsSourceAuthenticity.Conspiracist)
                    .MapValue(NewsSourceAuthenticityDto.Reliable, NewsSourceAuthenticity.Reliable)
                    .MapValue(NewsSourceAuthenticityDto.Suspicious, NewsSourceAuthenticity.Suspicious)
                    .MapValue(NewsSourceAuthenticityDto.SocialMedia, NewsSourceAuthenticity.SocialMedia));
        }
    }
}
