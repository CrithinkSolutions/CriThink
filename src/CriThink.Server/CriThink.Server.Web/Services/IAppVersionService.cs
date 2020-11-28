namespace CriThink.Server.Web.Services
{
    public interface IAppVersionService
    {
        /// <summary>
        /// Get the application version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Get application copyright
        /// </summary>
        string Copyright { get; }

        /// <summary>
        /// Get the name of the current environment
        /// </summary>
        string CurrentEnvironment { get; }
    }
}
