using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;
using Gasconade.UI.Internal;

namespace Gasconade.UI
{
    /// <summary>
    /// Register Gasconade against a WebApi service
    /// </summary>
    public static class GasconadeConfig
    {
        private static readonly List<Type> _discoveredTypes = new List<Type>();
        internal static string StylesheetText;
        internal static string ReturnLink;

        /// <summary>
        /// Register against a WebApi service
        /// </summary>
        public static void Register(HttpConfiguration httpConfig){

            httpConfig.Routes.MapHttpRoute("GasconadeConfig", "gasconade/ui/{*assetPath}", null, new { assetPath = ".+" }, new GasconadeUiHandler());

            httpConfig.Routes.MapHttpRoute("GasconadeConfig_shortcut", "gasconade", null, new {
                uriResolution = new HttpRouteDirectionConstraint(HttpRouteDirection.UriResolution)
            }, new RedirectHandler(DefaultRootUrlResolver, "gasconade/ui/index"));
        }

        /// <summary>
        /// Provide raw stylesheet data to be included in the page
        /// </summary>
        public static void AddStylesheet(string stylesheet) {
            StylesheetText = stylesheet;
        }

        /// <summary>
        /// Build a link to the gasconade docs, with given text.
        /// This is designed to work from Swagger UI
        /// </summary>
        public static string Link(string text)
        {
            return "<a href=\"../../gasconade\" target=\"_this\">"+text+"</a>";
        }

        /// <summary>
        /// Include a link back to Swagger in Gasconade
        /// </summary>
        public static void AddSwaggerLink(string link = null)
        {
            ReturnLink = link ?? "../../swagger";
        }

        /// <summary>
        /// Remove all discovered log message types
        /// </summary>
        public static void ClearTypes()
        {
            _discoveredTypes.Clear();
        }

        /// <summary>
        /// Add another assembly to be scanned for log messages
        /// </summary>
        public static void AddAssembly(Assembly assm) {
            _discoveredTypes.AddRange(GetTemplateTypes(assm));
        }

        /// <summary>
        /// Return all log message types discovered so far
        /// </summary>
        public static IEnumerable<Type> KnownLogTypes() {
            return _discoveredTypes.Select(id=>id);
        }

        private static string GetHeaderValue(HttpRequestMessage request, string headerName)
        {
            return !request.Headers.TryGetValues(headerName, out var values) ? null : values.FirstOrDefault();
        }

        private static string DefaultRootUrlResolver(HttpRequestMessage request)
        {
            return new UriBuilder(
                    GetHeaderValue(request, "X-Forwarded-Proto") ?? request.RequestUri.Scheme,
                    GetHeaderValue(request, "X-Forwarded-Host") ?? request.RequestUri.Host,
                    int.Parse(GetHeaderValue(request, "X-Forwarded-Port") ?? request.RequestUri.Port.ToString(CultureInfo.InvariantCulture)),
                    (GetHeaderValue(request, "X-Forwarded-Prefix") ?? string.Empty) + request.GetConfiguration().VirtualPathRoot)
                .Uri
                .AbsoluteUri
                .TrimEnd('/');
        }

        private static List<Type> GetTemplateTypes(Assembly assm)
        {
            return assm.GetTypes().Where(IsLogTemplate).ToList();
        }
        
        private static bool IsLogTemplate(Type arg)
        {
            return arg.CustomAttributes.Any(a => a.AttributeType == typeof(LogMessageTemplateAttribute));
        }
    }
}