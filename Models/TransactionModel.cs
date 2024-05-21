using System.Data.SqlClient;
using System.Transactions;

namespace CLDVWebAppST10046280.Models
{
    public class TransactionModel
    {
        public int TransactionID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int TransactionQuantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
        public decimal ProductPrice { get; set; }
    }

    public class TransactionTable
    {
        public static string con_string = "Server=tcp:gerard-clouddev-server.database.windows.net,1433;Initial Catalog=gerard-clouddev-db;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        public int InsertTransaction(TransactionModel m)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = "INSERT INTO transactionTable (orderID, productID, transactionQuantity, transactionDate, transactionStatus) VALUES (@OrderID, @ProductID, @TransactionQuantity, @TransactionDate, @TransactionStatus)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@OrderID", m.OrderID);
                    cmd.Parameters.AddWithValue("@ProductID", m.ProductID);
                    cmd.Parameters.AddWithValue("@TransactionQuantity", m.TransactionQuantity);
                    cmd.Parameters.AddWithValue("@TransactionDate", m.TransactionDate);
                    cmd.Parameters.AddWithValue("@TransactionStatus", m.TransactionStatus);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();

                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw ex;
            }
        }

        public List<TransactionModel> GetTransactionsForUser(int userId)
        {
            List<TransactionModel> transactions = new List<TransactionModel>();
            string connectionString = TransactionTable.con_string;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sql = @"SELECT t.transactionID, t.orderID, t.productID, t.transactionQuantity, t.transactionDate, t.transactionStatus,
                           p.productName, p.productPrice
                           FROM transactionTable t
                           JOIN orderTable o ON t.orderID = o.orderID
                           JOIN productTable p ON t.productID = p.productID
                           WHERE o.userID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        con.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                TransactionModel transaction = new TransactionModel
                                {
                                    TransactionID = dr["transactionID"] != DBNull.Value ? Convert.ToInt32(dr["transactionID"]) : 0,
                                    OrderID = dr["orderID"] != DBNull.Value ? Convert.ToInt32(dr["orderID"]) : 0,
                                    ProductID = dr["productID"] != DBNull.Value ? Convert.ToInt32(dr["productID"]) : 0,
                                    ProductName = dr["productName"].ToString(),
                                    TransactionQuantity = dr["transactionQuantity"] != DBNull.Value ? Convert.ToInt32(dr["transactionQuantity"]) : 0,
                                    TransactionDate = dr["transactionDate"] != DBNull.Value ? Convert.ToDateTime(dr["transactionDate"]) : DateTime.MinValue,
                                    TransactionStatus = dr["transactionStatus"].ToString(),
                                    ProductPrice = dr["productPrice"] != DBNull.Value ? Convert.ToDecimal(dr["productPrice"]) : 0
                                };
                                transactions.Add(transaction);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exceptions here
                throw new ApplicationException("Database error occurred.", sqlEx);
            }
            catch (Exception ex)
            {
                // Log other types of exceptions here
                throw new ApplicationException("An error occurred.", ex);
            }

            return transactions;
        }
    }
}