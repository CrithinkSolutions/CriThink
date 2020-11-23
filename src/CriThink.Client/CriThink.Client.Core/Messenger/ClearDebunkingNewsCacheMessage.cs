using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Messenger
{
    public class ClearDebunkingNewsCacheMessage : MvxMessage
    {
        public ClearDebunkingNewsCacheMessage(object sender)
            : base(sender)
        { }
    }
}
