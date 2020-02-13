using System.Globalization;

namespace Altairis.PrefixLocalization {
    public class PrefixToCultureMapping {

        // General properties

        public string Prefix { get; }

        public CultureInfo Culture { get; }

        public CultureInfo UiCulture { get; }

        // Construct from prefix and CultureInfo object(s)

        public PrefixToCultureMapping(string prefix, CultureInfo culture) : this(prefix, culture, culture) { }

        public PrefixToCultureMapping(string prefix, CultureInfo culture, CultureInfo uiCulture) {
            this.Prefix = prefix;
            this.Culture = culture;
            this.UiCulture = uiCulture;
        }

        // Construct from prefix and culture string(s)

        public PrefixToCultureMapping(string prefix, string culture) : this(prefix, culture, culture) { }

        public PrefixToCultureMapping(string prefix, string culture, string uiCulture) {
            this.Prefix = prefix;
            this.Culture = new CultureInfo(culture);
            this.UiCulture = new CultureInfo(uiCulture);
        }

        // Construct from prefix and Windows LCID(s)

        public PrefixToCultureMapping(string prefix, int culture) : this(prefix, culture, culture) { }

        public PrefixToCultureMapping(string prefix, int culture, int uiCulture) {
            this.Prefix = prefix;
            this.Culture = new CultureInfo(culture);
            this.UiCulture = new CultureInfo(uiCulture);
        }

        // Construct by autogenerating prefix from culture name

        public PrefixToCultureMapping(string cultureName, PrefixGenerationMethod method = PrefixGenerationMethod.TwoLetterISOLanguageName) {
            this.Culture = new CultureInfo(cultureName);
            this.UiCulture = this.Culture;
            switch (method) {
                case PrefixGenerationMethod.Name:
                    this.Prefix = this.Culture.Name;
                    break;
                case PrefixGenerationMethod.TwoLetterISOLanguageName:
                    this.Prefix = this.Culture.TwoLetterISOLanguageName;
                    break;
                case PrefixGenerationMethod.ThreeLetterISOLanguageName:
                    this.Prefix = this.Culture.ThreeLetterISOLanguageName;
                    break;
                case PrefixGenerationMethod.ThreeLetterWindowsLanguageName:
                    this.Prefix = this.Culture.ThreeLetterWindowsLanguageName;
                    break;
                case PrefixGenerationMethod.LCID:
                    this.Prefix = this.Culture.LCID.ToString();
                    break;
            }
        }

        public enum PrefixGenerationMethod {
            Name = 0,
            TwoLetterISOLanguageName = 1,
            ThreeLetterISOLanguageName = 2,
            ThreeLetterWindowsLanguageName = 3,
            LCID = 4
        }

    }
}
