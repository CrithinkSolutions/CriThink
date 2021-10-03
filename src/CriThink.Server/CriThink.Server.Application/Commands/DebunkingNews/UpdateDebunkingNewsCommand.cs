using System;
using System.Collections.Generic;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class UpdateDebunkingNewsCommand : IRequest
    {
        public UpdateDebunkingNewsCommand(
            Guid id,
            string title,
            string caption,
            string link,
            string imageLink)
        {
            Id = id;
            Title = title;
            Caption = caption;
            Link = link;
            ImageLink = imageLink;
        }

        public Guid Id { get; }

        public string Title { get; }

        public string Caption { get; }

        public string Link { get; }

        public string ImageLink { get; }

        public List<string> Keywords { get; }
    }
}
