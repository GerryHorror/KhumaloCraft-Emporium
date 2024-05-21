using Microsoft.AspNetCore.Mvc;
using CLDVWebAppST10046280.Models;
using CLDVWebApp.Models;
using System.Data.SqlClient;

namespace CLDVWebAppST10046280.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var loginModel = new LoginModel();
            int userID = loginModel.SelectUser(email, password);
            if (userID != -1)
            {
                // Fetch the user details from the database using the user ID
                userTable userDetails = FetchUserDetails(userID);
                // Store the user details in the session so that they can be accessed on other pages
                HttpContext.Session.SetInt32("UserId", userDetails.UserID);
                HttpContext.Session.SetString("UserName", userDetails.Name);
                HttpContext.Session.SetString("UserSurname", userDetails.Surname);
                HttpContext.Session.SetString("UserEmail", userDetails.Email);
                HttpContext.Session.SetInt32("IsAdmin", userDetails.IsAdmin ? 1 : 0); // Automatically sets isAdmin to false
                // Redirect to the LoginSuccess action method
                return View("~/Views/Home/LoginSuccess.cshtml", userDetails);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login. Please try again.");
                return View("~/Views/Home/Login.cshtml");
            }
        }

        private userTable FetchUserDetails(int userId)
        {
            userTable userDetails = new userTable();
            using (SqlConnection con = new SqlConnection(userTable.con_string))
            {
                string sql = "SELECT userID, userName, userSurname, userEmail, isAdmin FROM userTable WHERE userID = @UserId";
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
                        userDetails.IsAdmin = Convert.ToBoolean(reader["isAdmin"]);
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