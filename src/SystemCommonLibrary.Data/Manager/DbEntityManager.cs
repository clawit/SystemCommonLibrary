using Renci.SshNet.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SystemCommonLibrary.Data.Helper;

namespace SystemCommonLibrary.Data.Manager
{
    public static class DbEntityManager
    {
        public static async Task<int> Insert<T>(DbType type, string db, T entity)
        {
            string sql = GenInsertSql(type, entity, out bool hasIdentity);
            if (hasIdentity)
            {
                var id = await SqlHelper.ExecuteScalarAsync(type, db, sql);
                var columns = typeof(T).GetProperties();
                foreach (var column in columns)
                {
                    bool isKey = column.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0;
                    if (isKey)
                    {
                        column.SetValue(entity, Convert.ToInt32(id));
                        break;
                    }
                }

                return Convert.ToInt32(id);
            }
            else 
            {
                return await SqlHelper.ExecuteNonQueryAsync(type, db, sql);
            }
        }

        public static async Task<bool> Exist<T>(DbType type, string db, string key, object keyVal)
        {
            string sql = GenExistSql<T>(type, key, keyVal);

            object result = await SqlHelper.ExecuteScalarAsync(type, db, sql);
            return result.ToString() != "0";
        }

        public static async Task<int> Update<T>(DbType type, string db, T entity, string key, object keyVal)
        {
            string sql = GenUpdateSql(type, entity, key, keyVal);

            return await SqlHelper.ExecuteNonQueryAsync(type, db, sql);
        }

        public static async Task<int> Update<T>(DbType type, string db, T entity)
        {
            string sql = GenUpdateSql(type, entity);

            return await SqlHelper.ExecuteNonQueryAsync(type, db, sql);
        }

        public static async Task<T> SelectOne<T>(DbType type, string db, string key, object keyVal)
        {
            string sql = GenSelectSql<T>(type, key, keyVal);
            var result = await SqlHelper.QueryAsync<T>(type, db, sql);

            if (result.Count() == 1)
            {
                return result.Single();
            }
            else
            {
                return default(T);
            }
        }

        public static async Task<IEnumerable<T>> Select<T>(DbType type, string db, string key, object keyVal)
        {
            string sql = GenSelectSql<T>(type, key, keyVal);
            return await Select<T>(type, db, sql);
        }

        public static async Task<IEnumerable<T>> Select<T>(DbType type, string db, Dictionary<string, object> keyVals)
        {
            string sql = GenSelectSql<T>(type, keyVals);
            return await Select<T>(type, db, sql);
        }

        public static async Task<IEnumerable<T>> Select<T>(DbType type, string db, string sql)
        {
            return await SqlHelper.QueryAsync<T>(type, db, sql);
        }

        #region Private Methods
        private static string GenSelectSql<T>(DbType type, string key, object keyVal)
        {
            string sql = $"select * from {QuoteKeyword(type, typeof(T).Name)} where {key}={FormatValue(type, keyVal)}";
            Console.WriteLine(sql);
            return sql;
        }

        private static string GenSelectSql<T>(DbType type, Dictionary<string, object> keyVals)
        {
            var where = keyVals.Select(kv => $"{kv.Key}={FormatValue(type, kv.Value)} ");
            string sql = $"select * from {QuoteKeyword(type, typeof(T).Name)} where {string.Join("and ", where)}";
            Console.WriteLine(sql);
            return sql;
        }

        private static string GenInsertSql<T>(DbType type, T entity, out bool hasIdentity)
        {
            List<string> lstCols = new List<string>();
            List<string> lstVals = new List<string>();
            hasIdentity = false;
            string identityKey = string.Empty;

            var columns = typeof(T).GetProperties();
            foreach (var column in columns)
            {
                bool isKey = column.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0;
                object val = column.GetValue(entity);
                if (!isKey)
                {
                    lstVals.Add(FormatValue(type, val));
                    lstCols.Add(column.Name);
                }
                else
                {
                    if (val is short || val is int || val is long)
                    {
                        hasIdentity = true;
                        identityKey = column.Name;
                    }
                }
            }

            string sql = $"insert into {QuoteKeyword(type, typeof(T).Name)} ( {string.Join(",", lstCols)} ) values( {string.Join(",", lstVals)} ) ";
            if (hasIdentity)
            {
                sql += GetLastIdentity(type, typeof(T).Name, identityKey);
            }
            Console.WriteLine(sql);
            return sql;
        }

