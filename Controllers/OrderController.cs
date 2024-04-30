using Microsoft.AspNetCore.Mvc;
using CLDVWebAppST10046280.Models;

namespace CLDVWebAppST10046280.Controllers
{
    public class OrderController : Controller
    {
        [HttpPost]
        public IActionResult AddOrder(OrderModel order)
        {
            order.AddOrder(order);
            return RedirectToAction("Product", "ProductDisplay");
        }
    }
}