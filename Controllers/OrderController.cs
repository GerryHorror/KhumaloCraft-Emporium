using Microsoft.AspNetCore.Mvc;
using CLDVWebAppST10046280.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace CLDVWebAppST10046280.Controllers
{
    public class OrderController : Controller
    {
        public static string con_string = "Server=tcp:gerard-clouddev-server.database.windows.net,1433;Initial Catalog=gerard-clouddev-db;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        public IActionResult Orders()
        {
            int loggedInUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var orders = GetOrders(loggedInUserId);
            return View(orders);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, string status)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "UPDATE orderTable SET orderStatus = @status WHERE orderID = @orderId";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@orderId", orderId);
                con.Open();
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Orders", "Home");
        }

        [HttpPost]
        public IActionResult RemoveOrder(int orderId)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "DELETE FROM orderTable WHERE orderID = @orderId";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@orderId", orderId);
                con.Open();
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Orders", "Home");
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
        public IActionResult AddToOrder(int productId, int quantity)
        {
            int loggedInUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (loggedInUserId == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            int orderId = GetOrCreateOrder(loggedInUserId);
            CreateTransaction(orderId, productId, quantity);
            UpdateOrderTotal(orderId);
            return RedirectToAction("Transactions", "Home");
        }

        public List<OrderModel> GetOrders(int userId)
        {
            List<OrderModel> orders = new List<OrderModel>();
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "SELECT * FROM orderTable WHERE userID = @userId";
                SqlCommand command = new SqlCommand(query, con);
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
            return orders;
        }

        // Helper method to create a transaction
        private void CreateTransaction(int orderId, int productId, int quantity)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "INSERT INTO transactionTable (orderID, productID, transactionQuantity, transactionDate, transactionStatus) VALUES (@orderId, @productId, @quantity, GETDATE(), 'Pending')";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@productId", productId);
                command.Parameters.AddWithValue("@quantity", quantity);
                con.Open();
                command.ExecuteNonQuery();
            }
        }

        // Helper method to update order total
        private void UpdateOrderTotal(int orderId)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = @"
                UPDATE orderTable
                SET orderTotal = (
                    SELECT SUM(p.productPrice * t.transactionQuantity)
                    FROM transactionTable t
                    JOIN productTable p ON t.productID = p.productID
                    WHERE t.orderID = @orderId
                )
                WHERE orderID = @orderId";

                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@orderId", orderId);
                con.Open();
                command.ExecuteNonQuery();
            }
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