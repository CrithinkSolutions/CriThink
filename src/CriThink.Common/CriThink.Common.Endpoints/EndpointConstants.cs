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
        public const string ServiceSqlServerHealth = "sqlserver-health";
        public const string ServiceLoggingHealth = "logging-health";

        #endregion

        #region Identity

        public const string IdentityBase = "identity/";

        public const string IdentitySignUp = "sign-up";
        public const string IdentityLogin = "login";
        public const string IdentityConfirmEmail = "confirm-email";
        public const string IdentityChangePassword = "change-password";

        #endregion

        #region NewsAnalyzer

        public const string NewsAnalyzerBase = "news-analyzer/";

        public const string NewsAnalyzerHttpsSupport = "https-support";
        public const string NewsAnalyzerDomainLookup = "domain-lookup";
        public const string NewsAnalyzerPerformCompleteAnalysis = "perform-complete-anlysis";
        public const string NewsAnalyzerScrapeNews = "scrape-news";
        public const string NewsAnalyzerTextSentimentAnalysis = "sentiment-analysis";

        #endregion

        #region NewsSource

        public const string NewsSourceBase = "news-source/";

        public const string NewsSourceRemoveBlackNewsSource = "blacklist";
        public const string NewsSourceRemoveWhiteNewsSource = "whitelist";
        public const string NewsSourceNewsSourceGetAll = "all";

        #endregion
    }
}
