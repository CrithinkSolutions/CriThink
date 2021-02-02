using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateUnknownNewsSourceCommand : IRequest
    {
        public CreateUnknownNewsSourceCommand(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}
