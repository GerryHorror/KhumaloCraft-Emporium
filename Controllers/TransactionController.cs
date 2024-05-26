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
                bool isAdmin = HttpContext.Session.GetInt32("IsAdmin") == 1;
                var transactions = new TransactionTable().GetTransactionsForUser(loggedInUserId, isAdmin);
                return View(transactions);
            }

            [HttpPost]
            public IActionResult UpdateTransactionStatus(int transactionId, string status)
            {
                int? isAdmin = HttpContext.Session.GetInt32("IsAdmin");
                if (isAdmin != 1)
                {
                    return Content("You must be an admin to update the transaction status.");
                }

                try
                {
                    new TransactionTable().UpdateTransactionStatus(transactionId, status);
                    return Content("Transaction status updated successfully.");
                }
                catch (Exception ex)
                {
                    // Log the exception
                    return Content("An error occurred: " + ex.Message);
                }
            }
        }
    }
}