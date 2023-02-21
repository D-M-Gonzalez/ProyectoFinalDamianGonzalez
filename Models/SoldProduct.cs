
using System.Net;

namespace DamianGonzalezCSharp.Models
{
    public class SoldProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int SaleId { get; set; }
        public string Product { get; set; } = "";
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class ProductSaleResponse : Response
    {
        public List<SoldProduct>? Products { get; set; }

        public ProductSaleResponse()
        {
            Success = true;
            Message = "";
            StatusCode = HttpStatusCode.OK;
            Products = null;
        }
        public ProductSaleResponse(Boolean success, string message, HttpStatusCode statusCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            Products = null;
        }
    }
}
