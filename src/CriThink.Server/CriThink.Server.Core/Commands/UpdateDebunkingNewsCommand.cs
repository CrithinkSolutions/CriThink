using System;
using System.Collections.Generic;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class UpdateDebunkingNewsCommand : IRequest
    {
        public UpdateDebunkingNewsCommand(Guid id, string title, string caption, string link, string imageLink, IReadOnlyList<string> keywords)
        {
            Id = id;
            Title = title;
            Caption = caption;
            Link = link;
            Keywords = keywords;
            ImageLink = imageLink;
        }

        public Guid Id { get; }

        public string Title { get; }

        public string Caption { get; }

        public string Link { get; }

        public string ImageLink { get; set; }

        public IReadOnlyList<string> Keywords { get; }
    }
}
