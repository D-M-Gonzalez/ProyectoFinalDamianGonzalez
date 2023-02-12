using System.Data.SqlClient;
using System.Data;

namespace DamianGonzalesCSharp.Handlers
{
    public class ProductSaleHandler : SqlHandler
    {
        public Boolean HandleGetProductSales(DataSet ds, Int32 productSaleId, Int32 saleId, string sLike, string order)
        {
            SqlCommand cmd = new SqlCommand();
            Boolean success = false;

            if (productSaleId > 0 && saleId == 0)
            {
                var parameter = new SqlParameter();
                parameter.ParameterName = "saleId";
                parameter.SqlDbType = SqlDbType.BigInt;
                parameter.Value = productSaleId;

                cmd.CommandText = "select pv.Id, pv.Stock Cantidad, pv.IdProducto, pv.IdVenta, p.Descripciones, p.Costo, p.PrecioVenta, p.Stock from ProductoVendido pv inner join Producto p on p.id = pv.IdProducto where Id = @saleId";
                cmd.Parameters.Add(parameter);

            }
            if (saleId > 0 && productSaleId == 0)
            {
                var parameter = new SqlParameter();
                parameter.ParameterName = "saleId";
                parameter.SqlDbType = SqlDbType.BigInt;
                parameter.Value = saleId;

                cmd.CommandText = "select pv.Id, pv.Stock Cantidad, pv.IdProducto, pv.IdVenta, p.Descripciones, p.Costo, p.PrecioVenta, p.Stock from ProductoVendido pv inner join Producto p on p.id = pv.IdProducto where IdVenta = @saleId";
                cmd.Parameters.Add(parameter);

            }
            else
            {
                cmd.CommandText = "select pv.Id, pv.Stock Cantidad, pv.IdProducto, pv.IdVenta, p.Descripciones, p.Costo, p.PrecioVenta, p.Stock from ProductoVendido pv inner join Producto p on p.id = pv.IdProducto";
                if (sLike != "" && sLike != null)
                {
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "descrip";
                    parameter.SqlDbType = SqlDbType.Char;
                    parameter.Value = "%" + sLike + "%";

                    cmd.CommandText = cmd.CommandText + " where Descripciones like @descrip";
                    cmd.Parameters.Add(parameter);
                }

                if (order == null) order = "";
                order = order.ToLower();

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

            }

            success = GetCommand(ds, cmd);

            return success;
        }
    }
}
