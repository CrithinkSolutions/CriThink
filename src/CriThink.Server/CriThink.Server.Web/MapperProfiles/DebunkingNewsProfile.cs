using System;
using System.Linq;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Web.MapperProfiles
{
    // ReSharper disable once UnusedMember.Global
    public class DebunkingNewsProfile : Profile
    {
        public DebunkingNewsProfile()
        {
            CreateMap<GetAllDebunkingNewsQueryResponse, DebunkingNewsGetAllResponse>()
                .ForMember(dest =>
                    dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest =>
                    dest.Publisher, opt => opt.MapFrom(src => src.Publisher));

            CreateMap<DebunkingNews, DebunkingNewsGetResponse>()
                .ForMember(dest =>
                    dest.Caption, opt => opt.MapFrom(src => src.NewsCaption))
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.Link, opt => opt.MapFrom(src => src.Link))
                .ForMember(dest =>
                    dest.Publisher, opt => opt.MapFrom(src => src.PublisherName))
                .ForMember(dest =>
                    dest.PublishingDate, opt => opt.MapFrom(src => src.PublishingDate.ToShortDateString()))
                .ForMember(dest =>
                    dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest =>
                    dest.Keywords, opt => opt.MapFrom(src => src.Keywords.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList()));
        }
    }
}
