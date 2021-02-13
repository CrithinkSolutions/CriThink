using System;
using System.Linq;
using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Core.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class DebunkingNewsProfile : Profile
    {
        public DebunkingNewsProfile()
        {
            CreateMap<DebunkingNewsGetAllLanguageFilterRequests, GetAllDebunkingNewsLanguageFilters>()
                .ConvertUsingEnumMapping(opt => opt
                    .MapValue(DebunkingNewsGetAllLanguageFilterRequests.None, GetAllDebunkingNewsLanguageFilters.All)
                    .MapValue(DebunkingNewsGetAllLanguageFilterRequests.English, GetAllDebunkingNewsLanguageFilters.English)
                    .MapValue(DebunkingNewsGetAllLanguageFilterRequests.Italian, GetAllDebunkingNewsLanguageFilters.Italian));

            CreateMap<GetAllDebunkingNewsQueryResponse, DebunkingNewsGetResponse>()
                .ForMember(dest =>
                    dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest =>
                    dest.Publisher, opt => opt.MapFrom(src => src.Publisher))
                .ForMember(dest =>
                    dest.PublisherCountry, opt => opt.MapFrom(src => src.PublisherCountry))
                .ForMember(dest =>
                    dest.PublisherLanguage, opt => opt.MapFrom(src => src.PublisherLanguage))
                .ForMember(dest =>
                    dest.NewsLink, opt => opt.MapFrom(src => src.NewsLink))
                .ForMember(dest =>
                    dest.NewsImageLink, opt => opt.MapFrom(src => src.NewsImageLink));

            CreateMap<DebunkingNews, DebunkingNewsGetDetailsResponse>()
                .ForMember(dest =>
                    dest.Caption, opt => opt.MapFrom(src => src.NewsCaption))
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.Link, opt => opt.MapFrom(src => src.Link))
                .ForMember(dest =>
                    dest.Publisher, opt => opt.MapFrom(src => src.Publisher.Name))
                .ForMember(dest =>
                    dest.PublisherCountry, opt => opt.MapFrom(src => src.Publisher.Country.Name))
                .ForMember(dest =>
                    dest.PublisherLanguage, opt => opt.MapFrom(src => src.Publisher.Language.Name))
                .ForMember(dest =>
                    dest.PublishingDate, opt => opt.MapFrom(src => src.PublishingDate.ToShortDateString()))
                .ForMember(dest =>
                    dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest =>
                    dest.Keywords, opt => opt.MapFrom(src => src.Keywords.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList()))
                .ForMember(dest =>
                    dest.ImageLink, opt => opt.MapFrom(src => src.ImageLink));
        }
    }
}
