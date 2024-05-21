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
            int newUserId = usrtbl.insert_User(Users);
            // If the user is successfully added to the database, set the session variables (i.e. log the user in)
            if (newUserId > 0)
            {
                // Set session variables for the user
                HttpContext.Session.SetInt32("UserID", newUserId);
                HttpContext.Session.SetString("UserName", Users.Name);
                HttpContext.Session.SetString("UserSurname", Users.Surname);
                HttpContext.Session.SetString("UserEmail", Users.Email);
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