using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Altairis.PrefixLocalization.Routing {
    public class PrefixLocalizationConvention : IPageRouteModelConvention {

        public void Apply(PageRouteModel model) {
            foreach (var selector in model.Selectors) {
                selector.AttributeRouteModel.Template = AttributeRouteModel.CombineTemplates($"{{{PrefixLocalizationOptions.LocaleRouteParameterName}:{PrefixLocalizationOptions.LocaleRouteConstraintKey}}}/", selector.AttributeRouteModel.Template);
            }

        }
    }
}
