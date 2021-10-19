using System;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.Models.NewsChecker
{
    public class NewsCheckerResultModel : MvxNotifyPropertyChanged
    {

        private string _newsLink;
        public string NewsLink
        {
            get => _newsLink;
            set => SetProperty(ref _newsLink, value);
        }
        private bool _isUnknownResult;
        public bool IsUnknownResult
        {
            get => _isUnknownResult;
            set => SetProperty(ref _isUnknownResult, value);
        }

        private NewsSourcePostAnswersResponse _newsSourcePostAnswersResponse;
        public NewsSourcePostAnswersResponse NewsSourcePostAnswersResponse
        {
            get => _newsSourcePostAnswersResponse;
            set => SetProperty(ref _newsSourcePostAnswersResponse, value);
        }

        public static NewsCheckerResultModel IsErrorResultModel(string newsLink) =>
            new NewsCheckerResultModel
            {
                NewsLink = newsLink,
                IsUnknownResult = true
            };

        public static NewsCheckerResultModel Create(string newsLink, NewsSourcePostAnswersResponse newsSourcePostAnswersResponse)
            => new NewsCheckerResultModel
            {
                NewsLink = newsLink,
                NewsSourcePostAnswersResponse = newsSourcePostAnswersResponse
            };
    }
}
