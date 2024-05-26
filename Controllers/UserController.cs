using CLDVWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLDVWebApp.Controllers
{
    public class UserController : Controller
    {
        public userTable usrtbl = new userTable();

        [HttpPost]
        public ActionResult SignUp(userTable Users)
        {
            int userId = usrtbl.insert_User(Users);
            // If the user is successfully added to the database, set the session variables (i.e. log the user in)
            if (userId > 0)
            {
                // Set session variables for the user
                HttpContext.Session.SetInt32("UserID", userId);
                HttpContext.Session.SetString("UserName", Users.Name);
                HttpContext.Session.SetString("UserSurname", Users.Surname);
                HttpContext.Session.SetString("UserEmail", Users.Email);
                HttpContext.Session.SetInt32("IsAdmin", Users.IsAdmin ? 1 : 0);
            }
            // Redirect to the home page
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View(usrtbl);
        }
    }
}