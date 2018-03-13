using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gasconade.UI.Internal
{
    /// <summary>
    /// Message handler that sends redirection messages
    /// </summary>
    public class RedirectHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, string> _rootUrlResolver;
        private readonly string _redirectPath;

        /// <summary>
        /// Create a redirection handler
        /// </summary>
        /// <param name="rootUrlResolver">Function used to determine the root of the siet</param>
        /// <param name="redirectPath">Sub-path from root that is the redirect target</param>
        public RedirectHandler(Func<HttpRequestMessage, string> rootUrlResolver, string redirectPath)
        {
            _rootUrlResolver = rootUrlResolver;
            _redirectPath = redirectPath;
        }

        /// <summary>
        /// Handle a HTTP request
        /// </summary>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uriString = _rootUrlResolver(request) + "/" + _redirectPath;
            var response = request.CreateResponse(HttpStatusCode.MovedPermanently);
            response.Headers.Location = new Uri(uriString);
            var completionSource = new TaskCompletionSource<HttpResponseMessage>();
            completionSource.SetResult(response);
            return completionSource.Task;
        }
    }
}