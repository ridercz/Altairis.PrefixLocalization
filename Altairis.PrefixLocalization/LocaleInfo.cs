using System.Globalization;

namespace Altairis.PrefixLocalization {
    public class LocaleInfo {
        public string Prefix { get; set; }

        public CultureInfo Culture { get; set; }

        public CultureInfo UiCulture { get; set; }

        public bool IsCurrent { get; set; }
    }
}
