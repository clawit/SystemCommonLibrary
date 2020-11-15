using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Data.Helper
{
    public static class PgSqlHelper
    {
        public static async Task<object> ExecuteScalarAsync(string connectionString, string sql)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    return await command.ExecuteScalarAsync();
                }
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string sql)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string sql)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<T>(sql);
            }
        }

        public static async Task<object> ExecuteScalarAsync(IDbTransaction transaction, string sql)
        {
            var trans = transaction as NpgsqlTransaction;
            var connection = trans.Connection;

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection, trans))
            {
                return await command.ExecuteScalarAsync();
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(IDbTransaction transaction, string sql)
        {
            var trans = transaction as NpgsqlTransaction;
            var connection = trans.Connection;

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection, trans))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(IDbTransaction transaction, string sql)
        {
            var trans = transaction as NpgsqlTransaction;
            var connection = trans.Connection;

            return await connection.QueryAsync<T>(sql, transaction: trans);
        }
    }
}