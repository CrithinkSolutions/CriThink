using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CriThink.Common.Helpers
{
    public static class StringExtensions
    {
        private const string WhitespacePattern = @"\s+";

        public static IEnumerable<ReadOnlyMemory<char>> SplitInParts(this string s, int length)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            if (length <= 0)
                throw new ArgumentNullException(nameof(length));

            for (var i = 0; i < s.Length; i += length)
            {
                yield return s.AsMemory().Slice(i, Math.Min(length, s.Length - i));
            }
        }

        public static string FormatMe(this string value, params object[] args) =>
            string.Format(value, args);

        public static bool IsUrl(this string value)
        {
            return Uri.IsWellFormedUriString(value, UriKind.Absolute);
        }

        public static string Coalesce(this string value, params string[] args)
        {
            return value ?? args.First(x => x != null);
        }

        public static string RemoveWhitespaces(this string source) =>
            Regex.Replace(source, WhitespacePattern, "");
    }
}
