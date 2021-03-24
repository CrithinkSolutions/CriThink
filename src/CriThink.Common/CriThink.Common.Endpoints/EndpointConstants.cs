namespace CriThink.Common.Endpoints
{
    public sealed class EndpointConstants
    {
        public const string ApiBase = "api/";
        public const string Mobile = "mobile/";

        #region Versioning

        public const string ApiVersionHeader = "api-version";
        public const string VersionOne = "1.0";

        #endregion

        #region Authentication

        public const string AuthorizationHeader = "Authorization";
        public const string BearerPrefix = "Bearer ";

        #endregion

        #region Service

        public const string ServiceBase = "service/";
        public const string ServiceEnvironment = "environment";
        public const string ServiceLoggingHealth = "logging-health";

        #endregion

        #region Health Check

        public const string HealthCheckBase = "health/";
        public const string HealthCheckRedis = "redis";
        public const string HealthCheckPostgreSql = "postgresql";
        public const string HealthCheckDbContext = "dbcontext";

        #endregion

        #region Identity

        public const string IdentityBase = "identity/";

        public const string IdentitySignUp = "sign-up";
        public const string IdentityLogin = "login";
        public const string IdentityConfirmEmail = "confirm-email";
        public const string IdentityChangePassword = "change-password";
        public const string IdentityForgotPassword = "forgot-password";
        public const string IdentityResetPassword = "reset-password";
        public const string IdentityExternalLogin = "external-login";
        public const string IdentityUsernameAvailability = "username-availability";

        #endregion

        #region NewsSource

        public const string NewsSourceBase = "news-source/";

        public const string NewsSourceRemoveNewsSource = "remove";
        public const string NewsSourceSearch = "search";
        public const string NewsSourceRegisterForNotification = "register-for-notification";
        public const string NewsSourcesGetAllNotifications = "notification-requests";
        public const string NewsSourcesGetAllUnknownNewsSources = "get-all-unknown";
        public const string NewsSourceTriggerIdentifiedSource = "identify";

        #endregion

        #region DebunkNews

        public const string DebunkNewsBase = "debunking-news/";

        public const string DebunkNewsTriggerUpdate = "trigger-update";
        public const string DebunkingNewsGetAll = "all";
        public const string DebunkingNewsAddNews = "add-news";
        public const string DebunkingNewsRemoveNews = "remove-news";
        public const string DebunkingNewsInfoNews = "info-news";

        #endregion

        #region TriggerLog

        public const string TriggerLogBase = "trigger-log/";

        #endregion

        #region UserManagement

        public const string UserManagementBase = "user-management/";
        public const string UserManagementRoles = "roles";
        public const string UserManagementAddUser = "add-user";
        public const string UserManagementAddAdmin = "add-admin";
        public const string UserManagementRemoveUser = "remove-admin";
        public const string UserManagementSoftRemoveUser = "softremove-admin";
        public const string UserManagementInfoUser = "info-user";
        public const string UserManagementEditUser = "edit-user";
        public const string UserManagementEditRoleUser = "edit-roleuser";

        #endregion

        #region MVC

        public const string MvcAdd = "add";
        public const string MvcEdit = "edit";
        public const string MvcForgotPassword = "forgot-password";

        #endregion
    }
}
