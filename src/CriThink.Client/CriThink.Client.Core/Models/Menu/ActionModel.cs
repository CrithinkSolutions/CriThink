using System;
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

        public ActionModel(string text, IMvxAsyncCommand command)
            : this(text)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public IMvxAsyncCommand Command { get; set; }
    }
}