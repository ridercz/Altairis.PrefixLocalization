using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Altairis.PrefixLocalization;

namespace LocalizationSampleApp {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            // Register Razor Pages
            services.AddRazorPages();

            // Register prefix localization library
            services.AddPrefixLocalization(options => {
                // Define supported locales
                options.LocaleMappings.Add(new PrefixToCultureMapping("cesky", "cs-CZ", "cs"));
                options.LocaleMappings.Add(new PrefixToCultureMapping("english", "en-US", "en"));
                options.LocaleMappings.Add(new PrefixToCultureMapping("deutsch", "de-DE", "de"));
                // Add ignored paths
                options.IgnorePaths.Add("^/content/.+");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            // Show detailed error messages in development environment
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            // Always use prefix localization as a first middleware (with possible exception below)
            app.UsePrefixLocalization();

            // You may use static middleware as a first one, if you want all static files excluded from localization
            app.UseStaticFiles();

            // This is not exactly needed, but it helps to see error codes
            app.UseStatusCodePagesWithReExecute("/english/Errors/{0}");

            // This is common application initialization
            app.UseRouting();
            app.UseEndpoints(builder => {
                builder.MapRazorPages();
            });
        }
    }
}
