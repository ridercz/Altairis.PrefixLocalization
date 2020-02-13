using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.PrefixLocalization {
    public class LocaleInfo {
        public string Prefix { get; set; }

        public CultureInfo Culture { get; set; }

        public CultureInfo UiCulture { get; set; }

        public bool IsCurrent { get; set; }
    }
}
