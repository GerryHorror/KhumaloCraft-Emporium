using System;
using System.Data.SqlClient;

namespace CLDVWebAppST10046280.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string OrderStatus { get; set; }
        public decimal OrderTotal { get; set; }

        public string Status
        {
            get
            {
                return OrderStatus == "P" ? "Pending" : "Completed";
            }
        }

        public static string con_string = "Server=tcp:gerard-clouddev-server.database.windows.net,1433;Initial Catalog=gerard-clouddev-db;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        public static SqlConnection con = new SqlConnection(con_string);

        public int insert_Order(OrderModel m)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = "INSERT INTO orderTable (userID, orderStatus, orderTotal) VALUES (@UserID, @OrderStatus, @OrderTotal)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@UserID", m.UserID);
                    cmd.Parameters.AddWithValue("@OrderStatus", m.OrderStatus);
                    cmd.Parameters.AddWithValue("@OrderTotal", m.OrderTotal);
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