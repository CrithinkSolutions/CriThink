using System;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Common
{
    public class DateTimeViewModel : MvxNotifyPropertyChanged
    {
        public DateTimeViewModel(DateTime? dateTime)
        {
            DateTime = dateTime;
        }

        private DateTime? _dateTime;

        public DateTime? DateTime
        {
            get => _dateTime;
            set => SetProperty(ref _dateTime, value);
        }

        public override string ToString()
        {
            return _dateTime.HasValue ?
                _dateTime.Value.ToString("D") :
                string.Empty;
        }
    }
}