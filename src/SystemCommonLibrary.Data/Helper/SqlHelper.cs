using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemCommonLibrary.Data.Manager;

namespace SystemCommonLibrary.Data.Helper
{
    public static class SqlHelper
    {
        public static async Task<object> ExecuteScalarAsync(DbType type, string connectionString, string Sql)
        {
            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.ExecuteScalarAsync(connectionString, Sql); ;
                case DbType.SqlServer:
                    return await SqlServerHelper.ExecuteScalarAsync(connectionString, Sql); ;
                case DbType.PostgreSql:
                    return await PgSqlHelper.ExecuteScalarAsync(connectionString, Sql); ;

                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(DbType type, string connectionString, string Sql)
        {
            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.ExecuteNonQueryAsync(connectionString, Sql); ;
                case DbType.SqlServer:
                    return await SqlServerHelper.ExecuteNonQueryAsync(connectionString, Sql); ;
                case DbType.PostgreSql:
                    return await PgSqlHelper.ExecuteNonQueryAsync(connectionString, Sql); ;

                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(DbType type, string connectionString, string Sql)
        {
            switch (type)
            {
                case DbType.MySql:
                    return await MySqlHelper.QueryAsync<T>(connectionString, Sql); ;
                case DbType.SqlServer:
                    return await SqlServerHelper.QueryAsync<T>(connectionString, Sql); ;
                case DbType.PostgreSql:
                    return await PgSqlHelper.QueryAsync<T>(connectionString, Sql); ;

                default:
                    throw new NotImplementedException();
            }
        }

    }
}
