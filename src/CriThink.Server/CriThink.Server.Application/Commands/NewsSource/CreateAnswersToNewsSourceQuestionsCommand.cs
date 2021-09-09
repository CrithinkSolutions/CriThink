using System;
using System.Collections.Generic;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class CreateAnswersToNewsSourceQuestionsCommand : IRequest<NewsSourcePostAnswersResponse>
    {
        public CreateAnswersToNewsSourceQuestionsCommand(
            Guid userId,
            string email,
            string newsLink,
            IList<NewsSourcePostAnswerRequest> questions)
        {
            UserId = userId;
            Email = email;
            NewsLink = newsLink;
            Questions = questions;
        }

        public Guid UserId { get; }

        public string Email { get; }

        public string NewsLink { get; }

        public IList<NewsSourcePostAnswerRequest> Questions { get; }
    }
}
