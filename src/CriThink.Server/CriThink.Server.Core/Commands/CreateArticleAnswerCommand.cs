using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class CreateArticleAnswerCommand : IRequest
    {
        public CreateArticleAnswerCommand(ArticleAnswer answer)
        {
            Answer = answer;
        }

        public ArticleAnswer Answer { get; }
    }
}
