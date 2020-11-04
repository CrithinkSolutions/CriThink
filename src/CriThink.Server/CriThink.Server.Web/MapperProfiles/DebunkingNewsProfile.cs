using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
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
        }
    }
}
