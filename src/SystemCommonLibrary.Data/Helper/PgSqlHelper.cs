using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Data.Helper
{
    public static class PgSqlHelper
    {
        public static async Task<object> ExecuteScalarAsync(string connectionString, string Sql)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(Sql, connection))
                {
                    return await command.ExecuteScalarAsync();
                }
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string Sql)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(Sql, connection))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string Sql)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<T>(Sql);
            }
        }
    }
}