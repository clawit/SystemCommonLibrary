using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Data.Helper
{
    public static class MySqlHelper
    {
        public static async Task<object> ExecuteScalarAsync(string connectionString, string Sql)
        {
            return await MySql.Data.MySqlClient.MySqlHelper.ExecuteScalarAsync(connectionString, Sql); ;
        }

        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string Sql)
        {
            return await MySql.Data.MySqlClient.MySqlHelper.ExecuteNonQueryAsync(connectionString, Sql); ;
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string Sql)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                return await connection.QueryAsync<T>(Sql);
            }
        }
    }
}