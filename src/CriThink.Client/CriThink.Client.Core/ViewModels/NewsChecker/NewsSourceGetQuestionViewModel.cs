using System;
using System.Collections.Generic;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using System.Linq;
namespace CriThink.Client.Core.ViewModels.NewsChecker
{
    public class NewsSourceGetQuestionViewModel : BaseViewModel
    {
        public NewsSourceGetQuestionViewModel()
        {
            _response = new List<bool>(5)
            {
                false,
                false,
                false,
                false,
                false
            };
        }

        public NewsSourceGetQuestionViewModel(NewsSourceGetQuestionResponse newsSourceGetQuestionResponse) : this()
        {
            if (newsSourceGetQuestionResponse == null)
                throw new ArgumentNullException(nameof(newsSourceGetQuestionResponse));

            Id = newsSourceGetQuestionResponse.Id;
            Question = newsSourceGetQuestionResponse.Text; 
        }

        public int SelectedVote => _response.IndexOf(true) + 1;
        private string _question;
        public string Question
        {
            get => _question;
            set => SetProperty(ref _question, value);
        }
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        private List<bool> _response;
        public List<bool> Response => _response;

        public NewsSourcePostAnswerRequest CreateNewsSourcePostAnswerRequest()
        {
            return new NewsSourcePostAnswerRequest
            {
                QuestionId = Id,
                Rate = SelectedVote
            };
        }

    }
}
