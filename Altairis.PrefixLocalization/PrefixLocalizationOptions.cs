using System;
using System.Collections.Generic;

namespace Altairis.PrefixLocalization {
    public class PrefixLocalizationOptions {
        internal const string LocaleRouteParameterName = "locale";
        internal const string LocaleRouteConstraintKey = "supported-locale";

        public const int DefaultMaximumAcceptLanguageHeaderValuesToTry = 3;
        public const string DefaultCookieName = "Altairis.PrefixLocalization.Locale";
        public static readonly TimeSpan DefaultCookieMaxAge = TimeSpan.FromDays(365);

        public bool UseAcceptLanguageHeader { get; set; } = true;

        public int MaximumAcceptLanguageHeaderValuesToTry { get; set; } = DefaultMaximumAcceptLanguageHeaderValuesToTry;

        public string DefaultLocale { get; set; }

        public ICollection<PrefixToCultureMapping> LocaleMappings { get; set; } = new HashSet<PrefixToCultureMapping>();

        public bool IgnoreWellKnownPaths { get; set; } = true;

        public ICollection<string> IgnorePaths { get; set; } = new HashSet<string>();

        public bool UseCookie { get; set; } = true;

        public string CookieName { get; set; } = DefaultCookieName;

        public TimeSpan CookieMaxAge { get; set; } = DefaultCookieMaxAge;

        internal void PopulateWellKnownPaths() {
            if (!this.IgnoreWellKnownPaths) return;

            this.IgnorePaths.Add(@"^/\.");                  // Anything beginning with a dot, ie /.well-known
            this.IgnorePaths.Add(@"^/[^/\.]+\.[^/\.]+$");   // Any file with extension in root, ie. /robots.txt
        }

    }
}