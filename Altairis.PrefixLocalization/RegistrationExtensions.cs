using System;
using Altairis.PrefixLocalization.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.PrefixLocalization {
    public static class RegistrationExtensions {

        // Service registration

        public static void AddPrefixLocalization(this IServiceCollection services, Action<PrefixLocalizationOptions> setupAction) {
            services.Configure(setupAction);
            services.Configure<RazorPagesOptions>(options => { options.Conventions.Add(new PrefixLocalizationConvention()); });
            services.Configure<RouteOptions>(options => { options.ConstraintMap.Add(PrefixLocalizationOptions.LocaleRouteConstraintKey, typeof(SupportedLocaleConstraint)); });
        }

        // Middleware registration

        public static void UsePrefixLocalization(this IApplicationBuilder app) {
            app.UseMiddleware<PrefixLocalizationMiddleware>();
        }

    }
}
