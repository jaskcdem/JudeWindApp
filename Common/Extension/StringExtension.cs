using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Extension
{
    public static partial class StringExtension
    {
        [GeneratedRegex("&nbsp;")]
        public static partial Regex SpaceRegex();
        [GeneratedRegex("<.*?>")]
        public static partial Regex HtmlTagRegex();

        public static string RemoveSpecialChar(this string value, bool removeHtmlTags = false)
        {
            if (string.IsNullOrEmpty(value)) { return ""; }
            if (removeHtmlTags)
            {
                value = SpaceRegex().Replace(HtmlTagRegex().Replace(value, String.Empty), String.Empty);
            }
            return value.ToString().Replace("&", "&amp;").Replace("'", "&#39;").Replace(">", "&gt;")
                .Replace("<", "&lt;").Replace("\\", "&#92;").Replace("/", "&#47;").Replace("&nbsp;", string.Empty);
        }

    }
}
