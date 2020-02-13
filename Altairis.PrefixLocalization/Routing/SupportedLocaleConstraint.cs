using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Altairis.PrefixLocalization.Routing {
    public class SupportedLocaleConstraint : IRouteConstraint {
        private readonly PrefixLocalizationOptions options;

        public SupportedLocaleConstraint(IOptions<PrefixLocalizationOptions> options) {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection) {
            var value = values[routeKey] as string;
            return this.options.LocaleMappings.Any(m => m.Prefix.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
