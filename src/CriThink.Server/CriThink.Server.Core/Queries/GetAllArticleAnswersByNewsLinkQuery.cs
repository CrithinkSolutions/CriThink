using System.Collections.Generic;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllArticleAnswersByNewsLinkQuery : IRequest<IList<ArticleAnswer>>
    {
        public GetAllArticleAnswersByNewsLinkQuery(string newsLink)
        {
            NewsLink = newsLink;
        }

        public string NewsLink { get; }
    }
}
