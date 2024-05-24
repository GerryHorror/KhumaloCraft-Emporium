using Microsoft.AspNetCore.Mvc;
using CLDVWebAppST10046280.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace CLDVWebAppST10046280.Controllers
{
    public class OrderController : Controller
    {
        public static string con_string = "Server=tcp:sql-cldv-st10046280-server.database.windows.net,1433;Initial Catalog=sql-cldv-st10046280-database;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public IActionResult Orders()
        {
            int loggedInUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var orders = GetOrdersWithItems(loggedInUserId);
            return View(orders);
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
            return RedirectToAction("WorkTest", "Home");
        }

        // Helper method to create a transaction
        private void CreateTransaction(int orderId, int productId, int quantity)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                con.Open();

                // Check if a transaction for the same product already exists
                string checkQuery = "SELECT transactionQuantity FROM transactionTable WHERE orderID = @orderId AND productID = @productId";
                SqlCommand checkCommand = new SqlCommand(checkQuery, con);
                checkCommand.Parameters.AddWithValue("@orderId", orderId);
                checkCommand.Parameters.AddWithValue("@productId", productId);
                object result = checkCommand.ExecuteScalar();

                if (result != null)
                {
                    // If a transaction exists, update the quantity
                    string updateQuery = "UPDATE transactionTable SET transactionQuantity = transactionQuantity + @quantity WHERE orderID = @orderId AND productID = @productId";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, con);
                    updateCommand.Parameters.AddWithValue("@orderId", orderId);
                    updateCommand.Parameters.AddWithValue("@productId", productId);
                    updateCommand.Parameters.AddWithValue("@quantity", quantity);
                    updateCommand.ExecuteNonQuery();
                }
                else
                {
                    // If no transaction exists, create a new one
                    string insertQuery = "INSERT INTO transactionTable (orderID, productID, transactionQuantity, transactionDate, transactionStatus) VALUES (@orderId, @productId, @quantity, GETDATE(), 'Pending')";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, con);
                    insertCommand.Parameters.AddWithValue("@orderId", orderId);
                    insertCommand.Parameters.AddWithValue("@productId", productId);
                    insertCommand.Parameters.AddWithValue("@quantity", quantity);
                    insertCommand.ExecuteNonQuery();
                }
            }
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
        public IActionResult UpdateOrderStatus(int orderId, string status)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "SELECT orderStatus FROM orderTable WHERE orderID = @orderId";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@orderId", orderId);
                con.Open();
                string currentStatus = command.ExecuteScalar()?.ToString();

                if (currentStatus == "Complete")
                {
                    // Optionally return an error message or a specific view
                    return RedirectToAction("Orders");
                }

                query = "UPDATE orderTable SET orderStatus = @status WHERE orderID = @orderId";
                command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@orderId", orderId);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Orders", "Home");
        }

        [HttpPost]
        public IActionResult RemoveItemFromOrder(int orderId, int productId)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string query = "SELECT orderStatus FROM orderTable WHERE orderID = @orderId";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@orderId", orderId);
                con.Open();
                string currentStatus = command.ExecuteScalar()?.ToString();

                if (currentStatus == "Complete")
                {
                    // Optionally return an error message or a specific view
                    return RedirectToAction("Orders", "Home");
                }

                // Check the quantity of the product in the order
                query = "SELECT transactionQuantity FROM transactionTable WHERE orderID = @orderId AND productID = @productId";
                command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@productId", productId);
                int quantity = (int)command.ExecuteScalar();

                if (quantity > 1)
                {
                    // If quantity is more than 1, decrease it by 1
                    query = "UPDATE transactionTable SET transactionQuantity = transactionQuantity - 1 WHERE orderID = @orderId AND productID = @productId";
                }
                else
                {
                    // If quantity is 1, delete the transaction
                    query = "DELETE FROM transactionTable WHERE orderID = @orderId AND productID = @productId";
                }

                command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@productId", productId);
                command.ExecuteNonQuery();

                // Update the order total after removing the item
                UpdateOrderTotal(orderId);
            }
            return RedirectToAction("Orders", "Home");
        }


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

        public List<OrderModel> GetOrdersWithItems(int userId)
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
                        var order = new OrderModel
                        {
                            OrderID = reader.GetInt32(reader.GetOrdinal("orderID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("userID")),
                            OrderStatus = reader.GetString(reader.GetOrdinal("orderStatus")),
                            // Add null check for OrderTotal
                            OrderTotal = !reader.IsDBNull(reader.GetOrdinal("orderTotal")) ? reader.GetDecimal(reader.GetOrdinal("orderTotal")) : 0
                        };
                        order.Items = GetOrderItems(order.OrderID);
                        orders.Add(order);
                    }
                }
            }
            return orders;
        }

        private List<TransactionModel> GetOrderItems(int orderId)
        {
            List<TransactionModel> items = new List<TransactionModel>();
            using (SqlConnection con = new SqlConnection(con_string))
            {
                try
                {
                    string query = @"
                    SELECT t.transactionID, t.orderID, t.productID, t.transactionQuantity, t.transactionDate, t.transactionStatus,
                    p.productName, p.productPrice
                    FROM transactionTable t
                    JOIN productTable p ON t.productID = p.productID
                    WHERE t.orderID = @orderId";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@orderId", orderId);
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new TransactionModel
                            {
                                TransactionID = reader.GetInt32(reader.GetOrdinal("transactionID")),
                                OrderID = reader.GetInt32(reader.GetOrdinal("orderID")),
                                ProductID = reader.GetInt32(reader.GetOrdinal("productID")),
                                ProductName = reader.IsDBNull(reader.GetOrdinal("productName")) ? string.Empty : reader.GetString(reader.GetOrdinal("productName")),
                                TransactionQuantity = reader.GetInt32(reader.GetOrdinal("transactionQuantity")),
                                TransactionDate = reader.GetDateTime(reader.GetOrdinal("transactionDate")),
                                TransactionStatus = reader.IsDBNull(reader.GetOrdinal("transactionStatus")) ? string.Empty : reader.GetString(reader.GetOrdinal("transactionStatus")),
                                ProductPrice = reader.IsDBNull(reader.GetOrdinal("productPrice")) ? 0 : reader.GetDecimal(reader.GetOrdinal("productPrice"))
                            };
                            items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine("An error occurred: " + ex.Message);
                    throw;
                }
            }
            return items;
        }
    }
}