using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Data.Helper
{
    public static class MySqlHelper
    {
        public static async Task<object> ExecuteScalarAsync(string connectionString, string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    return await command.ExecuteScalarAsync();
                }
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql);
            }
        }

        public static async Task<object> ExecuteScalarAsync(IDbTransaction transaction, string sql)
        {
            var trans = transaction as MySqlTransaction;
            var connection = trans.Connection;

            using (MySqlCommand command = new MySqlCommand(sql, connection, trans))
            {
                return await command.ExecuteScalarAsync();
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(IDbTransaction transaction, string sql)
        {
            var trans = transaction as MySqlTransaction;
            var connection = trans.Connection;
            using (MySqlCommand command = new MySqlCommand(sql, connection, trans))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(IDbTransaction transaction, string sql)
        {
            var trans = transaction as MySqlTransaction;
            var connection = trans.Connection;

            return await connection.QueryAsync<T>(sql, transaction: trans);
        }
    }
}