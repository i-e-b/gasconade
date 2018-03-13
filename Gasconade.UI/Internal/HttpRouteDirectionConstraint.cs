using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Gasconade.UI.Internal
{
    /// <summary>
    /// Http redirect helper. Used to direct from the simple 'gasconade' endpoint to the full resource path
    /// </summary>
    public class HttpRouteDirectionConstraint : IHttpRouteConstraint
    {
        private readonly HttpRouteDirection _allowedDirection;

        /// <summary>
        /// Create a directional constraint
        /// </summary>
        /// <param name="allowedDirection"></param>
        public HttpRouteDirectionConstraint(HttpRouteDirection allowedDirection)
        {
            _allowedDirection = allowedDirection;
        }

        /// <summary>
        /// Check if the constraint matches the parameters
        /// </summary>
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return routeDirection == _allowedDirection;
        }
    }
}