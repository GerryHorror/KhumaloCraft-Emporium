using CLDVWebAppST10046280.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Data.SqlClient;

namespace CLDVWebAppST10046280.Controllers
{
    namespace CLDVWebAppST10046280.Controllers
    {
        public class TransactionController : Controller
        {
            public IActionResult Transactions()
            {
                int loggedInUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                var transactions = new TransactionTable().GetTransactionsForUser(loggedInUserId);
                return View(transactions);
            }
        }
    }
}