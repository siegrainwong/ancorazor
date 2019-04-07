#region

using Blog.API.Messages;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Blog.API.Filters
{
    /// <summary>
    /// 
    /// https://docs.microsoft.com/zh-cn/aspnet/core/security/anti-request-forgery?view=aspnetcore-2.2
    /// </summary>
    public class GlobalAntiforgerySetFilter : IAuthorizationFilter, IOrderedFilter
    {
        private readonly ILogger<GlobalValidateModelFilter> _logger;
        private readonly IAntiforgery _antiforgery;
        private readonly IQueryable<string> _antiforgeryMethods = (new[] { "POST", "PUT", "DELETE" }).AsQueryable();
        private const string _AntiforgeryHeaderName = "X-XSRF-TOKEN";
        private const string _AntiforgeryCookieName = "XSRF-TOKEN";

        /// <summary>
        /// Filter execution sort
        /// must run before AutoValidateAntiforgeryTokenAttribute (which is 1000 by default)
        /// MARK: 各类型 filter 执行顺序
        /// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.2#filter-types
        /// </summary>
        public int Order => 999;

        public GlobalAntiforgerySetFilter(ILogger<GlobalValidateModelFilter> logger, IAntiforgery antiforgery)
        {
            _logger = logger;
            _antiforgery = antiforgery;
        }

        //public void OnActionExecuting(ActionExecutingContext context)
        //{
        //    var request = context.HttpContext.Request;
        //    if (!IsAntiforgeryRequest(context.HttpContext)) return;
        //    if (!request.Headers.ContainsKey("Authorization")) return;
        //    if (request.Headers.ContainsKey(_AntiforgeryHeaderName)) return;

        //    var tokens = _antiforgery.GetAndStoreTokens(context.HttpContext);
        //    request.Cookies.Append(new KeyValuePair<string, string>(_AntiforgeryCookieName, tokens.RequestToken));
        //    request.Headers.Append(_AntiforgeryHeaderName, tokens.RequestToken);

        //    _logger.LogDebug($" {_AntiforgeryCookieName} written because of first antiforgery request has no X-XSRF-TOKEN: {tokens.RequestToken} with path {request.Path.Value}");
        //}

        //public void OnActionExecuted(ActionExecutedContext context)
        //{
        //    if (!IsAntiforgeryRequest(context.HttpContext)) return;

        //    var httpContext = context.HttpContext;
        //    var tokens = _antiforgery.GetAndStoreTokens(httpContext);
        //    httpContext.Response.Cookies.Append(_AntiforgeryCookieName, tokens.RequestToken,
        //        new CookieOptions() { HttpOnly = false });
        //    _logger.LogDebug($" {_AntiforgeryCookieName} written {tokens.RequestToken} with path: {httpContext.Request.Path.Value}");
        //}

        private bool IsAntiforgeryRequest(HttpContext context)
        {
            return _antiforgeryMethods.Contains(context.Request.Method.ToUpper());
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;
            if (!IsAntiforgeryRequest(context.HttpContext)) return;
            if (!request.Headers.ContainsKey("Authorization")) return;
            if (request.Headers.ContainsKey(_AntiforgeryHeaderName)) return;

            var tokens = _antiforgery.GetAndStoreTokens(context.HttpContext);
            request.Cookies.Append(new KeyValuePair<string, string>(_AntiforgeryCookieName, tokens.RequestToken));
            request.Headers.Append(_AntiforgeryHeaderName, tokens.RequestToken);

            _logger.LogDebug($" {_AntiforgeryCookieName} written because of first antiforgery request has no X-XSRF-TOKEN: {tokens.RequestToken} with path {request.Path.Value}");
        }
    }
}