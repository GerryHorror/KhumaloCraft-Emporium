using CLDVWebApp.Models;
using System.Data.SqlClient;

namespace CLDVWebAppST10046280.Models
{
    public class LoginModel
    {
        public static string con_string = "Server=tcp:sql-cldv-st10046280-server.database.windows.net,1433;Initial Catalog=sql-cldv-st10046280-database;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static SqlConnection con = new SqlConnection(con_string);

        public int SelectUser(string email, string password)
        {
            int userId = -1; // Default value if user is not found
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT userID FROM userTable WHERE userEmail = @Email AND userPassword = @Password";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
            return userId;
        }
    }
}