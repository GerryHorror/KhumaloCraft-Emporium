using CLDVWebApp.Models;
using CLDVWebAppST10046280.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CLDVWebApp.Controllers;
using CLDVWebAppST10046280.Controllers;

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

        public IActionResult LoginSuccess(int userId)
        {
            userTable userDetails = null;
            ViewData["userID"] = userId;
            return View(userDetails);
        }

        // This is a test method to check if the WorkTest view is working
        public IActionResult WorkTest()
        {
            var viewModel = new ProductViewModel
            {
                Products = new ProductDisplayModel().GetProducts(),
                ProductForm = new productTable()
            };
            return View(viewModel);
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
                return RedirectToAction("Login", "Home");
            }
        }

        public IActionResult Orders()
        {
            int loggedInUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var orders = new OrderController().GetOrdersWithItems(loggedInUserId);
            return View(orders);
        }

        public IActionResult Transactions()
        {
            int loggedInUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var transactions = new TransactionTable().GetTransactionsForUser(loggedInUserId);
            return View(transactions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}