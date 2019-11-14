using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SystemCommonLibrary.Data.Connector
{
    /// <summary>
    /// mssql数据库访问器
    /// </summary>
    public static class SqlConnector
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
                IDbConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings[name].ConnectionString);
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
