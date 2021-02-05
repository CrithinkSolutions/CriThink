using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class RemoveNewsSourceCommand : IRequest
    {
        public RemoveNewsSourceCommand(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}
