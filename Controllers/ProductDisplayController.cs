using Microsoft.AspNetCore.Mvc;
using CLDVWebAppST10046280.Models;

namespace CLDVWebAppST10046280.Controllers
{
    public class ProductDisplayController : Controller
    {
        public IActionResult Product()
        {
            var products = new ProductDisplayModel().GetProducts();
            return View(products);
        }
    }
}