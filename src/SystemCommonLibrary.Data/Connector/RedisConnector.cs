using StackExchange.Redis;
using System.Configuration;
using System.Collections.Generic;

namespace SystemCommonLibrary.Data.Connector
{
    public static class RedisConnector
    {

        //保存连接以复用
        //ref:https://stackexchange.github.io/StackExchange.Redis/Basics
        private static Dictionary<string, ConnectionMultiplexer> dicPool = new Dictionary<string, ConnectionMultiplexer>();

        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="connStr">数据库连接</param>
        /// <returns></returns>
        public static IConnectionMultiplexer Open(string connStr = "redis")
        {
            lock (dicPool)
            {
                if (!dicPool.ContainsKey(connStr))
                {
                    var conn = ConnectionMultiplexer.Connect(connStr);
                    dicPool.Add(connStr, conn);
                }
                return dicPool[connStr];
            }
        }

        /// <summary>
        /// 打开连接,获得数据库
        /// </summary>
        /// <param name="connStr">数据库连接</param>
        /// <param name="db">数据库index</param>
        /// <returns></returns>
        public static IDatabase Open(string connStr, int db = -1)
        {
            return Open(connStr).GetDatabase(db);
        }
    }
}
