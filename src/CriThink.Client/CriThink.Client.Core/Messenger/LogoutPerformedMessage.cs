using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Messenger
{
    public class LogoutPerformedMessage : MvxMessage
    {
        public LogoutPerformedMessage(object sender)
            : base(sender)
        { }
    }
}
