using DamianGonzalezCSharp.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace DamianGonzalesCSharp.Handlers
{
    public class SaleHandler : SqlHandler
    {
        public Boolean HandleGetSales(DataSet ds, Int32 saleId, string sLike, string order)
        {
            SqlCommand cmd = new SqlCommand();
            Boolean success = false;

            if (saleId > 0)
            {
                var parameter = new SqlParameter();
                parameter.ParameterName = "saleId";
                parameter.SqlDbType = SqlDbType.BigInt;
                parameter.Value = saleId;

                cmd.CommandText = "select * from Venta where Id = @saleId";
                cmd.Parameters.Add(parameter);

            } else
            {
                cmd.CommandText = "select * from Venta";
                if (sLike != "" && sLike != null)
                {
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "descrip";
                    parameter.SqlDbType = SqlDbType.Char;
                    parameter.Value = "%" + sLike + "%";

                    cmd.CommandText = cmd.CommandText + " where Comentarios like @descrip";
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
                        case "IdUsuario":
                            sOrder = "IdUsuario";
                            break;
                        default:
                            sOrder = "Comentarios";
                            break;
                    }

                    cmd.CommandText = cmd.CommandText + " order by " + sOrder;
                }

            }
            
            success = GetCommand(ds, cmd);

            return success;
        }

        public Boolean HandleSaleData(Sale data, Boolean insert)
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

            if (data.SaleComments != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var comments = new SqlParameter();
                comments.ParameterName = "comments";
                comments.SqlDbType = SqlDbType.Char;
                comments.Value = data.SaleComments;
                sData = sData + (insert ? " @comments " : " Comentarios = @comments ");
                createCommand.Parameters.Add(comments);
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

            if (data.SaleProducts.Count > 0 && data.Id == 0 && insert == true)
            {
                createCommand.CommandText = "insert into Venta (Comentarios, IdUsuario ) values ( " + sData + ") ";
                int count = 0;

                foreach (SaleProduct saleProduct in data.SaleProducts)
                {
                    count++;
                    if (saleProduct.SaleAmount != 0)
                    {
                        var saleAmount = new SqlParameter();
                        saleAmount.ParameterName = "saleAmount" + count;
                        saleAmount.SqlDbType = SqlDbType.BigInt;
                        saleAmount.Value = saleProduct.SaleAmount;
                        createCommand.Parameters.Add(saleAmount);
                    }

                    if (saleProduct.ProductId != 0)
                    {
                        var productId = new SqlParameter();
                        productId.ParameterName = "productId" + count;
                        productId.SqlDbType = SqlDbType.BigInt;
                        productId.Value = saleProduct.ProductId;
                        createCommand.Parameters.Add(productId);
                    }
                    string saleAmountPar = "@saleAmount" + count.ToString();
                    string productIdPar = "@productId" + count.ToString();

                    createCommand.CommandText = createCommand.CommandText + " insert into ProductoVendido (Stock,IdProducto,IdVenta) values (" + saleAmountPar + "," + productIdPar + ",(select MAX(id) from Venta)) update Producto set Stock = Stock - " + saleAmountPar + " where Id = " + productIdPar + " ";
                }

            } else if (data.Id > 0 && insert == false)
            {
                createCommand.CommandText = "update Venta set " + sData + " where Id = @id";
            }

            if (createCommand.CommandText != "" ) success = GenericCommand(createCommand);

            return success;
        }

        /*public Boolean HandleDeleteSale(Int32 id)
        {
            Boolean success = false;

            SqlCommand deleteCommand = new SqlCommand("delete Usuario where Id = @id");
            var parameter = new SqlParameter();
            parameter.ParameterName = "id";
            parameter.SqlDbType = SqlDbType.BigInt;
            parameter.Value = id;
            deleteCommand.Parameters.Add(parameter);

            success = GenericCommand(deleteCommand);

            return success;
        }*/
    }
}
