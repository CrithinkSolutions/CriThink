using AutoMapper;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Web.Areas.BackOffice.ViewModels.NewsSource;

namespace CriThink.Server.Web.Automapper
{
    public class NewsSourceViewModelProfile : Profile
    {
        public NewsSourceViewModelProfile()
        {
            CreateMap<GetAllNewsSourceQueryResult, NewsSourceViewModel>()
                .ForMember(dest => dest.Uri, opt =>
                    opt.MapFrom(src => src.NewsLink.ToLowerInvariant()))
                .ForMember(dest => dest.Classification, opt =>
                    opt.MapFrom((src,dest,srcMember, context) => context.Mapper.Map<NewsSourceAuthenticity, Classification>(src.SourceAuthencity)));
        }
    }
}
