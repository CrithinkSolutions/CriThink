using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateNewsSourceCommand : IRequest
    {
        public CreateNewsSourceCommand(Uri uri, NewsSourceAuthenticity authencity)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            Authencity = authencity;
        }

        public Uri Uri { get; }

        public NewsSourceAuthenticity Authencity { get; }
    }
}
