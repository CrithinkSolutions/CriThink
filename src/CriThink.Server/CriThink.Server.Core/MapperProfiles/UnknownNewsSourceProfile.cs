using AutoMapper;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.MapperProfiles
{
    public class UnknownNewsSourceProfile : Profile
    {
        public UnknownNewsSourceProfile()
        {
            CreateMap<UnknownNewsSource, UnknownNewsSourceResponse>()
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.Uri, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest =>
                    dest.Classification, opt => opt.MapFrom((src, subDest, dest, ctx) => ctx.Mapper.Map<NewsSourceAuthenticity, NewsSourceClassification>(src.Authenticity)));
        }
    }
}
