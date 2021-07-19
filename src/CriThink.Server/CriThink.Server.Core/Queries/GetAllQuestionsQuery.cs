﻿using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllQuestionsQuery : IRequest<GetAllArticleQuestionsResponse>
    {
        public GetAllQuestionsQuery()
        { }
    }
}
