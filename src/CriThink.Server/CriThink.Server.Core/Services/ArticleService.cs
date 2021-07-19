using System;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Article;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Services
{
    internal class ArticleService : IArticleService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ArticleService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ArticleGetAllQuestionsResponse> GetQuestionsAsync()
        {
            var query = new GetAllQuestionsQuery();
            var questions = await _mediator.Send(query);

            var response = _mapper.Map<GetAllArticleQuestionsResponse, ArticleGetAllQuestionsResponse>(questions);

            return response;
        }
    }
}
