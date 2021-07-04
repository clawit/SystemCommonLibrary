using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemCommonLibrary.Data.Manager;

namespace SystemCommonLibrary.AspNetCore.Controller
{
    public static class QueryFilterHeler
    {
        public static QueryFilter Convert<T>(IQueryCollection collection)
        {
            if (collection == null || collection.Count == 0)
            {
                return null;
            }
            else if (collection.Count == 1)
            {
                var item = collection.Single();
                try
                {
                    var keyValue = GetTypedKeyValue<T>(item.Key, item.Value.First());
                    return new QueryFilter()
                    {
                        Key = keyValue.Item1,
                        Value = keyValue.Item2,
                        Comparison = QueryComparison.Equal
                    };
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                var query = new QueryFilter() { SubFilters = new List<QueryFilter>() };
                foreach (var item in collection)
                {
                    try
                    {
                        var keyValue = GetTypedKeyValue<T>(item.Key, item.Value.First());
                        var subFilter = new QueryFilter()
                        {
                            Key = keyValue.Item1,
                            Value = keyValue.Item2,
                            Comparison = QueryComparison.Equal,
                            Operator = QueryOperator.And
                        };
                        query.SubFilters.Add(subFilter);
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (query.SubFilters.Count == 0)
                    return null;
                else
                    return query;
            }
        }

        private static (string, object) GetTypedKeyValue<T>(string key, string value)
        {
            var prop = typeof(T).GetProperty(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop == null)
            {
                throw new ArgumentException();
            }
            var type = prop.PropertyType;
            if (type == typeof(int))
            {
                return (prop.Name, System.Convert.ToInt32(value));
            }
            else if (type == typeof(uint))
            {
                return (prop.Name, System.Convert.ToUInt32(value));
            }
            else if (type == typeof(decimal))
            {
                return (prop.Name, System.Convert.ToDecimal(value));
            }
            else if (type == typeof(DateTime))
            {
                return (prop.Name, System.Convert.ToDateTime(value));
            }
            else
            {
                return (prop.Name, value);
            }
        }
    }
}
