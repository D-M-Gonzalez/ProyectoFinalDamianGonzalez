using System.Data.SqlClient;
using System.Data;
using DamianGonzalezCSharp.Models;

namespace DamianGonzalezCSharp.Handlers
{
    public class ProductSaleHandler : SqlHandler
    {
        private static ParameterHandler handler = new ParameterHandler();
        private Boolean result = false;
        public Boolean HandleGetProductSales(DataSet ds, Int32 productSaleId)
        {
            return HandleGetProductSales(ds, productSaleId, IdColumns.productSale);
        }
        public Boolean HandleGetProductSales(DataSet ds, Int32 productSaleId, IdColumns idColumn)
        {
            SqlCommand cmd = new SqlCommand();
            handler.CreateParameter("id", SqlDbType.BigInt, productSaleId, cmd);
            string sWhere = " where ";

            switch (idColumn)
            {
                case IdColumns.sale:
                    sWhere = sWhere + " pv.IdVenta = @id";
                    break;
                case IdColumns.product:
                    sWhere = sWhere + " pv.IdProducto = @id";
                    break;
                default:
                    sWhere = sWhere + " pv.Id = @id";
                    break;
            }

            cmd.CommandText = "select pv.Id, pv.Stock Cantidad, pv.IdProducto, pv.IdVenta, p.Descripciones, p.Costo, p.PrecioVenta, p.Stock from ProductoVendido pv inner join Producto p on p.id = pv.IdProducto" + sWhere;
            result = GetCommand(ds, cmd);

            return result;
        }

        public Boolean HandleGetProductSales(DataSet ds, string sLike, string order)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select pv.Id, pv.Stock Cantidad, pv.IdProducto, pv.IdVenta, p.Descripciones, p.Costo, p.PrecioVenta, p.Stock from ProductoVendido pv inner join Producto p on p.id = pv.IdProducto ";

            handler.CreateParameter("descrip", SqlDbType.Char, sLike, cmd, true);
            if (sLike != "") cmd.CommandText = cmd.CommandText + " where p.Descripciones like @descrip";

            if (order != "" && order != null)
            {
                string sOrder = "";
                switch (order)
                {
                    case "id":
                        sOrder = "v.Id";
                        break;
                    case "cantidad":
                        sOrder = "pv.Stock";
                        break;
                    case "stock":
                        sOrder = "p.Stock";
                        break;
                    case "costo":
                        sOrder = "p.Costo";
                        break;
                    case "precio":
                        sOrder = "p.PrecioVenta";
                        break;
                    default:
                        sOrder = "p.Descripciones";
                        break;
                }

                cmd.CommandText = cmd.CommandText + " order by " + sOrder;
            }
            result = GetCommand(ds, cmd);

            return result;
        }

        public Boolean HandleDeleteProductSale(Int32 id)
        {

            SqlCommand deleteCommand = new SqlCommand("delete ProductoVendido where Id = @id");
            handler.CreateParameter("id", SqlDbType.BigInt, id, deleteCommand);

            result = GenericCommand(deleteCommand);

            return result;
        }
    }
}
