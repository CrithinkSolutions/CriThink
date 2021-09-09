namespace CriThink.Server.Core.Exceptions
{
    public class CriThinkRefreshTokenExpiredException : CriThinkSecurityException
    {
        public CriThinkRefreshTokenExpiredException()
            : base(string.Empty)
        {
        }
    }
}
