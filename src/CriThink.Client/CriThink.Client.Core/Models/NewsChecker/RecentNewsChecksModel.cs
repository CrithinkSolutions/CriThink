using System;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.Models.NewsChecker
{
    public class RecentNewsChecksModel : MvxNotifyPropertyChanged
    {
        public RecentNewsChecksModel(
            Guid id,
            string newsLink)
        {
            Id = id;
            NewsLink = newsLink;
        }

        private Guid _id;
        public Guid Id
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
    }
}
