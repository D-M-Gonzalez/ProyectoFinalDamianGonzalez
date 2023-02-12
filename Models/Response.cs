using DamianGonzalezCSharp.Models;
using System.Net;
using System.Text.Json.Nodes;

namespace DamianGonzalesCSharp.Models
{
    public class Response
    {
        public Boolean Success { get; set; }
        public string Message { get; set; } = "";
        public HttpStatusCode StatusCode { get; set; }

    }
}
