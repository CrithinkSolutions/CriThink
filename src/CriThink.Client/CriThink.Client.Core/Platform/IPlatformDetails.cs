using System.Threading.Tasks;

namespace CriThink.Client.Core.Platform
{
    /// <summary>
    /// Information about the current platform
    /// </summary>
    public interface IPlatformDetails
    {
        /// <summary>
        /// Open Facebook on the optional desired page
        /// </summary>
        /// <param name="pageId">(Optional) Id of the page to open</param>
        void OpenFacebook(string pageId = null);

        /// <summary>
        /// Open Instagram on the optional desired profile
        /// </summary>
        /// <param name="profileId">(Optional) Id of the profile to open</param>
        void OpenInstagramProfile(string profileId = null);

        /// <summary>
        /// Open Twitter on the optional desired profile
        /// </summary>
        /// <param name="profileId">(Optional) Id of the profile to open</param>
        void OpenTwitterProfile(string profileId = null);

        /// <summary>
        /// Open LinkedIn on the optional desired profile
        /// </summary>
        /// <param name="profileId">(Optional) Id of the profile to open</param>
        void OpenLinkedInProfile(string profileId = null);

        /// <summary>
        /// Open Skype app on the desired profile
        /// </summary>
        /// <param name="profileName">Profile name</param>
        void OpenSkypeProfile(string profileName);

        /// <summary>
        /// Perform logout from social
        /// </summary>
        Task LogoutSocialLoginAsync();

        /// <summary>
        /// Silently refresh Google token login
        /// </summary>
        /// <returns>The token</returns>
        Task<string> RefreshGoogleTokenAsync();

        /// <summary>
        /// Silently refresh the Facebook token login
        /// </summary>
        /// <returns>The token</returns>
        string RefreshFacebookToken();
    }
}
