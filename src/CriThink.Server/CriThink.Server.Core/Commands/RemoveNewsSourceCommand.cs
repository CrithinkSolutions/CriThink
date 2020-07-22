using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public abstract class RemoveNewsSourceCommand : IRequest
    {
        protected RemoveNewsSourceCommand(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }

    public class RemoveBadNewsSourceCommand : RemoveNewsSourceCommand
    {
        public RemoveBadNewsSourceCommand(Uri uri) : base(uri)
        { }
    }

    public class RemoveGoodNewsSourceCommand : RemoveNewsSourceCommand
    {
        public RemoveGoodNewsSourceCommand(Uri uri) : base(uri)
        { }
    }
}
