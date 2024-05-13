﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemCommonLibrary.Enums;

namespace SystemCommonLibrary.Data.Helper
{
    public static class SqlHelper
    {
        public static bool DebugMode { get; set; } = false;

        public static async Task<object> ExecuteScalarAsync(DbType type, string connectionString, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.ExecuteScalarAsync(connectionString, sql);
                case DbType.SqlServer:
                    return await SqlServerHelper.ExecuteScalarAsync(connectionString, sql);
                case DbType.PostgreSql:
                    return await PgSqlHelper.ExecuteScalarAsync(connectionString, sql);
                case DbType.Sqlite:
                    return await SqliteHelper.ExecuteScalarAsync(connectionString, sql);
                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(DbType type, string connectionString, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.ExecuteNonQueryAsync(connectionString, sql);
                case DbType.SqlServer:
                    return await SqlServerHelper.ExecuteNonQueryAsync(connectionString, sql);
                case DbType.PostgreSql:
                    return await PgSqlHelper.ExecuteNonQueryAsync(connectionString, sql);
                case DbType.Sqlite:
                    return await SqliteHelper.ExecuteNonQueryAsync(connectionString, sql);
                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(DbType type, string connectionString, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.QueryAsync<T>(connectionString, sql);
                case DbType.SqlServer:
                    return await SqlServerHelper.QueryAsync<T>(connectionString, sql);
                case DbType.PostgreSql:
                    return await PgSqlHelper.QueryAsync<T>(connectionString, sql);
                case DbType.Sqlite:
                    return await SqliteHelper.QueryAsync<T>(connectionString, sql);
                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<object> ExecuteScalarAsync(DbType type, System.Data.IDbTransaction transaction, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.ExecuteScalarAsync(transaction, sql);
                case DbType.SqlServer:
                    return await SqlServerHelper.ExecuteScalarAsync(transaction, sql);
                case DbType.PostgreSql:
                    return await PgSqlHelper.ExecuteScalarAsync(transaction, sql);
                case DbType.Sqlite:
                    return await SqliteHelper.ExecuteScalarAsync(transaction, sql);
                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(DbType type, System.Data.IDbTransaction transaction, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.ExecuteNonQueryAsync(transaction, sql);
                case DbType.SqlServer:
                    return await SqlServerHelper.ExecuteNonQueryAsync(transaction, sql);
                case DbType.PostgreSql:
                    return await PgSqlHelper.ExecuteNonQueryAsync(transaction, sql);
                case DbType.Sqlite:
                    return await SqliteHelper.ExecuteNonQueryAsync(transaction, sql);
                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(DbType type, System.Data.IDbTransaction transaction, string sql)
        {
            if (DebugMode)
            {
                Console.WriteLine(sql);
            }

            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.QueryAsync<T>(transaction, sql);
                case DbType.SqlServer:
                    return await SqlServerHelper.QueryAsync<T>(transaction, sql);
                case DbType.PostgreSql:
                    return await PgSqlHelper.QueryAsync<T>(transaction, sql);
                case DbType.Sqlite:
                    return await SqliteHelper.QueryAsync<T>(transaction, sql);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
