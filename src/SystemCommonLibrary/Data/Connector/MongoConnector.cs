using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;

namespace SystemCommonLibrary.Data.Connector
{
    public static class MongoConnector
    {

        private static ConcurrentDictionary<string, IMongoClient> dicPool = new ConcurrentDictionary<string, IMongoClient>();

        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="connName">数据库名称</param>
        /// <returns></returns>
        public static IMongoClient Open(string connName = "mongo")
        {

            if (!dicPool.ContainsKey(connName))
            {
                string mongoConnectionStr = ConfigurationManager.ConnectionStrings[connName].ConnectionString;
                var conn = new MongoClient(mongoConnectionStr);
                dicPool.TryAdd(connName, conn);
            }
            return dicPool[connName];
        }

    }
}
