using System;
using System.Collections.Generic;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateDebunkingNewsCommand : IRequest
    {
        public CreateDebunkingNewsCommand(IEnumerable<DebunkingNews> debunkedNewsCollection)
        {
            if (debunkedNewsCollection == null)
                throw new ArgumentNullException(nameof(debunkedNewsCollection));

            DebunkedNewsCollection = new List<DebunkingNews>(debunkedNewsCollection).AsReadOnly();
        }

        public IReadOnlyList<DebunkingNews> DebunkedNewsCollection { get; }
    }
}
