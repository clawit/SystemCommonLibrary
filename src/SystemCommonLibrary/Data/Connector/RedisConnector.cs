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
        /// <param name="connName">数据库名称</param>
        /// <returns></returns>
        public static IConnectionMultiplexer Open(string connName = "redis")
        {
            lock (dicPool)
            {
                if (!dicPool.ContainsKey(connName))
                {
                    string redisConnectionStr = ConfigurationManager.ConnectionStrings[connName].ConnectionString;
                    var conn = ConnectionMultiplexer.Connect(redisConnectionStr);
                    dicPool.Add(connName, conn);
                }
                return dicPool[connName];
            }

          
        }
    }
}
