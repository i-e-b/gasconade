using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gasconade.UiCore;
using Microsoft.AspNetCore.Http;
using Tag;

namespace Gasconade.UI.Core
{
    /// <summary>
    /// Gasconade UI middleware. This handles just about everything
    /// </summary>
    public class GasconadeUiIndexMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Create new middleware
        /// </summary>
        public GasconadeUiIndexMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        /// <summary>
        /// Handle a request by context
        /// </summary>
        public async Task Invoke(HttpContext httpContext)
        {
            var uiIndexMiddleware = this;
            if (!uiIndexMiddleware.IsRequestingIndex(httpContext.Request))
                await uiIndexMiddleware._next(httpContext);
            else if (!httpContext.Request.Path.Value.EndsWith("/"))
                uiIndexMiddleware.RedirectResponse(httpContext.Response, "/" + GasconadeUi.RoutePrefix + "/");
            else
                await uiIndexMiddleware.RespondWithIndexHtml(httpContext.Response);
        }

        private bool IsRequestingIndex(HttpRequest request)
        {
            return request.Method == "GET" && request.Path.Value.StartsWith("/" + GasconadeUi.RoutePrefix + "/", StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task RespondWithIndexHtml(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html";

            var content = BuildPage();

            await response.WriteAsync(content, Encoding.UTF8);
        }

        private void RedirectResponse(HttpResponse response, string redirectPath)
        {
            response.StatusCode = 301;
            response.Headers["Location"] = redirectPath;
        }
        
        private string BuildPage()
        {
            var all = GasconadeUi.KnownLogTypes().ToList();

            if ( ! all.Any()) {
                return TagHttpPage(PageContentGenerator.EmptyLogsPage());
            }

            return TagHttpPage(PageContentGenerator.ListingDocument(all, GasconadeUi.ReturnLink, GasconadeUi.HeaderHtml));
        }

        private static string TagHttpPage(TagContent body)
        {
            if (body.Tag != "body") body = T.g("body")[body];

            body.Add(T.g("script", "type","text/javascript")[GetScriptData()]);

            var content = T.g("html")[
                T.g("head")[
                    T.g("title")["Log documentation (Gasconade)"],
                    T.g("style")[GetStyleData()]
                ],
                body
            ];

            return content;
        }

        private static string GetScriptData()
        {
            if ( ! string.IsNullOrWhiteSpace(GasconadeUi.ScriptText)) return GasconadeUi.ScriptText;
            return DefaultContent.DefaultJavaScript;
        }

        private static string GetStyleData()
        {
            if ( ! string.IsNullOrWhiteSpace(GasconadeUi.StylesheetText)) return GasconadeUi.StylesheetText;
            return DefaultContent.DefaultStyleSheet;
        }
    }
}