using CriThink.Common.Endpoints;

namespace CriThink.Client.Droid.Constants
{
    public class DeepLinkConstants
    {
        public const string SchemaHTTP = "http";

        public const string SchemaHTTPS = "https";

        public const string SchemaHost = "crithink.com";

        public const string SchemaPrefixResetPassword = EndpointConstants.ApiBase + EndpointConstants.IdentityBase + EndpointConstants.IdentityResetPassword;

        public const string SchemaPrefixEmailConfirmation = EndpointConstants.ApiBase + EndpointConstants.IdentityBase + EndpointConstants.IdentityConfirmEmail;
    }
}