using System;
using System.Collections.Generic;
using System.Linq;

namespace Zek.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string StringJoin<T>(this IEnumerable<T> items, Func<T, string> selector, string separator = ", ")
        {
            return string.Join(separator, items.Select(selector));
        }
    }
}