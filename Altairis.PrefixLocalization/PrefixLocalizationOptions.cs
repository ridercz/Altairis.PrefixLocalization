using System.Collections.Generic;

namespace Altairis.PrefixLocalization {
    public class PrefixLocalizationOptions {

        internal const string LocaleRouteParameterName = "locale";
        internal const string LocaleRouteConstraintKey = "supported-locale";
        internal const string LocaleContextItemName = "Altairis.PrefixLocalization.Locale";

        public bool UseAcceptLanguageHeader { get; set; } = true;

        public int MaximumAcceptLanguageHeaderValuesToTry { get; set; } = 3;

        public string DefaultLocale { get; set; }

        public ICollection<PrefixToCultureMapping> LocaleMappings { get; set; } = new HashSet<PrefixToCultureMapping>();

        public bool IgnoreWellKnownPaths { get; set; } = true;

        public ICollection<string> IgnorePaths { get; set; } = new HashSet<string>();

        internal void PopulateWellKnownPaths() {
            if (!this.IgnoreWellKnownPaths) return;

            this.IgnorePaths.Add(@"^/\.");                  // Anything beginning with a dot, ie /.well-known
            this.IgnorePaths.Add(@"^/[^/\.]+\.[^/\.]+$");   // Any file with extension in root, ie. /robots.txt
        }

    }
}