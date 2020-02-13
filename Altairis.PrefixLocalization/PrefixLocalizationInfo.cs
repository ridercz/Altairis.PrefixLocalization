using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.PrefixLocalization {
    public class PrefixLocalizationInfo {

        internal PrefixLocalizationInfo(IEnumerable<PrefixToCultureMapping> mappings, string currentLocalePrefix) {
            var list = mappings.Select(x => new LocaleInfo {
                Prefix = x.Prefix,
                Culture = x.Culture,
                UiCulture = x.UiCulture,
                IsCurrent = x.Prefix.Equals(currentLocalePrefix, StringComparison.OrdinalIgnoreCase)
            });
            this.SupportedLocales = list.ToList().AsReadOnly();

            this.CurrentPrefix = currentLocalePrefix;
        }

        public ReadOnlyCollection<LocaleInfo> SupportedLocales { get; }

        public string CurrentPrefix { get; }


    }
}
