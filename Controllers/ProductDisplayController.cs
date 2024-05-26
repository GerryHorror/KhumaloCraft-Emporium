using Microsoft.AspNetCore.Mvc;
using CLDVWebAppST10046280.Models;

namespace CLDVWebAppST10046280.Controllers
{
    public class ProductDisplayController : Controller
    {
        public IActionResult WorkTest()
        {
            var products = new ProductDisplayModel().GetProducts();
            return View(products);
        }
    }
}