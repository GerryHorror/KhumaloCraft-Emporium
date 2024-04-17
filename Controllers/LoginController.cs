using Microsoft.AspNetCore.Mvc;
using CLDVWebAppST10046280.Models;
using CLDVWebApp.Models;
using System.Data.SqlClient;

namespace CLDVWebAppST10046280.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login(string email, string password)
        {
            var loginModel = new LoginModel();
            int userID = loginModel.SelectUser(email, password);
            if (userID != -1)
            {
                // Fetch user details using the existing userTable model
                userTable userDetails = FetchUserDetails(userID);
                return View("~/Views/Home/LoginSuccess.cshtml", userDetails);
            }
            else
            {
                return View("LoginFailed");
            }
        }

        private userTable FetchUserDetails(int userId)
        {
            userTable userDetails = new userTable();
            using (SqlConnection con = new SqlConnection(userTable.con_string))
            {
                string sql = "SELECT userID, userName, userSurname, userEmail FROM userTable WHERE userID = @UserId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userDetails.UserID = Convert.ToInt32(reader["userID"]);
                        userDetails.Name = reader["userName"].ToString();
                        userDetails.Surname = reader["userSurname"].ToString();
                        userDetails.Email = reader["userEmail"].ToString();
                        // Ensure that any sensitive properties are not set or are cleared
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately
                    throw ex;
                }
            }
            return userDetails;
        }
    }
}