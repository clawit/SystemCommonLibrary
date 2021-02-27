using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SystemCommonLibrary.Data.Helper;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTest")]
namespace SystemCommonLibrary.Data.Manager
{
    public static class DbEntityManager
    {
        #region DB Methods

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

        public static async Task<int> Update(DbType type, string db, string sql)
        {
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

        public static async Task<T> SelectOne<T>(DbType type, string db, QueryFilter queryFilter)
        {
            string filterSql = GenFilterSql(type, queryFilter);
            string sql = GenSelectSql<T>(type);
            if (!string.IsNullOrEmpty(filterSql))
            {
                sql += " where " + filterSql;
            }
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

        public static async Task<T> SelectOne<T>(DbType type, string db, string sql)
        {
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

        public static async Task<object> SelectScalar(DbType type, string db, string sql)
        {
            return await SqlHelper.ExecuteScalarAsync(type, db, sql);
        }

        public static async Task<object> SelectScalar<T>(DbType type, string db, string col, Dictionary<string, object> keyVals)
        {
            var sql = GenScalaSql<T>(type, col, keyVals);
            return await SqlHelper.ExecuteScalarAsync(type, db, sql);
        }
        public static async Task<IEnumerable<T>> Select<T>(DbType type, string db)
        {
            string sql = GenSelectSql<T>(type);
            return await Select<T>(type, db, sql);
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

        public static async Task<IEnumerable<T>> Select<T>(DbType type, string db, QueryFilter queryFilter)
        {
            string filterSql = GenFilterSql(type, queryFilter);
            string sql = GenSelectSql<T>(type);
            if (!string.IsNullOrEmpty(filterSql))
            {
                sql += " where " + filterSql;
            }
            return await Select<T>(type, db, sql);
        }

        public static async Task<IEnumerable<T>> Select<T>(DbType type, string db, string sql)
        {
            return await SqlHelper.QueryAsync<T>(type, db, sql);
        }

        public static async Task<int> Remove(DbType type, string db, string sql)
        {
            return await SqlHelper.ExecuteNonQueryAsync(type, db, sql);
        }

        public static async Task<int> Remove<T>(DbType type, string db, string key, object keyVal)
        {
            string sql = GenRemoveSql<T>(type, key, keyVal);
            return await Remove(type, db, sql);
        }

        #endregion

        #region Transaction Methods
        public static async Task<int> Insert<T>(DbType type, System.Data.IDbTransaction transaction, T entity)
        {
            string sql = GenInsertSql(type, entity, out bool hasIdentity);
            if (hasIdentity)
            {
                var id = await SqlHelper.ExecuteScalarAsync(type, transaction, sql);
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
                return await SqlHelper.ExecuteNonQueryAsync(type, transaction, sql);
            }
        }

        public static async Task<bool> Exist<T>(DbType type, System.Data.IDbTransaction transaction, string key, object keyVal)
        {
            string sql = GenExistSql<T>(type, key, keyVal);

            object result = await SqlHelper.ExecuteScalarAsync(type, transaction, sql);
            return result.ToString() != "0";
        }

        public static async Task<int> Update<T>(DbType type, System.Data.IDbTransaction transaction, T entity, string key, object keyVal)
        {
            string sql = GenUpdateSql(type, entity, key, keyVal);

            return await SqlHelper.ExecuteNonQueryAsync(type, transaction, sql);
        }

        public static async Task<int> Update<T>(DbType type, System.Data.IDbTransaction transaction, T entity)
        {
            string sql = GenUpdateSql(type, entity);

            return await SqlHelper.ExecuteNonQueryAsync(type, transaction, sql);
        }

        public static async Task<int> Update(DbType type, System.Data.IDbTransaction transaction, string sql)
        {
            return await SqlHelper.ExecuteNonQueryAsync(type, transaction, sql);
        }

        public static async Task<T> SelectOne<T>(DbType type, System.Data.IDbTransaction transaction, string key, object keyVal)
        {
            string sql = GenSelectSql<T>(type, key, keyVal);
            var result = await SqlHelper.QueryAsync<T>(type, transaction, sql);

            if (result.Count() == 1)
            {
                return result.Single();
            }
            else
            {
                return default(T);
            }
        }

        public static async Task<T> SelectOne<T>(DbType type, System.Data.IDbTransaction transaction, QueryFilter queryFilter)
        {
            string filterSql = GenFilterSql(type, queryFilter);
            string sql = GenSelectSql<T>(type);
            if (!string.IsNullOrEmpty(filterSql))
            {
                sql += " where " + filterSql;
            }
            var result = await SqlHelper.QueryAsync<T>(type, transaction, sql);

            if (result.Count() == 1)
            {
                return result.Single();
            }
            else
            {
                return default(T);
            }
        }


        public static async Task<T> SelectOne<T>(DbType type, System.Data.IDbTransaction transaction, string sql)
        {
            var result = await SqlHelper.QueryAsync<T>(type, transaction, sql);

            if (result.Count() == 1)
            {
                return result.Single();
            }
            else
            {
                return default(T);
            }
        }

        public static async Task<object> SelectScalar(DbType type, System.Data.IDbTransaction transaction, string sql)
        {
            return await SqlHelper.ExecuteScalarAsync(type, transaction, sql);
        }

        public static async Task<object> SelectScalar<T>(DbType type, System.Data.IDbTransaction transaction, string col, Dictionary<string, object> keyVals)
        {
            var sql = GenScalaSql<T>(type, col, keyVals);
            return await SqlHelper.ExecuteScalarAsync(type, transaction, sql);
        }
        public static async Task<IEnumerable<T>> Select<T>(DbType type, System.Data.IDbTransaction transaction)
        {
            string sql = GenSelectSql<T>(type);
            return await Select<T>(type, transaction, sql);
        }

        public static async Task<IEnumerable<T>> Select<T>(DbType type, System.Data.IDbTransaction transaction, string key, object keyVal)
        {
            string sql = GenSelectSql<T>(type, key, keyVal);
            return await Select<T>(type, transaction, sql);
        }

        public static async Task<IEnumerable<T>> Select<T>(DbType type, System.Data.IDbTransaction transaction, Dictionary<string, object> keyVals)
        {
            string sql = GenSelectSql<T>(type, keyVals);
            return await Select<T>(type, transaction, sql);
        }

        public static async Task<IEnumerable<T>> Select<T>(DbType type, System.Data.IDbTransaction transaction, QueryFilter queryFilter)
        {
            string filterSql = GenFilterSql(type, queryFilter);
            string sql = GenSelectSql<T>(type);
            if (!string.IsNullOrEmpty(filterSql))
            {
                sql += " where " + filterSql;
            }
            return await Select<T>(type, transaction, sql);
        }

        public static async Task<IEnumerable<T>> Select<T>(DbType type, System.Data.IDbTransaction transaction, string sql)
        {
            return await SqlHelper.QueryAsync<T>(type, transaction, sql);
        }

        public static async Task<int> Remove(DbType type, System.Data.IDbTransaction transaction, string sql)
        {
            return await SqlHelper.ExecuteNonQueryAsync(type, transaction, sql);
        }

        public static async Task<int> Remove<T>(DbType type, System.Data.IDbTransaction transaction, string key, object keyVal)
        {
            string sql = GenRemoveSql<T>(type, key, keyVal);
            return await Remove(type, transaction, sql);
        }
        #endregion

        #region Private Methods
        private static string GenSelectSql<T>(DbType type)
        {
            string sql = $"select * from {QuoteKeyword(type, typeof(T).Name)}";
            return sql;
        }
        private static string GenSelectSql<T>(DbType type, string key, object keyVal)
        {
            string sql = $"{GenSelectSql<T>(type)} where {key}={FormatValue(type, keyVal)}";
            return sql;
        }

        private static string GenSelectSql<T>(DbType type, Dictionary<string, object> keyVals)
        {
            var where = keyVals.Select(kv => $"{kv.Key}={FormatValue(type, kv.Value)} ");
            string sql = $"{GenSelectSql<T>(type)} where {string.Join("and ", where)}";
            return sql;
        }

        private static string GenScalaSql<T>(DbType type, string col, Dictionary<string, object> keyVals)
        {
            var where = keyVals.Select(kv => $"{kv.Key}={FormatValue(type, kv.Value)} ");
            string sql = $"select {col} from {QuoteKeyword(type, typeof(T).Name)} where {string.Join("and ", where)}";
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
            return sql;
        }

        private static string GenExistSql<T>(DbType type, string key, object keyVal)
        {
            string sql = $"select count(1) from {QuoteKeyword(type, typeof(T).Name)} where {key} = {FormatValue(type, keyVal)}";
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
            return sql;
        }

        private static string GenRemoveSql<T>(DbType type, string key, object keyVal)
        {
            string sql = $"delete from {QuoteKeyword(type, typeof(T).Name)} where {key}={FormatValue(type, keyVal)}";
            return sql;
        }

        internal static string GenFilterSql(DbType type, QueryFilter queryFilter)
        {
            if (queryFilter == null)
            {
                return string.Empty;
            }
            else if (queryFilter.SubFilters == null || queryFilter.SubFilters.Count == 0)
            {
                if (string.IsNullOrEmpty(queryFilter.Key))
                {
                    throw new ArgumentException();
                }
                var key = QuoteKeyword(type, queryFilter.Key);
                var comparison = GenQueryComparison(queryFilter.Comparison);
                var val = FormatValue(type, queryFilter.Value, queryFilter.Comparison);

                return $"({key} {comparison} {val})";
            }
            else
            {
                string conditions = "(";
                for (int i = 0; i < queryFilter.SubFilters.Count; i++)
                {
                    var subFilter = queryFilter.SubFilters[i];
                    if (i != 0)
                    {
                        conditions += $" {subFilter.Operator} ";
                    }
                    conditions += GenFilterSql(type, subFilter);
                }
                conditions += ")";

                return conditions;
            }
        }

        private static string GenQueryComparison(QueryComparison comparison)
        {
            switch (comparison)
            {
                case QueryComparison.Equal:
                    return "=";
                case QueryComparison.Greater:
                    return ">";
                case QueryComparison.Less:
                    return "<";
                case QueryComparison.GreaterEqual:
                    return ">=";
                case QueryComparison.LessEqual:
                    return "<=";
                case QueryComparison.NotEqual:
                    return "<>";
                case QueryComparison.Like:
                    return "like";
                default:
                    throw new NotImplementedException();
            }
        }

        private static string FormatValue(DbType type, object val, QueryComparison comparison = QueryComparison.Equal)
        {
            if (val == null)
            {
                return $"null";
            }
            else if (val is string)
            {
                val = EscapeQuote(type, (string)val);
                if (comparison == QueryComparison.Like)
                {
                    return QuoteString(type, $"%{val}%");
                }
                else
                {
                    return QuoteString(type, $"{val}");
                }
            }
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
                case DbType.Sqlite:
                    return $"`{keyword}`";
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
                case DbType.Sqlite:
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
                case DbType.Sqlite:
                    return $";Select last_insert_rowid();";
                default:
                    return $"Select max({key}) from {QuoteKeyword(type, table)};";
            }
        }

        private static string EscapeQuote(DbType type, string value)
        {
            switch (type)
            {
                case DbType.MySql:
                    value = value.Replace("\\", "\\\\")
                        .Replace("\"", "\\\"")
                        .Replace("\'", "\\\'");
                    break;
                case DbType.SqlServer:
                    value = value.Replace("\'", "\'\'");
                    break;
                case DbType.PostgreSql:
                    value = value.Replace("\'", "\'\'");
                    break;
                case DbType.Sqlite:
                    value = value.Replace("\'", "\'\'");
                    break;
                default:
                    break;
            }

            return value;
        }
        
        #endregion
    }
}
