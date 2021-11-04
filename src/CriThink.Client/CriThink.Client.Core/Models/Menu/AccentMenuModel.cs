using MvvmCross.Commands;

namespace CriThink.Client.Core.Models.Menu
{
    public class AccentMenuModel : MenuModel
    {
        public AccentMenuModel(
            string text,
            string iconPath,
            IMvxAsyncCommand command)
            : base(
                  text,
                  iconPath,
                  command)
        { }
    }
}
