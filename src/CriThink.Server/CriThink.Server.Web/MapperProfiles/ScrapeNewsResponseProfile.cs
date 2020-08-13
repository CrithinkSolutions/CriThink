using System.Globalization;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;
using CriThink.Server.Providers.NewsAnalyzer;

namespace CriThink.Server.Web.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class ScrapeNewsResponseProfile : Profile
    {
        public ScrapeNewsResponseProfile()
        {
            CreateMap<NewsScraperProviderResponse, ScrapeNewsResponse>()
                .ForMember(dest =>
                    dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest =>
                    dest.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest =>
                    dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest =>
                    dest.Body, opt => opt.MapFrom(src => src.NewsBody))
                .ForMember(dest =>
                    dest.IsSuccessfullyParsed, opt => opt.MapFrom(src => src.IsReadable))
                .ForMember(dest =>
                    dest.PublishingDate, opt => opt.MapFrom(src => src.Date == null ? string.Empty : src.Date.Value.ToString(CultureInfo.CurrentCulture)))
                .ForMember(dest =>
                    dest.ReadingMinutes, opt => opt.MapFrom(src => src.TimeToRead.Minutes))
                .ForMember(dest =>
                    dest.Website, opt => opt.MapFrom(src => src.WebSiteName));
        }
    }
}
