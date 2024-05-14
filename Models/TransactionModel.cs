using System.Data.SqlClient;
using System.Transactions;

namespace CLDVWebAppST10046280.Models
{
    public class TransactionModel
    {
        public int TransactionID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int TransactionQuantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
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
            try
            {
                using (SqlConnection con = new SqlConnection(TransactionTable.con_string))
                {
                    string sql = @" SELECT t.* FROM transactionTable t
                                    JOIN orderTable o ON t.orderID = o.orderID
                                    WHERE o.userID = @UserID";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        TransactionModel transaction = new TransactionModel();
                        transaction.TransactionID = dr["transactionID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["transactionID"]);
                        transaction.OrderID = dr["orderID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["orderID"]);
                        transaction.ProductID = dr["productID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["productID"]);
                        transaction.TransactionQuantity = dr["transactionQuantity"] == DBNull.Value ? 0 : Convert.ToInt32(dr["transactionQuantity"]);
                        transaction.TransactionDate = dr["transactionDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["transactionDate"]);
                        transaction.TransactionStatus = dr["transactionStatus"] == DBNull.Value ? string.Empty : dr["transactionStatus"].ToString();
                        transactions.Add(transaction);
                    }
                }
                return transactions;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw ex;
            }
        }
    }
}