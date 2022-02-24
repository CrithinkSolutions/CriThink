using MvvmCross.Plugin.Messenger;

namespace CriThink.Client.Core.Messenger
{
    public class PictureMessage : MvxMessage
    {
        public PictureMessage(
            object sender,
            byte[] bytes,
            string fileName) : base(sender)
        {
            Bytes = bytes;
            FileName = fileName;
        }

        public byte[] Bytes { get; }

        public string FileName { get; }
    }
}
