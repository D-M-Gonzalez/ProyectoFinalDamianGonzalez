using DamianGonzalezCSharp.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace DamianGonzalesCSharp.Handlers
{
    public class UserHandler : SqlHandler
    {
        public Boolean HandleGetUsers(DataSet ds, Int32 userId, string sLike, string order)
        {
            SqlCommand cmd = new SqlCommand();
            Boolean success = false;

            if (userId > 0)
            {
                var parameter = new SqlParameter();
                parameter.ParameterName = "userId";
                parameter.SqlDbType = SqlDbType.BigInt;
                parameter.Value = userId;

                cmd.CommandText = "select * from Usuario where Id=@userId";
                cmd.Parameters.Add(parameter);

            } else
            {
                cmd.CommandText = "select * from Usuario ";
                if (sLike != "" && sLike != null)
                {
                    var parameter = new SqlParameter();
                    parameter.ParameterName = "descrip";
                    parameter.SqlDbType = SqlDbType.Char;
                    parameter.Value = "%" + sLike + "%";

                    cmd.CommandText = cmd.CommandText + " where Nombre like @descrip or Apellido like @descrip or NombreUsuario like @descrip";
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

            }
            
            success = GetCommand(ds, cmd);

            return success;
        }

        public Boolean HandleUserData(User data, Boolean insert)
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

            if (data.Name != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var name = new SqlParameter();
                name.ParameterName = "name";
                name.SqlDbType = SqlDbType.Char;
                name.Value = data.Name;
                sData = sData + (insert ? " @name " : " Nombre = @name ");
                createCommand.Parameters.Add(name);
            }

            if (data.Surname != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var surname = new SqlParameter();
                surname.ParameterName = "surname";
                surname.SqlDbType = SqlDbType.Char;
                surname.Value = data.Surname;
                sData = sData + (insert ? " @surname " : " Apellido = @surname ");
                createCommand.Parameters.Add(surname);
            }

            if (data.UserName != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var userName = new SqlParameter();
                userName.ParameterName = "userName";
                userName.SqlDbType = SqlDbType.Char;
                userName.Value = data.UserName;
                sData = sData + (insert ? " @userName " : " NombreUsuario = @userName ");
                createCommand.Parameters.Add(userName);
            }

            if (data.Password != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var password = new SqlParameter();
                password.ParameterName = "password";
                password.SqlDbType = SqlDbType.Char;
                password.Value = data.Password;
                sData = sData + (insert ? " @password " : " Contraseña = @password ");
                createCommand.Parameters.Add(password);
            }

            if (data.Email != null)
            {
                sData = sData != "" ? sData + " , " : "";
                var email = new SqlParameter();
                email.ParameterName = "email";
                email.SqlDbType = SqlDbType.Char;
                email.Value = data.Email;
                sData = sData + (insert ? " @email " : " Mail = @email ");
                createCommand.Parameters.Add(email);
            }

            createCommand.CommandText = insert == true ? "insert into Usuario (Nombre, Apellido, NombreUsuario, Contraseña, Mail) values ( " + sData + ")" : "update Usuario set " + sData + " where Id = @id";

            success = GenericCommand(createCommand);

            return success;
        }

        public Boolean HandleDeleteUser(Int32 id)
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
        }
    }
}
