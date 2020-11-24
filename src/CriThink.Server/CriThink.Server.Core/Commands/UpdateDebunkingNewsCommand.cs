using System;
using System.Collections.Generic;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class UpdateDebunkingNewsCommand : IRequest
    {
        public UpdateDebunkingNewsCommand(Guid id, string title, string caption, string link, IReadOnlyList<string> keywords)
        {
            Id = id;
            Title = title;
            Caption = caption;
            Link = link;
            Keywords = keywords;
        }

        public Guid Id { get; }

        public string Title { get; }

        public string Caption { get; }

        public string Link { get; }

        public IReadOnlyList<string> Keywords { get; }
    }
}
