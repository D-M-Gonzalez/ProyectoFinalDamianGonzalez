using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Data;
using System.Data.SqlClient;

namespace DamianGonzalezCSharp.Handlers
{
    public class ParameterHandler
    {
        public SqlParameter CreateParameter(string name, SqlDbType type, string? value, SqlCommand command, Boolean like)
        {
            SqlParameter sqlParameter = new SqlParameter();

            string likeValue = "%" + value + "%";

            CreateParameter(name, type, likeValue, command);

            return sqlParameter;
        }
        public SqlParameter CreateParameter(string name, SqlDbType type, string? value, SqlCommand command)
        {
            SqlParameter sqlParameter = new SqlParameter();

            sqlParameter.ParameterName = name;
            sqlParameter.SqlDbType = type;
            sqlParameter.Value = value != null ? value : DBNull.Value;
            command.Parameters.Add(sqlParameter);

            return sqlParameter;
        }

        public SqlParameter CreateParameter(string name, SqlDbType type, int? value, SqlCommand command)
        {
            return CreateParameter(name, type, value?.ToString(), command);
        }

        public SqlParameter CreateParameter(string name, SqlDbType type, decimal? value, SqlCommand command)
        {
            return CreateParameter(name, type, value?.ToString(), command);
        }
    }
}
