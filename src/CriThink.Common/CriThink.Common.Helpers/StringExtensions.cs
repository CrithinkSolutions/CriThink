using System;
using System.Collections.Generic;

namespace CriThink.Common.Helpers
{
    public static class StringExtensions
    {
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
    }
}
