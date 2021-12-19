using System;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.Models.NewsChecker
{
    public class RecentNewsChecksModel : MvxNotifyPropertyChanged
    {
        public RecentNewsChecksModel(
            Guid id,
            string searchedText,
            string title,
            string favIcon,
            DateTime timeStamp)
        {
            Id = id;
            SearchedText = searchedText;
            Title = title;
            FavIcon = favIcon ?? "res:ic_blog_logo";
            TimeStamp = timeStamp;
        }

        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _newsLink;
        public string SearchedText
        {
            get => _newsLink;
            set => SetProperty(ref _newsLink, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _favIcon;
        public string FavIcon
        {
            get => _favIcon;
            set => SetProperty(ref _favIcon, value);
        }

        private DateTime _timeStamp;
        public DateTime TimeStamp
        {
            get => _timeStamp;
            set => SetProperty(ref _timeStamp, value);
        }
    }
}
