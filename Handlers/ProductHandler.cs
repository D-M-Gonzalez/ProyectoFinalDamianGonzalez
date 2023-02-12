using DamianGonzalezCSharp.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace DamianGonzalesCSharp.Handlers
{
    public class ProductHandler : SqlHandler
    {
        public Boolean HandleGetProducts(DataSet ds, Int32 productId, string sLike, string order)
        {
            SqlCommand cmd = new SqlCommand();
            Boolean success = false;

            if (productId > 0)
            {
                var parameter = new SqlParameter();
                parameter.ParameterName = "productId";
                parameter.SqlDbType = SqlDbType.BigInt;
                parameter.Value = productId;

                cmd.CommandText = "select * from Producto where Id=@productId";
                cmd.Parameters.Add(parameter);

            } else
            {
                cmd.CommandText = "select * from Producto ";
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
                            sOrder = "Id";
                            break;
                        case "costo":
                            sOrder = "Costo";
                            break;
                        case "precioventa":
                            sOrder = "PrecioVenta";
                            break;
                        case "stock":
                            sOrder = "Stock";
                            break;
                        case "idusuario":
                            sOrder = "IdUsuario";
                            break;
                        default:
                            sOrder = "Descripciones";
                            break;
                    }

                    cmd.CommandText = cmd.CommandText + " order by " + sOrder;
                }

            }
            
            success = GetCommand(ds, cmd);

            return success;
        }

        public Boolean HandleProductData(Product data, Boolean insert)
        {
            Boolean success = false;
            SqlCommand createCommand = new SqlCommand();
            string sData = "";

            if (data.Id != 0)
            {
                var parameter = new SqlParameter();
                parameter.ParameterName = "id";
                parameter.SqlDbType = SqlDbType.Char;
                parameter.Value = data.Id;
                createCommand.Parameters.Add(parameter);
            }

            if (data.Description != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var descrip = new SqlParameter();
                descrip.ParameterName = "descrip";
                descrip.SqlDbType = SqlDbType.Char;
                descrip.Value = data.Description;
                sData = sData + (insert ? " @descrip " : " Descripciones = @descrip ");
                createCommand.Parameters.Add(descrip);
            }

            if (data.Cost != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var cost = new SqlParameter();
                cost.ParameterName = "cost";
                cost.SqlDbType = SqlDbType.Decimal;
                cost.Value = data.Cost;
                sData = sData + (insert ? " @cost " : " Costo = @cost ");
                createCommand.Parameters.Add(cost);
            }

            if (data.SellingPrice != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var price = new SqlParameter();
                price.ParameterName = "price";
                price.SqlDbType = SqlDbType.Decimal;
                price.Value = data.SellingPrice;
                sData = sData + (insert ? " @price " : " PrecioVenta = @price ");
                createCommand.Parameters.Add(price);
            }

            if (data.Stock != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var stock = new SqlParameter();
                stock.ParameterName = "stock";
                stock.SqlDbType = SqlDbType.Decimal;
                stock.Value = data.Stock;
                sData = sData + (insert ? " @stock " : " Stock = @stock ");
                createCommand.Parameters.Add(stock);
            }

            if (data.UserId != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var userId = new SqlParameter();
                userId.ParameterName = "userId";
                userId.SqlDbType = SqlDbType.Char;
                userId.Value = data.UserId;
                sData = sData + (insert ? " @userId " : " IdUsuario = @userId ");
                createCommand.Parameters.Add(userId);
            }

            createCommand.CommandText = insert == true ? "insert into Producto (Descripciones, Costo, PrecioVenta, Stock, IdUsuario) values ( " + sData + ")" : "update Producto set " + sData + " where Id = @id";

            success = GenericCommand(createCommand);

            return success;
        }

        public Boolean HandleDeleteProduct(Int32 id)
        {
            Boolean success = false;

            SqlCommand deleteCommand = new SqlCommand("delete Producto where Id = @id");
            var parameter = new SqlParameter();
            parameter.ParameterName = "id";
            parameter.SqlDbType = SqlDbType.BigInt;
            parameter.Value = id;
            deleteCommand.Parameters.Add(parameter);

            success = GenericCommand(deleteCommand);

            return success;
        }
    }
}
