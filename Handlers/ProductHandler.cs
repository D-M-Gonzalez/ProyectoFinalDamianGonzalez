using DamianGonzalezCSharp.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DamianGonzalezCSharp.Handlers
{
    public class ProductHandler : SqlHandler
    {
        private static ParameterHandler handler = new ParameterHandler();
        private Boolean result = false;
        public Boolean HandleGetProducts(DataSet ds, Int32 id)
        {
            return HandleGetProducts(ds, id, IdColumns.product);
        }
        public Boolean HandleGetProducts(DataSet ds, Int32 id, IdColumns idColumn)
        {
            SqlCommand cmd = new SqlCommand();
            string sWhere = " where ";
            handler.CreateParameter("id", SqlDbType.BigInt, id, cmd);

            switch (idColumn)
            {
                case IdColumns.user:
                    sWhere = sWhere + " IdUsuario = @id";
                    break;
                default:
                    sWhere = sWhere + " Id=@id";
                    break;
            }

            cmd.CommandText = "select * from Producto " + sWhere;

            result = GetCommand(ds, cmd);

            return result;
        }

        public Boolean HandleGetProducts(DataSet ds, string sLike, string order)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from Producto ";

            handler.CreateParameter("descrip", SqlDbType.Char, sLike, cmd, true);
            if (sLike != "") cmd.CommandText = cmd.CommandText + " where Descripciones like @descrip";

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

            result = GetCommand(ds, cmd);

            return result;
        }
        public Boolean HandleCreateProduct(Product data)
        {
            SqlCommand createCommand = new SqlCommand();
            handler.CreateParameter("descrip", SqlDbType.Char, data.Description, createCommand);
            handler.CreateParameter("cost", SqlDbType.Decimal, data.Cost, createCommand);
            handler.CreateParameter("price", SqlDbType.Decimal, data.SellingPrice, createCommand);
            handler.CreateParameter("stock", SqlDbType.BigInt, data.Stock, createCommand);
            handler.CreateParameter("userId", SqlDbType.BigInt, data.UserId, createCommand);

            createCommand.CommandText = "insert into Producto (Descripciones, Costo, PrecioVenta, Stock, IdUsuario) values (@descrip,@cost,@price,@stock,@userId)";
            result = GenericCommand(createCommand);

            return result;
        }

        public Boolean HandleUpdateProduct(Product data)
        {
            string updateText = "";
            SqlCommand updateCommand = new SqlCommand();
            handler.CreateParameter("Descripciones", SqlDbType.Char, data.Description, updateCommand);
            handler.CreateParameter("Costo", SqlDbType.Decimal, data.Cost, updateCommand);
            handler.CreateParameter("PrecioVenta", SqlDbType.Decimal, data.SellingPrice, updateCommand);
            handler.CreateParameter("Stock", SqlDbType.BigInt, data.Stock, updateCommand);
            handler.CreateParameter("IdUsuario", SqlDbType.BigInt, data.UserId, updateCommand);

            foreach (SqlParameter par in updateCommand.Parameters)
            {
                if (par.SqlValue.ToString() != "Null")
                {
                    if (updateText != "") updateText = updateText + ",";
                    updateText = updateText + " " + par.ParameterName + "=@" + par.ParameterName;
                }
            }

            handler.CreateParameter("id", SqlDbType.BigInt, data.Id, updateCommand);

            updateCommand.CommandText = "update Producto set " + updateText + " where Id = @id";
            result = GenericCommand(updateCommand);

            return result;
        }
        public Boolean HandleDeleteProduct(Int32 id)
        {

            SqlCommand deleteCommand = new SqlCommand("delete Producto where Id = @id");
            handler.CreateParameter("id", SqlDbType.BigInt, id, deleteCommand);

            result = GenericCommand(deleteCommand);

            return result;
        }
    }
}
