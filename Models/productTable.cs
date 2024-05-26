using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;

namespace CLDVWebAppST10046280.Models
{
    public class productTable
    {
        public static string con_string = "Server=tcp:sql-cldv-st10046280-server.database.windows.net,1433;Initial Catalog=sql-cldv-st10046280-database;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static SqlConnection con = new SqlConnection(con_string);

        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool Available { get; set; }

        public string Availability
        {
            get
            {
                return Available ? "In Stock" : "Out of Stock";
            }
        }

        public int insert_Product(productTable m)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = "INSERT INTO productTable (productName, productPrice, productCategory, productAvailability) VALUES (@Name, @Price, @Category, @Available)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@Name", m.Name);

                    // Ensure Price is handled correctly as a decimal
                    SqlParameter priceParam = new SqlParameter("@Price", SqlDbType.Money);
                    priceParam.Value = m.Price;
                    cmd.Parameters.Add(priceParam);

                    cmd.Parameters.AddWithValue("@Category", m.Category);
                    cmd.Parameters.AddWithValue("@Available", m.Available);
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

        public int UpdateProduct(productTable m)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = "UPDATE productTable SET productName = @Name, productPrice = @Price, productCategory = @Category, productAvailability = @Available WHERE productID = @ProductID";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@ProductID", m.ProductID);
                    cmd.Parameters.AddWithValue("@Name", m.Name);

                    // Ensure Price is handled correctly as a decimal
                    SqlParameter priceParam = new SqlParameter("@Price", SqlDbType.Money);
                    priceParam.Value = m.Price;
                    cmd.Parameters.Add(priceParam);

                    cmd.Parameters.AddWithValue("@Category", m.Category);
                    cmd.Parameters.AddWithValue("@Available", m.Available);
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