using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamianGonzalesCSharp.Handlers
{
    public class SqlHandler
    {
        static string connectionString = "Data Source=DESKTOP-QR97TGF;Initial Catalog=SistemaGestion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private static SqlConnection connection = new SqlConnection(connectionString);
        private static SqlDataAdapter dataAdapter = new SqlDataAdapter();
        //private bool ValidLogin = false;
        public static Boolean GetCommand(DataSet ds, SqlCommand command)
        {
            Boolean success = false;
            connection.Open();
            command.Connection = connection;
            try
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(ds, "DAT");
                connection.Close();
                success = true;
            } catch (Exception)
            {
                connection.Close();
            }

            return success;
        }

        public bool GenericCommand(SqlCommand command)
        {
            bool result = false;

            try
            {
                connection.Open();
                command.Connection = connection;
                result = command.ExecuteNonQuery() > 0 ? true : false;
                connection.Close();
            } catch (Exception)
            {
                connection.Close();
            }


            return result;
        }

        public bool InsertCommand(SqlCommand command)
        {
            bool result = false;

            connection.Open();
            command.Connection = connection;
            result = command.ExecuteNonQuery() > 0 ? true : false;
            connection.Close();

            return result;
        }

        /*public bool LoginUser(string userName, string userPassword)
        {
            SqlCommand command = new SqlCommand("select * from Usuario where NombreUsuario=@userName and Contraseña=@userPassword", connection);
            DataSet ds = new DataSet();
            connection.Open();

            var parameterUser = new SqlParameter();
            parameterUser.ParameterName = "userName";
            parameterUser.SqlDbType = SqlDbType.VarChar;
            parameterUser.Value = userName;

            var parameterPass = new SqlParameter();
            parameterPass.ParameterName = "userPassword";
            parameterPass.SqlDbType = SqlDbType.VarChar;
            parameterPass.Value = userPassword;

            command.Parameters.Add(parameterUser);
            command.Parameters.Add(parameterPass);

            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(ds, "USER");
            connection.Close();

            if (ds.Tables.Contains("USER") && ds.Tables["USER"].Rows.Count > 0) ValidLogin = true;

            return ValidLogin;
        }*/
    }
}
