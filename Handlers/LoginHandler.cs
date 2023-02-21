using DamianGonzalezCSharp.Models;
using System.Data;
using System.Data.SqlClient;

namespace DamianGonzalezCSharp.Handlers
{
    public class LoginHandler : SqlHandler
    {
        private static ParameterHandler handler = new ParameterHandler();
        private Boolean result = false;
        public Boolean HandleLoginUser(DataSet ds, Login data)
        {
            SqlCommand cmd = new SqlCommand();

            handler.CreateParameter("userName", SqlDbType.Char, data.UserName, cmd);
            handler.CreateParameter("password", SqlDbType.Char, data.Password, cmd);
            cmd.CommandText = "select * from Usuario where NombreUsuario=@userName and Contraseña=@password";
            result = GetCommand(ds, cmd);

            return result;
        }

        public string HandleCreateToken()
        {
            return "hyjo";
        }

        public Boolean HandleGetToken(IHeaderDictionary headers)
        {
            string sToken = "";

            foreach (KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> item in headers)
            {
                if (item.Key == "token")
                {
                    sToken = item.Value.First();
                    sToken = sToken.Replace("Bearer", "").Trim();
                }
            }

            if (sToken == "hyjo") result = true;

            return result;
        }
    }
}
