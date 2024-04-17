using CLDVWebApp.Models;
using CLDVWebAppST10046280.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLDVWebAppST10046280.Controllers
{
    public class ProductController : Controller
    {
        private productTable prdtbl = new productTable();

        [HttpPost]
        public ActionResult WorkTest(productTable Products)
        {
            var result = prdtbl.insert_Product(Products);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult WorkTest()
        {
            return View(prdtbl);
        }
    }
}