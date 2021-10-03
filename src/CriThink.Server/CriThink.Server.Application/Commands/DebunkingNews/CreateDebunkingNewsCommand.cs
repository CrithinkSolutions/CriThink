using System;
using System.Collections.Generic;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class CreateDebunkingNewsCommand : IRequest
    {
        public CreateDebunkingNewsCommand(
            string caption,
            string link,
            IReadOnlyCollection<string> keywords,
            string title,
            string imageLink,
            Guid publisherId)
        {
            Caption = caption;
            Link = link;
            Keywords = keywords;
            Title = title;
            ImageLink = imageLink;
            PublisherId = publisherId;
        }

        public string Caption { get; }

        public string Link { get; }

        public IReadOnlyCollection<string> Keywords { get; }

        public string Title { get; }

        public string ImageLink { get; }

        public Guid PublisherId { get; }
    }
}
