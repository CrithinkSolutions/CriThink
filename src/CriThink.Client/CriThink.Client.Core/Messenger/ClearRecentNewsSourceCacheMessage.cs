using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Messenger
{
    public class ClearRecentNewsSourceCacheMessage : MvxMessage
    {
        public ClearRecentNewsSourceCacheMessage(object sender)
            : base(sender)
        { }
    }
}
