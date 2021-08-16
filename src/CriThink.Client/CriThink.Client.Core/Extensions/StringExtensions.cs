using System;
using System.Text.RegularExpressions;

namespace CriThink.Client.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatMe(this string value, params object[] args)
            => string.Format(value, args);

        public static bool IsUrl(this string value)
        {
            return Uri.IsWellFormedUriString(value, UriKind.Absolute);

        }

        public static bool IsEmail(this string value)
        {
            var pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(value);
        }
    }

}
