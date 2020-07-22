using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateNewsSourceCommand : IRequest
    {
        public CreateNewsSourceCommand(Uri uri, NewsSourceAuthencity authencity)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            Authencity = authencity;
        }

        public Uri Uri { get; }

        public NewsSourceAuthencity Authencity { get; }

        public bool IsGoodSource => Authencity == NewsSourceAuthencity.Authentic ||
                                    Authencity == NewsSourceAuthencity.Trusted;
    }
}
