using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemCommonLibrary.Data.Manager;

namespace SystemCommonLibrary.Data.Helper
{
    public static class SqlHelper
    {
        public static async Task<object> ExecuteScalarAsync(DbType type, string connectionString, string sql)
        {
#if DEBUG
            Console.WriteLine(sql);
#endif
            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.ExecuteScalarAsync(connectionString, sql); ;
                case DbType.SqlServer:
                    return await SqlServerHelper.ExecuteScalarAsync(connectionString, sql); ;
                case DbType.PostgreSql:
                    return await PgSqlHelper.ExecuteScalarAsync(connectionString, sql); ;

                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(DbType type, string connectionString, string sql)
        {
#if DEBUG
            Console.WriteLine(sql);
#endif
            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.ExecuteNonQueryAsync(connectionString, sql); ;
                case DbType.SqlServer:
                    return await SqlServerHelper.ExecuteNonQueryAsync(connectionString, sql); ;
                case DbType.PostgreSql:
                    return await PgSqlHelper.ExecuteNonQueryAsync(connectionString, sql); ;

                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(DbType type, string connectionString, string sql)
        {
#if DEBUG
            Console.WriteLine(sql);
#endif
            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.QueryAsync<T>(connectionString, sql); ;
                case DbType.SqlServer:
                    return await SqlServerHelper.QueryAsync<T>(connectionString, sql); ;
                case DbType.PostgreSql:
                    return await PgSqlHelper.QueryAsync<T>(connectionString, sql); ;

                default:
                    throw new NotImplementedException();
            }
        }

    }
}
