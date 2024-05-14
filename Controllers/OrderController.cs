using Microsoft.AspNetCore.Mvc;
using CLDVWebAppST10046280.Models;
using System.Data.SqlClient;
using System.Dynamic;

namespace CLDVWebAppST10046280.Controllers
{
    public class OrderController : Controller
    {
        public static string con_string = "Server=tcp:gerard-clouddev-server.database.windows.net,1433;Initial Catalog=gerard-clouddev-db;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        public IActionResult Transactions()
        {
            int loggedInUserId = HttpContext.Session.GetInt32("UserID") ?? 0;
            var orders = GetOrders(loggedInUserId);
            return View(orders);
        }

        // GET: Get or create order
        private int GetOrCreateOrder(int userId)
        {
            int orderId = GetExistingOrderId(userId); // Check for existing pending order
            if (orderId == 0)
            {
                orderId = CreateNewOrder(userId); // Create a new order if none exist
            }
            return orderId;
        }

        // GET: Existing Order ID
        private int GetExistingOrderId(int userId)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "SELECT orderID FROM orderTable WHERE userID = @userId AND orderStatus = 'Pending'";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@userId", userId);
                con.Open();
                var result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        // POST: Create New Order
        private int CreateNewOrder(int userId)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "INSERT INTO orderTable (userID, orderStatus, orderTotal) VALUES (@userId, 'Pending', 0); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@userId", userId);
                con.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        [HttpPost]
        public IActionResult AddToOrder(int productId)
        {
            int loggedInUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (loggedInUserId == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            int orderId = GetOrCreateOrder(loggedInUserId);
            CreateTransaction(orderId, productId);
            return RedirectToAction("Transactions", "Home");
        }

        private List<OrderModel> GetOrders(int userId)
        {
            List<OrderModel> orders = new List<OrderModel>();
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "SELECT * FROM orderTable WHERE userID = @userId";

                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderModel order = new OrderModel
                            {
                                OrderID = reader.GetInt32(reader.GetOrdinal("orderID")),
                                UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                                OrderStatus = reader.GetString(reader.GetOrdinal("orderStatus")),
                                OrderTotal = reader.GetDecimal(reader.GetOrdinal("orderTotal"))
                            };

                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        // Helper method to create a transaction
        private void CreateTransaction(int orderId, int productId)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "INSERT INTO transactionTable (orderID, productID, transactionQuantity, transactionDate, transactionStatus) VALUES (@orderId, @productId, 1, GETDATE(), 'Pending')";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@productId", productId);
                con.Open();
                command.ExecuteNonQuery();
            }
        }

        // POST: Finalize Order
        public IActionResult FinalizeOrder(int orderId)
        {
            int loggedInUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (loggedInUserId == 0)
            {
                return RedirectToAction("Login", "Login");
            }

            FinalizeOrderStatus(orderId); // Change order status to 'Complete'
            return RedirectToAction("OrderConfirmation", "Home");
        }

        // Helper method to finalize order status
        private void FinalizeOrderStatus(int orderId)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "UPDATE orderTable SET orderStatus = 'Complete' WHERE orderID = @orderId";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@orderId", orderId);
                con.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}