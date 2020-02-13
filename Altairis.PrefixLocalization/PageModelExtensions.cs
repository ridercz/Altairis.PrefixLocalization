using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.PrefixLocalization {
    public static class PageModelExtensions {

        public static RedirectToPageResult RedirectToLocalizedPage(this PageModel page) => page.RedirectToLocalizedPage(pageName: null, pageHandler: null, routeValues: null, fragment: null);

        public static RedirectToPageResult RedirectToLocalizedPage(this PageModel page, object routeValues) => page.RedirectToLocalizedPage(pageName: null, pageHandler: null, routeValues, fragment: null);

        public static RedirectToPageResult RedirectToLocalizedPage(this PageModel page, string pageName) => page.RedirectToLocalizedPage(pageName, pageHandler: null, routeValues: null, fragment: null);

        public static RedirectToPageResult RedirectToLocalizedPage(this PageModel page, string pageName, object routeValues) => page.RedirectToLocalizedPage(pageName, pageHandler: null, routeValues, fragment: null);

        public static RedirectToPageResult RedirectToLocalizedPage(this PageModel page, string pageName, string pageHandler) => page.RedirectToLocalizedPage(pageName, pageHandler, routeValues: null, fragment: null);

        public static RedirectToPageResult RedirectToLocalizedPage(this PageModel page, string pageName, string pageHandler, string fragment) => page.RedirectToLocalizedPage(pageName, pageHandler, routeValues: null, fragment);

        public static RedirectToPageResult RedirectToLocalizedPage(this PageModel page, string pageName, string pageHandler, object routeValues, string fragment) {
            var newRouteValues = new Dictionary<string, object>();
            if (routeValues != null) {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(routeValues)) {
                    newRouteValues.Add(descriptor.Name, descriptor.GetValue(routeValues));
                }
            }
            newRouteValues.Add(PrefixLocalizationOptions.LocaleRouteParameterName, page.RouteData.Values[PrefixLocalizationOptions.LocaleRouteParameterName]);
            return page.RedirectToPage(pageName, pageHandler, newRouteValues, fragment);
        }

    }
}
