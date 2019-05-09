using Siegrain.Common;
using System.Net;
using System.Text.RegularExpressions;

namespace Blog.API.Common
{
    public static class UrlHelper
    {
        public static string ToUrlSafeString(string source, bool convertToPinyin)
        {
            var str = convertToPinyin ? CHNToPinyin.ConvertToPinYin(source) : source;
            return Regex.Replace(str, Constants.Constants.Article.RouteReplaceRegex, " ")
                .Trim()
                .Replace(" ", "-")
                .ToLowerInvariant();
        }

        public static string UrlStringEncode(string source)
        {
            return WebUtility.HtmlEncode(source.Replace(" ", "-"));
        }
    }
}
