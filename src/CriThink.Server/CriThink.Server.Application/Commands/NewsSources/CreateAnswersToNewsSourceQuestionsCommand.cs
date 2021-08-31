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
            IList<NewsSourcePostAnswersRequest> questions)
        {
            UserId = userId;
            Email = email;
            NewsLink = newsLink;
            Questions = questions;
        }

        public Guid UserId { get; }

        public string Email { get; }

        public string NewsLink { get; }

        public IList<NewsSourcePostAnswersRequest> Questions { get; }
    }
}
