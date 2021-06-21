﻿using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Core.MapperProfiles
{
    public class UnknownNewsSourceProfile : Profile
    {
        public UnknownNewsSourceProfile()
        {
            CreateMap<UnknownNewsSource, Common.Endpoints.DTOs.UnknownNewsSource.UnknownNewsSourceResponse>()
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.Uri, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest =>
                    dest.Classification, opt => opt.MapFrom((src, subDest, dest, ctx) => ctx.Mapper.Map<NewsSourceAuthenticity, NewsSourceClassification>(src.Authenticity)));

            CreateMap<GetAllSubscribedUsersWithSourceResponse, NotificationRequestGetResponse>()
                .ForMember(dest =>
                    dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.RequestCount, opt => opt.MapFrom(src => src.RequestCount))
                .ForMember(dest =>
                    dest.RequestedAt, opt => opt.MapFrom(src => src.RequestedAt))
                .ForMember(dest =>
                    dest.RequestedLink, opt => opt.MapFrom(src => src.Domain));

            CreateMap<GetAllUnknownSources, UnknownNewsSourceGetResponse>()
                .ForMember(dest =>
                    dest.Authenticity, opt => opt.MapFrom(src => src.NewsSourceAuthenticity.ToString()))
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.IdentifiedAt, opt => opt.MapFrom(src => src.IdentifiedAt))
                .ForMember(dest =>
                    dest.RequestCount, opt => opt.MapFrom(src => src.RequestCount))
                .ForMember(dest =>
                    dest.Uri, opt => opt.MapFrom(src => src.Domain))
                .ForMember(dest =>
                    dest.RequestedAt, opt => opt.MapFrom(src => src.RequestedAt));
        }
    }
}
