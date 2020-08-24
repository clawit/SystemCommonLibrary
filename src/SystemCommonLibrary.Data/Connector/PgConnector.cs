using Npgsql;
using System;
using System.Configuration;
using System.Data;

namespace SystemCommonLibrary.Data.Connector
{
    /// <summary>
    /// mssql数据库访问器
    /// </summary>
    public static class PgConnector
    {
        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IDbConnection Open(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name)
                    || ConfigurationManager.ConnectionStrings[name] == null)
                {
                    Console.WriteLine("ConnectionStrings not found: " + name);
                    return null;
                }
                    
                IDbConnection _db = new NpgsqlConnection(ConfigurationManager.ConnectionStrings[name].ConnectionString);
                _db.Open();
                return _db;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
