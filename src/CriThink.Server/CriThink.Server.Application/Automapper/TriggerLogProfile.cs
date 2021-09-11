using AutoMapper;
using CriThink.Server.Application.Administration.ViewModels;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Application.Automapper
{
    public class TriggerLogProfile : Profile
    {
        public TriggerLogProfile()
        {
            CreateMap<GetAllTriggerLogQueryResult, TriggerLogGetViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp.ToString("u")))
                .ForMember(dest => dest.Failures, opt => opt.MapFrom(src => src.FailReason ?? string.Empty));
        }
    }
}
