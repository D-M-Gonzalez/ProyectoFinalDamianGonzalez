using System.Net;

namespace DamianGonzalezCSharp.Models
{
    public class Response
    {
        public Boolean Success { get; set; }
        public string Message { get; set; } = "";
        public HttpStatusCode StatusCode { get; set; }
        public void GenerateResponse(bool success, string message, HttpStatusCode statusCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }
    }

    public enum IdColumns
    {
        sale,
        product,
        productSale,
        user,
    }
}
