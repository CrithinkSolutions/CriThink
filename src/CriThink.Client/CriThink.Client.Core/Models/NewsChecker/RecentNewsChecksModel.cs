using System;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.Models.NewsChecker
{
    public class RecentNewsChecksModel : MvxNotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _newsLink;
        public string NewsLink
        {
            get => _newsLink;
            set => SetProperty(ref _newsLink, value);
        }

        private string _classification;

        public string Classification
        {
            get => _classification;
            set => SetProperty(ref _classification, value);
        }

        private DateTime _searchDateTime;

        public DateTime SearchDateTime
        {
            get => _searchDateTime;
            set => SetProperty(ref _searchDateTime, value);
        }

        private string _newsImageLink;

        public string NewsImageLink
        {
            get => _newsImageLink;
            set => SetProperty(ref _newsImageLink, value);
        }
    }
}
