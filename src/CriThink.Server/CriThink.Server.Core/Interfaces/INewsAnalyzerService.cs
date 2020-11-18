using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.NewsAnalyzer;

namespace CriThink.Server.Core.Interfaces
{
    public interface INewsAnalyzerService
    {
        /// <summary>
        /// Returns a status to identify if the provided URI has HTTPS support or not
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A response indicating the analysis result</returns>
        Task<NewsAnalyzerResponse> HasUriHttpsSupportAsync(Uri uri);

        /// <summary>
        /// Analyze the domain of the given <see cref="Uri"/>
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A response indicating the analysis result</returns>
        Task<NewsAnalyzerResponse> AnalyzeDomainAsync(Uri uri);

        /// <summary>
        /// Perform an analysis of the given <see cref="Uri"/> using all the available analyzers
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> to analyze</param>
        /// <returns>A response indicating the analysis result</returns>
        Task<NewsAnalyzerResponse[]> GetCompleteAnalysisAsync(Uri uri);

        /// <summary>
        /// Scrape the given news parsing useful information
        /// </summary>
        /// <param name="uri">News uri</param>
        /// <returns>News details such as author, title, body, etc.</returns>
        Task<ScrapeNewsResponse> ScrapeNewsAsync(Uri uri);

        /// <summary>
        /// Analyze the given news content and gives scores
        /// </summary>
        /// <param name="uri">News uri</param>
        /// <returns>News sentiment scores</returns>
        Task<NewsAnalyzerResponse> AnalyzeNewsSentimentAsync(Uri uri);

        /// <summary>
        /// Get the news list for the demo
        /// </summary>
        /// <returns>List of news</returns>
        Task<IList<DemoNewsResponse>> GetDemoNewsListAsync();

        /// <summary>
        /// Add the given news
        /// </summary>
        /// <param name="request">News to add</param>
        /// <returns>Awaitable task</returns>
        Task AddDemoNewsAsync(DemoNewsAddRequest request);

        /// <summary>
        /// Add the given question
        /// </summary>
        /// <param name="request">Question to add</param>
        /// <returns>Awaitable task</returns>
        Task AddQuestionAsync(QuestionAddRequest request);

        /// <summary>
        /// Get the question list
        /// </summary>
        /// <returns>List of questions</returns>
        Task<IList<QuestionResponse>> GetQuestionListAsync();

        /// <summary>
        /// Compare the given questions with the correct ones
        /// </summary>
        /// <param name="request">Given answers</param>
        /// <returns>Awaitable task with the result</returns>
        Task<IList<QuestionAnswerResponse>> CompareAnswersAsync(AnswerQuestionsRequest request);

        /// <summary>
        /// Add an answer question for a specific news
        /// </summary>
        /// <param name="request">Answer to add</param>
        /// <returns>Awaitable task</returns>
        Task AddAnswerAsync(QuestionAnswerAddRequest request);
    }
}
