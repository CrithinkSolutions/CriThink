using AutoMapper;
using CriThink.Server.Core.Localize;
using Microsoft.Extensions.Localization;

namespace CriThink.Server.Core.MapperProfiles
{
    internal class LocalizationResolver<TSource, TDestination> : IMemberValueResolver<TSource, TDestination, string, string>
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public LocalizationResolver(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(TSource source, TDestination destination, string sourceMember, string destMember, ResolutionContext context)
        {
            var c = _localizer[sourceMember];
            return c;
        }
    }
}