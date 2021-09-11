using AutoMapper;
using CriThink.Server.Application.Administration.ViewModels;
using CriThink.Server.Domain.QueryResults;

namespace CriThink.Server.Application.Automapper
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<GetAllRolesQueryResult, RoleGetViewModel>()
                .ForMember(dest => dest.Id, opt =>
                    opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt =>
                    opt.MapFrom(src => src.Name));
        }
    }
}
