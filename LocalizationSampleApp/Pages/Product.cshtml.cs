using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LocalizationSampleApp.Pages {
    public class ProductModel : PageModel {

        public ProductModel() {
        }

        public int ProductId { get; set; }

        public IActionResult OnGet(int productId) {
            this.ProductId = productId;
            return this.Page();
        }
    }
}