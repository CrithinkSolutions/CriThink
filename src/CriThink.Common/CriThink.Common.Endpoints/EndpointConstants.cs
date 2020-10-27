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
        public const string ServiceEnableSignup = "signup-enabled";
        public const string DbContextStatus = "dbcontext-status";

        #endregion

        #region Identity

        public const string IdentityBase = "identity/";

        public const string IdentitySignUp = "sign-up";
        public const string IdentityLogin = "login";
        public const string IdentityConfirmEmail = "confirm-email";
        public const string IdentityChangePassword = "change-password";
        public const string IdentityForgotPassword = "forgot-password";
        public const string IdentityResetPassword = "reset-password";

        #endregion

        #region Admin

        public const string AdminBase = "admin/";

        public const string AdminSignUp = "sign-up";
        public const string AdminRole = "role";

        public const string AdminUserRole = "user/role";
        public const string AdminUserGetAll = "user/all";
        public const string AdminUser = "user";

        #endregion

        #region NewsAnalyzer

        public const string NewsAnalyzerBase = "news-analyzer/";

        public const string NewsAnalyzerHttpsSupport = "https-support";
        public const string NewsAnalyzerDomainLookup = "domain-lookup";
        public const string NewsAnalyzerPerformCompleteAnalysis = "perform-complete-anlysis";
        public const string NewsAnalyzerScrapeNews = "scrape-news";
        public const string NewsAnalyzerTextSentimentAnalysis = "sentiment-analysis";
        public const string NewsAnalyzerQuestionAdd = "question";
        public const string NewsAnalyzerQuestionGetAll = "question";
        public const string NewsAnalyzerQuestionAnswer = "answer-question";
        public const string NewsAnalyzerQuestionAnswerAdd = "answer-question/create";

        #endregion

        #region NewsSource

        public const string NewsSourceBase = "news-source/";

        public const string NewsSourceRemoveBlackNewsSource = "blacklist";
        public const string NewsSourceRemoveWhiteNewsSource = "whitelist";
        public const string NewsSourceNewsSourceGetAll = "all";

        #endregion

        #region DebunkNews

        public const string DebunkNewsBase = "debunking-news/";

        public const string DebunkNewsTriggerUpdate = "trigger-update";

        #endregion

        #region Demo

        public const string DemoBase = "demo/";

        public const string DemoNewsGetAll = "demo-news";
        public const string DemoNewsAdd = "demo-news";

        #endregion
    }
}
