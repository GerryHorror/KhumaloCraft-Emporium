// OrderModel.cs
using System.Data.SqlClient;

namespace CLDVWebAppST10046280.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string OrderStatus { get; set; }
        public decimal OrderTotal { get; set; }
    }
}