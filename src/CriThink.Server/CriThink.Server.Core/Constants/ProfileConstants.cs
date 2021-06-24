using System.IO;

namespace CriThink.Server.Core.Constants
{
    /// <summary>
    /// Constants regarding the uploads subfolders
    /// </summary>
    public static class ProfileConstants
    {
        public static string ProfileFolder = $"profile{Path.DirectorySeparatorChar}";

        public const string AvatarFileName = "avatar.jpg";
    }
}
