using Dapper;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading.Tasks;
using DbType = SystemCommonLibrary.Enums.DbType;

namespace SystemCommonLibrary.Data.Manager
{
    public static class SqlHelper
    {
        public static bool DebugMode { get; set; } = false;

        public static IDbConnection CreateConnection(DbType type, string connectionString)
        {
            switch (type)
            {
                case DbType.MySql:
                    return new MySqlConnection(connectionString);
                case DbType.SqlServer:
                    return new SqlConnection(connectionString);
                case DbType.PostgreSql:
                    return new NpgsqlConnection(connectionString);
                case DbType.Sqlite:
                    return new SQLiteConnection(connectionString);
                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<object> ExecuteScalarAsync(DbType type, string connectionString, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            using (var connection = CreateConnection(type, connectionString))
            {
                return await connection.ExecuteScalarAsync(sql);
            }
        }

        public static async Task<int> ExecuteAsync(DbType type, string connectionString, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            using (var connection = CreateConnection(type, connectionString))
            {
                return await connection.ExecuteAsync(sql);
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(DbType type, string connectionString, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            using (var connection = CreateConnection(type, connectionString))
            {
                return await connection.QueryAsync<T>(sql);
            }
        }

        public static async Task<object> ExecuteScalarAsync(DbType type, IDbTransaction transaction, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }
            return await transaction.Connection.ExecuteScalarAsync(sql, transaction);
        }

        public static async Task<int> ExecuteNonQueryAsync(DbType type, IDbTransaction transaction, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            return await transaction.Connection.ExecuteAsync(sql, transaction);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(DbType type, IDbTransaction transaction, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            return await transaction.Connection.QueryAsync<T>(sql, transaction);
        }
    }
}
