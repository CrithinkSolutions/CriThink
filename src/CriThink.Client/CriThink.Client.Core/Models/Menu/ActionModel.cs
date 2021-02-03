using MvvmCross.Commands;

namespace CriThink.Client.Core.Models.Menu
{
    public class ActionModel : BaseMenuItem
    {
        public ActionModel() { }

        public ActionModel(string text)
        {
            Text = text;
        }

        public IMvxAsyncCommand Command { get; set; }
    }
}