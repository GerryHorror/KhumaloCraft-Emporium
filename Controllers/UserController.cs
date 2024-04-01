using CLDVWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLDVWebApp.Controllers
{
    public class UserController : Controller
    {
        public userTable newUser = new userTable();

        [HttpPost]
        public ActionResult About(userTable Users)
        {
            var result = newUser.insert_User(Users);
            return RedirectToAction("Privacy", "Index");
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}