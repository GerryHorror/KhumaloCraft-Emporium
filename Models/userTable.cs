using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace CLDVWebApp.Models
{
    public class userTable : Controller
    {
        // Connection string to connect to the database
        public static string connectionString = "Server=tcp:gerard-clouddev-server.database.windows.net,1433;Initial Catalog=gerard-clouddev-db;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        // Connection object to connect to the database
        public static SqlConnection connection = new SqlConnection(connectionString);

        // User properties to be stored in the database
        public string Name { get; set; }

        public string Surname { get; set; }
        public string Email { get; set; }

        // Method to insert user data into the database table (userTable)
        public int insert_User(userTable m)
        {
            string sql = "INSERT INTO userTable (userName, userSurname, userEmail) VALUES (@Name, @Surname, @Email)";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Name", m.Name);
            cmd.Parameters.AddWithValue("@Surname", m.Surname);
            cmd.Parameters.AddWithValue("@Email", m.Email);
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();
            return rowsAffected;
        }
    }
}