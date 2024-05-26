using System.Data.SqlClient;

namespace CLDVWebApp.Models
{
    public class userTable
    {
        public static string con_string = "Server=tcp:sql-cldv-st10046280-server.database.windows.net,1433;Initial Catalog=sql-cldv-st10046280-database;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static SqlConnection con = new SqlConnection(con_string);

        public int UserID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public int insert_User(userTable m)
        {
            try
            {
                string sql = "INSERT INTO userTable (userName, userSurname, userEmail, userPassword, isAdmin) OUTPUT INSERTED.UserID VALUES (@Name, @Surname, @Email, @Password, @Admin)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Name", m.Name);
                cmd.Parameters.AddWithValue("@Surname", m.Surname);
                cmd.Parameters.AddWithValue("@Email", m.Email);
                cmd.Parameters.AddWithValue("@Password", m.Password);
                cmd.Parameters.AddWithValue("@Admin", m.IsAdmin ? 1 : 0);
                con.Open();
                // Execute the command and return the new user ID to the calling method
                int userId = (int)cmd.ExecuteScalar();
                con.Close();
                return userId;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // For now, rethrow the exception
                throw ex;
            }
        }
    }
}