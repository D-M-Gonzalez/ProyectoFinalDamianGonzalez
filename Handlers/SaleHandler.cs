using DamianGonzalezCSharp.Models;
using System.Data;
using System.Data.SqlClient;

namespace DamianGonzalezCSharp.Handlers
{
    public class SaleHandler : SqlHandler
    {
        private static ParameterHandler handler = new ParameterHandler();
        private Boolean result = false;
        public Boolean HandleGetSales(DataSet ds, Int32 saleId)
        {
            SqlCommand cmd = new SqlCommand();

            handler.CreateParameter("saleId", SqlDbType.BigInt, saleId, cmd);
            cmd.CommandText = "select * from Venta where Id = @saleId";
            result = GetCommand(ds, cmd);

            return result;
        }
        public Boolean HandleGetSales(DataSet ds, string sLike, string order)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from Venta ";

            handler.CreateParameter("descrip", SqlDbType.Char, sLike, cmd, true);
            if (sLike != "") cmd.CommandText = cmd.CommandText + " where Comentarios like @descrip";

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

            result = GetCommand(ds, cmd);

            return result;
        }

        public Boolean HandleCreateSale(Sale data)
        {
            SqlCommand createCommand = new SqlCommand();
            handler.CreateParameter("comments", SqlDbType.Char, data.SaleComments, createCommand);
            handler.CreateParameter("userId", SqlDbType.BigInt, data.UserId, createCommand);

            if (data.SaleProducts.Count > 0)
            {
                createCommand.CommandText = "insert into Venta (Comentarios, IdUsuario ) values (@comments,@userId) ";
                int count = 0;
                foreach (SaleProduct saleProduct in data.SaleProducts)
                {
                    count++;
                    string saleAmountPar = "@saleAmount" + count.ToString();
                    string productIdPar = "@productId" + count.ToString();
                    handler.CreateParameter(saleAmountPar, SqlDbType.BigInt, data.SaleProducts[count - 1].SaleAmount, createCommand);
                    handler.CreateParameter(productIdPar, SqlDbType.BigInt, data.SaleProducts[count - 1].ProductId, createCommand);


                    createCommand.CommandText = createCommand.CommandText + " insert into ProductoVendido (Stock,IdProducto,IdVenta) values (" + saleAmountPar + "," + productIdPar + ",(select MAX(id) from Venta)) update Producto set Stock = Stock - " + saleAmountPar + " where Id = " + productIdPar + " ";
                }
            }

            result = GenericCommand(createCommand);

            return result;
        }

        public Boolean HandleUpdateSale(Sale data)
        {
            string updateText = "";
            SqlCommand updateCommand = new SqlCommand();
            handler.CreateParameter("Comentarios", SqlDbType.Char, data.SaleComments, updateCommand);
            handler.CreateParameter("IdUsuario", SqlDbType.Char, data.UserId, updateCommand);

            foreach (SqlParameter par in updateCommand.Parameters)
            {
                if (par.SqlValue.ToString() != "Null")
                {
                    if (updateText != "") updateText = updateText + ",";
                    updateText = updateText + " " + par.ParameterName + "=@" + par.ParameterName;
                }
            }

            handler.CreateParameter("id", SqlDbType.BigInt, data.Id, updateCommand);

            updateCommand.CommandText = "update Venta set " + updateText + " where Id = @id";
            result = GenericCommand(updateCommand);

            return result;
        }

        public Boolean HandleDeleteSale(Int32 id)
        {
            SqlCommand deleteCommand = new SqlCommand("delete Venta where Id = @id");
            handler.CreateParameter("id", SqlDbType.BigInt, id, deleteCommand);

            result = GenericCommand(deleteCommand);

            return result;
        }

        public Boolean HandleGetSalesByUserId(DataSet ds, Int32 userId)
        {
            SqlCommand cmd = new SqlCommand();

            handler.CreateParameter("saleId", SqlDbType.BigInt, userId, cmd);
            cmd.CommandText = "select * from Venta where IdUsuario = @saleId";
            result = GetCommand(ds, cmd);

            return result;
        }
    }
}
