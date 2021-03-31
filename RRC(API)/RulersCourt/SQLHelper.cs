using System;
using System.Data;
using System.Data.SqlClient;

namespace RulersCourt
{
    public static class SqlHelper
    {
        public static TData ExecuteProcedureReturnData<TData>(string connString, string procName, Func<SqlDataReader, TData> translator, params SqlParameter[] parameters)
        {
            using (var sqlConnection = new SqlConnection(connString))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = procName;
                    if (parameters != null)
                    {
                        sqlCommand.Parameters.AddRange(parameters);
                    }

                    sqlConnection.Open();
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        TData elements;
                        try
                        {
                            elements = translator(reader);
                        }
                        finally
                        {
                            while (reader.NextResult())
                            { }
                        }

                        return elements;
                    }
                }
            }
        }

        public static System.Threading.Tasks.Task<TData> ExecuteProcedureReturnDataAsync<TData>(string connString, string procName, Func<SqlDataReader, TData> translator, params SqlParameter[] parameters)
        {
            return System.Threading.Tasks.Task.Run(async () =>
            {
                using (var sqlConnection = new SqlConnection(connString))
                {
                    using (var sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.CommandText = procName;
                        if (parameters != null)
                        {
                            sqlCommand.Parameters.AddRange(parameters);
                        }

                        await sqlConnection.OpenAsync();
                        using (var reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            TData elements;
                            try
                            {
                                elements = translator(reader);
                            }
                            finally
                            {
                                while (reader.NextResult())
                                { }
                            }

                            return elements;
                        }
                    }
                }
            });
        }

        public static string ExecuteProcedureReturnString(string connString, string procName, params SqlParameter[] paramters)
        {
            string result = string.Empty;
            using (var sqlConnection = new SqlConnection(connString))
            {
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = procName;
                    command.CommandTimeout = 0;
                    if (paramters != null)
                    {
                        command.Parameters.AddRange(paramters);
                    }

                    sqlConnection.Open();
                    var ret = command.ExecuteScalar();
                    if (ret != null)
                    {
                        result = Convert.ToString(ret);
                    }
                }
            }

            return result;
        }

        public static string ExecuteQueryReturnString(string connString, string query, params SqlParameter[] paramters)
        {
            string result = string.Empty;
            using (var sqlConnection = new SqlConnection(connString))
            {
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;
                    if (paramters != null)
                    {
                        command.Parameters.AddRange(paramters);
                    }

                    sqlConnection.Open();
                    var ret = command.ExecuteScalar();
                    if (ret != null)
                    {
                        result = Convert.ToString(ret);
                    }
                }
            }

            return result;
        }

        public static string GetNullableString(SqlDataReader reader, string colName)
        {
            return reader.IsDBNull(reader.GetOrdinal(colName)) ? null : Convert.ToString(reader[colName]);
        }

        public static int? GetNullableInt32(SqlDataReader reader, string colName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(colName)))
            {
                return Convert.ToInt32(reader[colName]);
            }

            return null;
        }

        public static bool? GetBoolean(SqlDataReader reader, string colName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(colName)))
            {
                return Convert.ToBoolean(reader[colName]);
            }

            return null;
        }

        public static DateTime? GetDateTime(SqlDataReader reader, string colName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(colName)))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(reader[colName]).ToLocalTime().ToUniversalTime();
            }
        }

        public static decimal? GetDecimal(SqlDataReader reader, string colName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(colName)))
            {
                return Convert.ToDecimal(reader[colName]);
            }

            return null;
        }

        public static long? GetLong(SqlDataReader reader, string colName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(colName)))
            {
                return Convert.ToInt64(reader[colName]);
            }

            return null;
        }

        public static bool IsColumnExists(this System.Data.IDataRecord dr, string colName)
        {
            try
            {
                return dr.GetOrdinal(colName) >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}