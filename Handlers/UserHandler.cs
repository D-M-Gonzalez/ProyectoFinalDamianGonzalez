using DamianGonzalezCSharp.Models;
using System.Data;
using System.Data.SqlClient;

namespace DamianGonzalezCSharp.Handlers
{
    public class UserHandler : SqlHandler
    {
        private static ParameterHandler handler = new ParameterHandler();
        private Boolean result = false;
        public Boolean HandleGetUsers(DataSet ds, Int32 userId)
        {
            SqlCommand cmd = new SqlCommand();

            handler.CreateParameter("userId", SqlDbType.BigInt, userId, cmd);
            cmd.CommandText = "select * from Usuario where Id=@userId";
            result = GetCommand(ds, cmd);

            return result;
        }
        public Boolean HandleGetUsers(DataSet ds, string userName)
        {
            SqlCommand cmd = new SqlCommand();

            handler.CreateParameter("userName", SqlDbType.Char, userName, cmd);
            cmd.CommandText = "select * from Usuario where NombreUsuario=@userName";
            result = GetCommand(ds, cmd);

            return result;
        }
        public Boolean HandleGetUsers(DataSet ds, string sLike, string order)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from Usuario ";

            handler.CreateParameter("descrip", SqlDbType.Char, sLike, cmd, true);
            if (sLike != "") cmd.CommandText = cmd.CommandText + " where Nombre like @descrip or Apellido like @descrip or NombreUsuario like @descrip";

            if (order != "" && order != null)
            {
                string sOrder = "";
                switch (order)
                {
                    case "id":
                        sOrder = "Id";
                        break;
                    case "nombre":
                        sOrder = "Nombre";
                        break;
                    case "apellido":
                        sOrder = "Apellido";
                        break;
                    case "mail":
                        sOrder = "Mail";
                        break;
                    default:
                        sOrder = "NombreUsuario";
                        break;
                }
                cmd.CommandText = cmd.CommandText + " order by " + sOrder;
            }

            result = GetCommand(ds, cmd);

            return result;
        }
        public Boolean HandleCreateUser(User data)
        {
            SqlCommand createCommand = new SqlCommand();
            handler.CreateParameter("name", SqlDbType.Char, data.Name, createCommand);
            handler.CreateParameter("surname", SqlDbType.Char, data.Surname, createCommand);
            handler.CreateParameter("username", SqlDbType.Char, data.UserName, createCommand);
            handler.CreateParameter("password", SqlDbType.Char, data.Password, createCommand);
            handler.CreateParameter("mail", SqlDbType.Char, data.Email, createCommand);


            createCommand.CommandText = "insert into Usuario (Nombre, Apellido, NombreUsuario, Contraseña, Mail) values (@name,@surname,@username,@password,@mail)";
            result = GenericCommand(createCommand);

            return result;
        }
        public Boolean HandleUpdateUser(User data)
        {
            string updateText = "";
            SqlCommand updateCommand = new SqlCommand();
            handler.CreateParameter("Nombre", SqlDbType.Char, data.Name, updateCommand);
            handler.CreateParameter("Apellido", SqlDbType.Char, data.Surname, updateCommand);
            handler.CreateParameter("Contraseña", SqlDbType.Char, data.Password, updateCommand);
            handler.CreateParameter("Mail", SqlDbType.Char, data.Email, updateCommand);

            foreach (SqlParameter par in updateCommand.Parameters)
            {
                if (par.SqlValue.ToString() != "Null")
                {
                    if (updateText != "") updateText = updateText + ",";
                    updateText = updateText + " " + par.ParameterName + "=@" + par.ParameterName;
                }
            }

            handler.CreateParameter("NombreUsuario", SqlDbType.Char, data.UserName, updateCommand);

            updateCommand.CommandText = "update Usuario set " + updateText + " where NombreUsuario = @NombreUsuario";
            result = GenericCommand(updateCommand);

            return result;
        }
        public Boolean HandleDeleteUser(Int32 id)
        {
            SqlCommand deleteCommand = new SqlCommand("delete Usuario where Id = @id");
            handler.CreateParameter("id", SqlDbType.BigInt, id, deleteCommand);

            result = GenericCommand(deleteCommand);

            return result;
        }

        public Boolean HandleUpdatePassword(UserPassword data)
        {
            SqlCommand updateCommand = new SqlCommand();

            handler.CreateParameter("Contraseña", SqlDbType.Char, data.NewPassword, updateCommand);
            handler.CreateParameter("NombreUsuario", SqlDbType.Char, data.UserName, updateCommand);

            updateCommand.CommandText = "update Usuario set Contraseña = @Contraseña where NombreUsuario = @NombreUsuario";
            result = GenericCommand(updateCommand);

            return result;
        }
    }
}
