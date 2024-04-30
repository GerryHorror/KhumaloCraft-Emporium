using System.Data.SqlClient;
using System.Transactions;

namespace CLDVWebAppST10046280.Models
{
    public class TransactionModel
    {
        public int TransactionID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

        public static string con_string = "Server=tcp:gerard-clouddev-server.database.windows.net,1433;Initial Catalog=gerard-clouddev-db;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        public void InsertTransactions(int orderId, List<int> cart)
        {
            using (SqlConnection con = new SqlConnection(con_string))
            {
                foreach (var productId in cart)
                {
                    string sql = "INSERT INTO TransactionTable (orderID, productID, transactionDate) VALUES (@OrderId, @ProductId, @TransactionDate)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public List<TransactionModel> GetTransactionsByUserId(int userId)
        {
            List<TransactionModel> transactions = new List<TransactionModel>();
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = @"
                SELECT t.transactionID, t.orderID, t.productID, t.transactionDate, p.productName, p.productPrice
                FROM TransactionTable t
                INNER JOIN OrderTable o ON t.orderID = o.orderID
                INNER JOIN productTable p ON t.productID = p.prodcutID
                WHERE o.userID = @UserId
            ";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    transactions.Add(new TransactionModel
                    {
                        TransactionID = Convert.ToInt32(reader["transactionID"]),
                        OrderID = Convert.ToInt32(reader["orderID"]),
                        ProductID = Convert.ToInt32(reader["productID"]),
                        TransactionDate = Convert.ToDateTime(reader["transactionDate"]),
                        ProductName = reader["productName"].ToString(),
                        ProductPrice = Convert.ToDecimal(reader["productPrice"])
                    });
                }
                con.Close();
            }
            return transactions;
        }
    }
}