using MvvmCross.ViewModels;

namespace CriThink.Client.Core.Models.Menu
{
    public class BaseMenuItem : MvxNotifyPropertyChanged
    {
        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
    }
}