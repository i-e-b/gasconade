using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Gasconade.UI
{
    /// <summary>
    /// Create assemblies with dynamic resources
    /// </summary>
    public static class GasconadeConfig
    {
        private static string GetHeaderValue(HttpRequestMessage request, string headerName)
        {
            IEnumerable<string> values;
            if (!request.Headers.TryGetValues(headerName, out values))
                return (string) null;
            return values.FirstOrDefault<string>();
        }
        public static string DefaultRootUrlResolver(HttpRequestMessage request)
        {
            return new UriBuilder(GetHeaderValue(request, "X-Forwarded-Proto") ?? request.RequestUri.Scheme, GetHeaderValue(request, "X-Forwarded-Host") ?? request.RequestUri.Host, int.Parse(GetHeaderValue(request, "X-Forwarded-Port") ?? request.RequestUri.Port.ToString((IFormatProvider) CultureInfo.InvariantCulture)), (GetHeaderValue(request, "X-Forwarded-Prefix") ?? string.Empty) + request.GetConfiguration().VirtualPathRoot).Uri.AbsoluteUri.TrimEnd('/');
        }

        /// <summary>
        /// Register against a WebApi service
        /// </summary>
        public static void Register(HttpConfiguration _httpConfig){

            _httpConfig.Routes.MapHttpRoute("GasconadeConfig", "gasconade/ui/{*assetPath}", (object) null, (object) new
            {
                assetPath = ".+"
            }, (HttpMessageHandler) new GasconadeUiHandler());

            _httpConfig.Routes.MapHttpRoute("GasconadeConfig_shortcut", "gasconade", (object) null, (object) new
            {
                uriResolution = new HttpRouteDirectionConstraint(HttpRouteDirection.UriResolution)
            }, (HttpMessageHandler) new RedirectHandler(DefaultRootUrlResolver, "gasconade/ui/index"));
        }

        /// <summary>
        /// Test generation
        /// </summary>
        public static Assembly Gen() {
            var aName = new AssemblyName("DynamicAssemblyExample");
            var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);

            var rr = ab.DefineResource("Test", "Test", "Test");
            rr.AddResource("Test", "Testoutput");
            rr.Close();

            var stream = ab.GetManifestResourceStream("Test");
            Console.WriteLine(stream);

            return ab;
        }
    }
    public class RedirectHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, string> _rootUrlResolver;
        private readonly string _redirectPath;

        public RedirectHandler(Func<HttpRequestMessage, string> rootUrlResolver, string redirectPath)
        {
            this._rootUrlResolver = rootUrlResolver;
            this._redirectPath = redirectPath;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string uriString = this._rootUrlResolver(request) + "/" + this._redirectPath;
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.MovedPermanently);
            response.Headers.Location = new Uri(uriString);
            TaskCompletionSource<HttpResponseMessage> completionSource = new TaskCompletionSource<HttpResponseMessage>();
            completionSource.SetResult(response);
            return completionSource.Task;
        }
    }

    public class HttpRouteDirectionConstraint : IHttpRouteConstraint
    {
        private readonly HttpRouteDirection _allowedDirection;

        public HttpRouteDirectionConstraint(HttpRouteDirection allowedDirection)
        {
            this._allowedDirection = allowedDirection;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return routeDirection == this._allowedDirection;
        }
    }

    public class GasconadeUiHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sampleMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Hello, world") };
            return Task.FromResult(sampleMessage);
        }
    }
}
