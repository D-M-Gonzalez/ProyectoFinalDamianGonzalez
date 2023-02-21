using System.Data.SqlClient;
using System.Data;

namespace DamianGonzalezCSharp.Handlers
{
    public class SqlHandler
    {
        static string connectionString = "Data Source=DESKTOP-QR97TGF;Initial Catalog=SistemaGestion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private static SqlConnection connection = new SqlConnection(connectionString);
        private static SqlDataAdapter dataAdapter = new SqlDataAdapter();
        public static Boolean GetCommand(DataSet ds, SqlCommand command)
        {
            Boolean success = false;
            connection.Open();
            command.Connection = connection;
            try
            {
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(ds, "DAT");
                success = true;
            } catch (Exception)
            {
            }
            connection.Close();

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
            } catch (Exception)
            {
            }
            connection.Close();

            return result;
        }
    }
}
