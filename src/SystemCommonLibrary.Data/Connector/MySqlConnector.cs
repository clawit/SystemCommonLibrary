using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//本类名与驱动包的命名空间 MySqlConnector 相同, 用别名引用驱动的连接类型
using MySqlConnection = global::MySqlConnector.MySqlConnection;

namespace SystemCommonLibrary.Data.Connector
{
    /// <summary>
    /// mysql数据库访问器
    /// </summary>
    public static class MySqlConnector
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
                IDbConnection _db = new MySqlConnection(ConfigurationManager.ConnectionStrings[name].ConnectionString);
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
