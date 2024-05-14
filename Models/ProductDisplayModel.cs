// ProductDisplayModel.cs

using System.Data.SqlClient;

namespace CLDVWebAppST10046280.Models
{
    public class ProductDisplayModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductCategory { get; set; }
        public bool ProductAvailable { get; set; }

        // This displays the availability of the product as "In Stock" or "Out of Stock" instead of "True" or "False"
        public string Availability
        {
            get
            {
                return ProductAvailable ? "In Stock" : "Out of Stock";
            }
        }

        public ProductDisplayModel()
        { }

        public ProductDisplayModel(int productID, string name, decimal price, string category, bool available)
        {
            ProductID = productID;
            ProductName = name;
            ProductPrice = price;
            ProductCategory = category;
            ProductAvailable = available;
        }

        public List<ProductDisplayModel> GetProducts()
        {
            List<ProductDisplayModel> products = new List<ProductDisplayModel>();
            try
            {
                string con_string = "Server=tcp:gerard-clouddev-server.database.windows.net,1433;Initial Catalog=gerard-clouddev-db;Persist Security Info=False;User ID=Gerard;Password=vuhpis-sEbpat-zezho2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

                using (SqlConnection con = new SqlConnection(con_string))

                {
                    string sql = "SELECT * FROM productTable";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ProductDisplayModel product = new ProductDisplayModel();
                        product.ProductID = dr["productID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["productID"]);
                        product.ProductName = dr["productName"] == DBNull.Value ? string.Empty : dr["productName"].ToString();
                        product.ProductPrice = dr["productPrice"] == DBNull.Value ? 0 : Math.Round(Convert.ToDecimal(dr["productPrice"]), 2);
                        product.ProductCategory = dr["productCategory"] == DBNull.Value ? string.Empty : dr["productCategory"].ToString();
                        product.ProductAvailable = dr["productAvailability"] == DBNull.Value ? false : Convert.ToBoolean(dr["productAvailability"]);
                        products.Add(product);
                    }
                }
                return products;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw ex;
            }
        }
    }
}