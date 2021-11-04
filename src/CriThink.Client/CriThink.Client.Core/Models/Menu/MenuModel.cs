using MvvmCross.Commands;

namespace CriThink.Client.Core.Models.Menu
{
    public class MenuModel : BaseMenuItem
    {
        public MenuModel(string text, string iconPath)
        {
            Text = text;
            IconPath = iconPath;
        }

        public MenuModel(
            string text,
            string iconPath,
            IMvxAsyncCommand command)
            : this(text, iconPath)
        {
            Command = command;
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
