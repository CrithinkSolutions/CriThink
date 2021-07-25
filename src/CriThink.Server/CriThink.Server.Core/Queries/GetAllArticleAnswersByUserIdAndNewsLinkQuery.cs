using System.Collections.Generic;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllArticleAnswersByUserIdAndNewsLinkQuery : IRequest<IList<ArticleAnswer>>
    {
        public GetAllArticleAnswersByUserIdAndNewsLinkQuery(string userId, string newsLink)
        {
            UserId = userId;
            NewsLink = newsLink;
        }

        public string UserId { get; }

        public string NewsLink { get; }
    }
}
