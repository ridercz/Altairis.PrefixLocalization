using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Altairis.PrefixLocalization.Routing {
    public class PrefixLocalizationMiddleware {
        private readonly RequestDelegate nextMiddleware;
        private readonly PrefixLocalizationOptions options;

        public PrefixLocalizationMiddleware(RequestDelegate next, IOptions<PrefixLocalizationOptions> options) {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.nextMiddleware = next ?? throw new ArgumentNullException(nameof(next));

            this.options.PopulateWellKnownPaths();
        }

        public Task Invoke(HttpContext context) {
            // Get locale name from path
            var path = context.Request.Path.Value;

            // Check if it's ignored
            if (!this.CheckIfPathIsIgnored(path)) {
                if (path == "/") {
                    // Homepage - redirect to fallback locale prefix
                    var prefix = this.GetFallbackLocalePrefix(context);
                    context.Response.Redirect($"/{prefix}");
                    return Task.CompletedTask;
                }

                var currentLocaleMapping = this.GetLocaleMappingFromPath(path);
                if (currentLocaleMapping == null) {
                    // No locale specified - redirect to fallback one
                    var prefix = this.GetFallbackLocalePrefix(context);
                    context.Response.Redirect($"/{prefix}{path}");
                    return Task.CompletedTask;
                } else {
                    // Set the culture
                    CultureInfo.CurrentCulture = currentLocaleMapping.Culture;
                    CultureInfo.CurrentUICulture = currentLocaleMapping.UiCulture;

                    // Set the locale name
                    context.Features.Set(new PrefixLocalizationInfo(this.options.LocaleMappings, currentLocaleMapping.Prefix));

                    // Set cookie
                    if (this.options.UseCookie && !currentLocaleMapping.Prefix.Equals(context.Request.Cookies[this.options.CookieName])) {
                        var co = new CookieOptions {
                            MaxAge = this.options.CookieMaxAge,
                            HttpOnly = true,
                            IsEssential = false
                        };
                        context.Response.Cookies.Append(this.options.CookieName, currentLocaleMapping.Prefix, co);
                    }
                }
            }

            // Pass to next middleware
            return this.nextMiddleware(context);
        }

        private string GetFallbackLocalePrefix(HttpContext context) {
            // Use static default locale if configured
            if (this.options.LocaleMappings.Any(x => x.Prefix.Equals(this.options.DefaultLocale, StringComparison.OrdinalIgnoreCase))) return this.options.DefaultLocale;

            // Use cookie
            if (this.options.UseCookie) {
                var lastKnownLocale = context.Request.Cookies[this.options.CookieName];
                if (!string.IsNullOrEmpty(lastKnownLocale) && this.options.LocaleMappings.Any(x => x.Prefix.Equals(lastKnownLocale, StringComparison.OrdinalIgnoreCase))) return lastKnownLocale;
            }

            // Use Accept-Language header
            if (this.options.UseAcceptLanguageHeader) {
                // Get Accept-Language header
                var acceptLanguageHeader = context.Request.GetTypedHeaders().AcceptLanguage;
                if (acceptLanguageHeader == null || acceptLanguageHeader.Count == 0) return this.options.LocaleMappings.First().Prefix;

                // Get sorted list of languages
                var languages = acceptLanguageHeader.AsEnumerable();
                if (this.options.MaximumAcceptLanguageHeaderValuesToTry > 0) languages = languages.Take(this.options.MaximumAcceptLanguageHeaderValuesToTry);
                var orderedLanguages = languages.OrderByDescending(h => h, StringWithQualityHeaderValueComparer.QualityComparer).Select(x => x.Value.Value).ToList();

                // Find locale based on its UiCulture
                CultureInfo ci;
                foreach (var language in orderedLanguages) {
                    try {
                        ci = new CultureInfo(language);
                    } catch (CultureNotFoundException) {
                        continue;
                    }
                    var lm = this.options.LocaleMappings.FirstOrDefault(x => x.UiCulture.Name.Equals(ci.Name, StringComparison.OrdinalIgnoreCase));
                    if (lm == null) lm = this.options.LocaleMappings.FirstOrDefault(x => x.UiCulture.TwoLetterISOLanguageName.Equals(ci.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase));
                    if (lm != null) return lm.Prefix;
                }
            }

            // Use first defined locale as last resort
            return this.options.LocaleMappings.First().Prefix;
        }

        private PrefixToCultureMapping GetLocaleMappingFromPath(string path) {
            if (string.IsNullOrWhiteSpace(path) || path.Equals("/")) return null;

            foreach (var item in this.options.LocaleMappings) {
                if (path.Equals($"/{item.Prefix}", StringComparison.OrdinalIgnoreCase)) return item;
                if (path.StartsWith($"/{item.Prefix}/", StringComparison.OrdinalIgnoreCase)) return item;
            }
            return null;
        }

        private bool CheckIfPathIsIgnored(string path) {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(path));

            return this.options.IgnorePaths.Any(p => Regex.IsMatch(path, p, RegexOptions.IgnoreCase));
        }

    }
}
