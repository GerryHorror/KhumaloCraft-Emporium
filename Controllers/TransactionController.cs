using CLDVWebAppST10046280.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLDVWebAppST10046280.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Transactions(int userId)
        {
            var transactionModel = new TransactionModel();
            var transactions = transactionModel.GetTransactionsByUserId(userId);
            return View(transactions);
        }
    }
}