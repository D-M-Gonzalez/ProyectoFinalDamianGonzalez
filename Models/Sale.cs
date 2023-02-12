using DamianGonzalesCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamianGonzalezCSharp.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string SaleComments { get; set; } = "";
        public List<SaleProduct>? SaleProducts { get; set; }
    }

    public class SaleProduct
    {
        public Int32? SaleProductId { get; set; }
        public Int32 SaleAmount { get; set; }
        public Int32 ProductId { get; set; }
        public string? Product { get; set; } = "";
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public Int32? Stock { get; set; }
    }

    public class SaleResponse : Response
    {
        public List<Sale>? Sales { get; set; }
    }
}
