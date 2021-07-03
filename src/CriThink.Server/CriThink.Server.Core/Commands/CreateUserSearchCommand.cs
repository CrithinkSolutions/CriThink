using System;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateUserSearchCommand : IRequest
    {
        public CreateUserSearchCommand(Guid userId, string newsLink)
            : this(userId, newsLink, NewsSourceAuthenticity.Unknown)
        { }

        public CreateUserSearchCommand(Guid userId, string newsLink, NewsSourceAuthenticity classification)
        {
            UserId = userId;
            NewsLink = newsLink;
            Classification = classification;
        }

        public Guid UserId { get; }

        public string NewsLink { get; }

        public NewsSourceAuthenticity Classification { get; }
    }
}
