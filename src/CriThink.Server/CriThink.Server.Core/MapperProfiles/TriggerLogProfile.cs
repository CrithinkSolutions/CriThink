using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Core.MapperProfiles
{
    public class TriggerLogProfile : Profile
    {
        public TriggerLogProfile()
        {
            CreateMap<GetAllTriggerLogQueryResponse, TriggerLogGetResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.IsSuccessful, opt => opt.MapFrom(src => src.IsSuccessful))
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.FailReason ?? string.Empty));
        }
    }
}
