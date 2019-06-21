using Ancorazor.API.Common.Constants;
using Microsoft.Extensions.Options;
using Siegrain.Common;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Ancorazor.API.Common
{
    public static class UrlHelper
    {
        public static string ToUrlSafeString(string source, bool convertToPinyin = true)
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

        public static string GetArticleRoutePath(int id, DateTime date, string alias, string category, string template)
        {
            category = category ?? Constants.Constants.Article.DefaultCategoryName;
            var path = template
                .Replace(nameof(id), id.ToString())
                .Replace(nameof(date), date.ToString("yyyy/MM/dd"))
                .Replace(nameof(category), CHNToPinyin.ConvertToPinYin(category))
                .Replace(nameof(alias), alias)
                .ToLowerInvariant();
            return path;
        }
    }
}
