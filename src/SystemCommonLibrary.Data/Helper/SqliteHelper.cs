using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Data.Helper
{
    public static class SqliteHelper
    {
        public static async Task<object> ExecuteScalarAsync(string connectionString, string sql)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    return await command.ExecuteScalarAsync();
                }
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string sql)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string sql, int? commandTimeout = null)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql, commandTimeout);
            }
        }

        public static async Task<object> ExecuteScalarAsync(IDbTransaction transaction, string sql)
        {
            var trans = transaction as SQLiteTransaction2;
            var connection = trans.Connection;

            using (SQLiteCommand command = new SQLiteCommand(sql, connection, trans))
            {
                return await command.ExecuteScalarAsync();
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(IDbTransaction transaction, string sql)
        {
            var trans = transaction as SQLiteTransaction2;
            var connection = trans.Connection;

            using (SQLiteCommand command = new SQLiteCommand(sql, connection, trans))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(IDbTransaction transaction, string sql)
        {
            var trans = transaction as SQLiteTransaction2;
            var connection = trans.Connection;

            return await connection.QueryAsync<T>(sql, transaction: trans);
        }
    }
}