        private static string GenExistSql<T>(DbType type, string key, object keyVal)
        {
            string sql = $"select count(1) from {QuoteKeyword(type, typeof(T).Name)} where {key} = {FormatValue(type, keyVal)}";
            Console.WriteLine(sql);
            return sql;
        }

        private static string GenUpdateSql<T>(DbType type, T entity, string key, object keyVal)
        {
            Dictionary<string, string> dicColVal = new Dictionary<string, string>();

            var columns = typeof(T).GetProperties();
            foreach (var column in columns)
            {
                bool isKey = column.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0;

                if (column.Name != key && !isKey)
                {
                    dicColVal.Add(column.Name, FormatValue(type, column.GetValue(entity)));
                }
            }

            string strUpdate = string.Empty;
            foreach (var col in dicColVal.Keys)
            {
                strUpdate += $"{col}={dicColVal[col]},";
            }

            string sql = $"update {QuoteKeyword(type, typeof(T).Name)} set {strUpdate.Substring(0, strUpdate.Length - 1)} where {key} = {FormatValue(type, keyVal)}";
            Console.WriteLine(sql);
            return sql;
        }

        private static string GenUpdateSql<T>(DbType type, T entity)
        {
            Dictionary<string, string> dicColVal = new Dictionary<string, string>();
            string key = string.Empty;
            object keyVal = null;
            var columns = typeof(T).GetProperties();
            foreach (var column in columns)
            {
                bool isKey = column.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0;

                if (!isKey)
                {
                    dicColVal.Add(column.Name, FormatValue(type, column.GetValue(entity)));
                }
                else
                {
                    key = column.Name;
                    keyVal = column.GetValue(entity);
                }
            }

            string strUpdate = string.Empty;
            foreach (var col in dicColVal.Keys)
            {
                strUpdate += $"{col}={dicColVal[col]},";
            }

            string sql = $"update {QuoteKeyword(type, typeof(T).Name)} set {strUpdate.Substring(0, strUpdate.Length - 1)} where {key} = {FormatValue(type, keyVal)}";
            Console.WriteLine(sql);
            return sql;
        }

        private static string FormatValue(DbType type, object val)
        {
            if (val == null)
            {
                return $"null";
            }
            else if (val is string)
                return QuoteString(type, $"{val}");
            else if (val is int || val is long || val is float || val is double || val is decimal)
                return $"{val}";
            else if (val is bool)
            {
                return $"{val.ToString().ToLower()}";
            }
            else if (val is Enum)
            {
                return $"{(int)val}";
            }
            else if (val is DateTime)
            {
                return QuoteString(type, $"{((DateTime)val).ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            else
                throw new NotSupportedException("Value Type Not Supported.");
        }

        private static string QuoteKeyword(DbType type, string keyword)
        {
            switch (type)
            {
                case DbType.MySql:
                    return $"`{keyword}`";
                case DbType.SqlServer:
                    return $"[{keyword}]";
                case DbType.PostgreSql:
                    return $"\"{keyword}\"";
                default:
                    return keyword;
            }
        }

        private static string QuoteString(DbType type, string value)
        {
            switch (type)
            {
                case DbType.MySql:
                    return $"'{value}'";
                case DbType.SqlServer:
                    return $"'{value}'";
                case DbType.PostgreSql:
                    return $"'{value}'";
                default:
                    return $"'{value}'";
            }
        }

        private static string GetLastIdentity(DbType type, string table, string key) 
        {
            switch (type)
            {
                case DbType.MySql:
                    return $";Select @@IDENTITY;";
                case DbType.SqlServer:
                    return $";Select @@IDENTITY;";
                case DbType.PostgreSql:
                    return $"RETURNING {key};";
                default:
                    return $"Select max({key}) from {QuoteKeyword(type, table)};";
            }
        }
        #endregion
    }
}
