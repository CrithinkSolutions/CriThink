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

        public MenuModel(string text, string iconPath)
        {
            Text = text;
            IconPath = iconPath;
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
