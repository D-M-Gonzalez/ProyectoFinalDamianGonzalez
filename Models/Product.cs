
using DamianGonzalezCSharp.Handlers;
using System.Data;
using System.Net;

namespace DamianGonzalezCSharp.Models
{
    public class Product
    {
        public int Id { get; set; } = 0;
        public string? Description { get; set; }
        public decimal? Cost { get; set; }
        public decimal? SellingPrice { get; set; }
        public int? Stock { get; set; }
        public int? UserId { get; set; }
    }
    public class ProductResponse : Response
    {
        public List<Product>? Products { get; set; }

        public ProductResponse()
        {
            Success = true;
            Message = "";
            StatusCode = HttpStatusCode.OK;
            Products = null;
        }
        public ProductResponse (Boolean success, string message, HttpStatusCode statusCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            Products = null;
        }
    }
}
