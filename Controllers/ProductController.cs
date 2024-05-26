using CLDVWebApp.Models;
using CLDVWebAppST10046280.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CLDVWebAppST10046280.Controllers
{
    public class ProductController : Controller
    {
        private productTable prdtbl = new productTable();

        [HttpPost]
        public ActionResult WorkTest(productTable Products)
        {
            // Handle both comma and period as decimal separators
            string priceString = Products.Price.ToString().Replace(',', '.');
            decimal priceValue;

            // Try parsing with the invariant culture (period as decimal separator)
            if (decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out priceValue))
            {
                Products.Price = priceValue;
            }
            else
            {
                ModelState.AddModelError("Price", "Invalid price format.");
                return View(Products);
            }

            var result = prdtbl.insert_Product(Products);
            if (result > 0)
            {
                return RedirectToAction("WorkTest", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Error while inserting the product.");
                return View(Products);
            }
        }

        [HttpGet]
        public ActionResult WorkTest()
        {
            return View(prdtbl);
        }
    }
}