namespace CriThink.Common.Endpoints
{
    public sealed class EndpointConstants
    {
        public const string ApiBase = "api/";

        #region Versioning

        public const string ApiVersionHeader = "api-version";
        public const string VersionOne = "1.0";

        #endregion

        #region Service

        public const string ServiceBase = "service/";
        public const string ServiceEnvironment = "environment";
        public const string ServiceRedisHealth = "redis-health";

        #endregion

        #region Identity

        public const string IdentityBase = "identity/";

        public const string SignUp = "sign-up";
        public const string Login = "login";
        public const string ConfirmEmail = "confirm-email";
        public const string ChangePassword = "change-password";

        #endregion

        #region NewsAnalyzer

        public const string NewsAnalyzerBase = "news-analyzer/";

        public const string HttpsSupport = "https-support";
        public const string DomainLookup = "domain-lookup";
        public const string CompleteAnalysis = "complete-analysis";

        #endregion

        #region NewsSource

        public const string NewsSourceBase = "news-source/";

        public const string RemoveBadNewsSource = "bad";
        public const string RemoveGoodNewsSource = "good";

        #endregion
    }
}
