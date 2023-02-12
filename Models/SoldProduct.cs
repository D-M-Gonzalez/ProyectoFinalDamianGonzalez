using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamianGonzalezCSharp.Models
{
    public class SoldProduct
    {
        public int Id { get; set; }
        public int ProductSaleId { get; set; }
        public int Amount { get; set; }
        public int SaleId { get; set; }
        public string Product { get; set; } = "";
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
