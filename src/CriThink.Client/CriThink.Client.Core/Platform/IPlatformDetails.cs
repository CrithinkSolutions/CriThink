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
        public void OpenFacebook(string pageId = null);

        /// <summary>
        /// Open Instagram on the optional desired profile
        /// </summary>
        /// <param name="profileId">(Optional) Id of the profile to open</param>
        public void OpenInstagramProfile(string profileId = null);

        /// <summary>
        /// Open Twitter on the optional desired profile
        /// </summary>
        /// <param name="profileId">(Optional) Id of the profile to open</param>
        public void OpenTwitterProfile(string profileId = null);

        /// <summary>
        /// Open LinkedIn on the optional desired profile
        /// </summary>
        /// <param name="profileId">(Optional) Id of the profile to open</param>
        public void OpenLinkedInProfile(string profileId = null);

        /// <summary>
        /// Perform logout from social
        /// </summary>
        public Task LogoutSocialLoginAsync();

        /// <summary>
        /// Silently refresh Google token login
        /// </summary>
        /// <returns>The token</returns>
        Task<string> RefreshGoogleToken();

        /// <summary>
        /// Silently refresh the Facebook token login
        /// </summary>
        /// <returns>The token</returns>
        string RefreshFacebookToken();
    }
}
