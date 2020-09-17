using System;
using System.Collections.Generic;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateDebunkingNewsCommand : IRequest
    {
        public CreateDebunkingNewsCommand(IEnumerable<DebunkedNews> debunkedNewsCollection)
        {
            if (debunkedNewsCollection == null)
                throw new ArgumentNullException(nameof(debunkedNewsCollection));

            DebunkedNewsCollection = new List<DebunkedNews>(debunkedNewsCollection).AsReadOnly();
        }

        public IReadOnlyList<DebunkedNews> DebunkedNewsCollection { get; }
    }
}
