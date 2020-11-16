using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Data.Helper
{
    public static class SqlServerHelper
    {
        public static async Task<object> ExecuteScalarAsync(string connectionString, string sql)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    return await command.ExecuteScalarAsync();
                }
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string sql)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string sql)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql);
            }
        }

        public static async Task<object> ExecuteScalarAsync(IDbTransaction transaction, string sql)
        {
            var trans = transaction as SqlTransaction;
            var connection = trans.Connection;

            using (SqlCommand command = new SqlCommand(sql, connection, trans))
            {
                return await command.ExecuteScalarAsync();
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(IDbTransaction transaction, string sql)
        {
            var trans = transaction as SqlTransaction;
            var connection = trans.Connection;

            using (SqlCommand command = new SqlCommand(sql, connection, trans))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(IDbTransaction transaction, string sql)
        {
            var trans = transaction as SqlTransaction;
            var connection = trans.Connection;

            return await connection.QueryAsync<T>(sql, transaction: trans);
        }
    }
}