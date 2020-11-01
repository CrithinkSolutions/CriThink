using System.Text.RegularExpressions;

namespace CriThink.Common.Helpers
{
    public static class EmailHelper
    {
        private const string EmailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public static bool IsEmail(string content)
        {
            return Regex.IsMatch(content, EmailPattern, RegexOptions.IgnoreCase);
        }
    }
}
