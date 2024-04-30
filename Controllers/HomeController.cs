using CLDVWebApp.Models;
using CLDVWebAppST10046280.Models;
using CLDVWebAppST10046280.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CLDVWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Work()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginSuccess(int userID)
        {
            userTable userDetails = null;
            ViewData["userID"] = userID;
            return View(userDetails);
        }

        // This is a test method to check if the WorkTest view is working
        public IActionResult WorkTest()
        {
            return View();
        }

        public IActionResult Product()
        {
            // Created a new instance of the ProductDisplayModel class and called the GetProducts method to get the products
            var products = new ProductDisplayModel().GetProducts();
            // Returned the products to the Product view to display the products
            return View("Product", products);
        }

        // A test method to check if the user details are stored in the session
        public IActionResult UserDetails()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            string userName = HttpContext.Session.GetString("UserName");
            string userSurname = HttpContext.Session.GetString("UserSurname");
            string userEmail = HttpContext.Session.GetString("UserEmail");

            if (userId != 0)
            {
                var userDetails = new userTable
                {
                    UserID = userId,
                    Name = userName,
                    Surname = userSurname,
                    Email = userEmail
                };
                return View(userDetails);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}