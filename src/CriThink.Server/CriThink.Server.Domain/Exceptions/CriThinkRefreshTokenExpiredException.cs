namespace CriThink.Server.Domain.Exceptions
{
    public class CriThinkRefreshTokenExpiredException : CriThinkSecurityException
    {
        public CriThinkRefreshTokenExpiredException()
            : base(string.Empty)
        {
        }
    }
}
