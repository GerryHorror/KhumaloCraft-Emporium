// OrderModel.cs
using System.Data.SqlClient;

namespace CLDVWebAppST10046280.Models
{
    public class OrderModel
    {
        public static string con_string = "Server=tcp:gerard-clouddev-server.database.windows.net,1433;Initial Catalog=gerard-clouddev-db;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        public static SqlConnection con = new SqlConnection(con_string);

        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string orderStatus { get; set; }
        public decimal orderTotal { get; set; }

        public int AddOrder(OrderModel order)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = "INSERT INTO orderTable (UserID, orderStatus, orderTotal) VALUES (@UserID, @orderStatus, @orderTotal)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@UserID", order.UserID);
                    cmd.Parameters.AddWithValue("@orderStatus", order.orderStatus);
                    cmd.Parameters.AddWithValue("@orderTotal", order.orderTotal);
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
    }
}