using DamianGonzalesCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamianGonzalezCSharp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal? Cost { get; set; }
        public decimal? SellingPrice { get; set; }
        public int? Stock { get; set; }
        public int? UserId { get; set; }
    }

    public class ProductResponse : Response
    {
        public List<Product>? Products { get; set; }
    }
}
