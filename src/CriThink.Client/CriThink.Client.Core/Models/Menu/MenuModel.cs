using MvvmCross.Commands;

namespace CriThink.Client.Core.Models.Menu
{
    public class MenuModel : BaseMenuItem
    {
        public MenuModel() { }

        public MenuModel(string text)
        {
            Text = text;
        }

        private string _iconPath;
        public string IconPath
        {
            get => _iconPath;
            set => SetProperty(ref _iconPath, value);
        }

        public IMvxAsyncCommand Command { get; set; }
    }
}
