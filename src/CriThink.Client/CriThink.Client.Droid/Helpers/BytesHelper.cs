using System.IO;

namespace CriThink.Client.Droid.Helpers
{
    public static class BytesHelper
    {
        public static byte[] ConvertByteArray(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using var ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                ms.Write(buffer, 0, read);
            return ms.ToArray();
        }
    }
}