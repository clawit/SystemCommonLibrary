﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Threading.Tasks;
using SystemCommonLibrary.Data.DataEntity;
using SystemCommonLibrary.Data.Manager;
using SystemCommonLibrary.Enums;
using SystemCommonLibrary.Json;

namespace SystemCommonLibrary.AspNetCore.Controller
{
    public class EntityControllerBase<T> : ControllerBase where T : Entity
    {
        protected DbType DbType { get; set; }
        protected string Db { get; set; }

        public EntityControllerBase(DbType dbType, string db)
        {
            DbType = dbType;
            Db = db;
        }

        [HttpGet]
        public virtual IActionResult Schema()
        {
            var schema = EntitySchemaReader.GetSchema<T>();
            return Ok(schema);
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get(int id)
        {
            var entity = await DbEntityManager.SelectOneAsync<T>(DbType, Db, nameof(Entity.Id), id);
            return Ok(entity);
        }

        [HttpGet]
        public virtual async Task<IActionResult> Query()
        {
            var query = QueryFilterHeler.Convert<T>(Request.Query);
            var entities = await DbEntityManager.SelectAsync<T>(DbType, Db, query);
            return Ok(entities);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post(dynamic json)
        {
            T entity = default(T);
            if (json is DynamicJson)
            {
                entity = json.Deserialize<T>();
            }
            else
            {
                entity = DynamicJson.Parse(json.ToString()).Deserialize<T>();
            }
            await DbEntityManager.InsertAsync(DbType, Db, entity);
            return Created($"api/{typeof(T).Name}/{entity.Id}", entity);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put(int id, dynamic json)
        {
            if (!(json is DynamicJson))
            {
                json = DynamicJson.Parse(json.ToString());
            }
            var entity = await DbEntityManager.SelectOneAsync<T>(DbType, Db, nameof(Entity.Id), id);

            var ps = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in ps)
            {
                var attrEditor = property.GetCustomAttribute(typeof(EditorAttribute)) as EditorAttribute;
                if (attrEditor != null && attrEditor.Editable)
                {
                    if (json.TryGetValue(property.Name, out object value))
                    {
                        if (property.PropertyType.IsEnum)
                        {
                            value = Enum.Parse(property.PropertyType, value.ToString());
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            value = Convert.ToBoolean(value);
                        }
                        else if (property.PropertyType == typeof(short))
                        {
                            value = Convert.ToInt16(value);
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            value = Convert.ToInt32(value);
                        }
                        else if (property.PropertyType == typeof(long))
                        {
                            value = Convert.ToInt64(value);
                        }
                        else if (property.PropertyType == typeof(float))
                        {
                            value = Convert.ToSingle(value);
                        }
                        else if (property.PropertyType == typeof(double))
                        {
                            value = Convert.ToDouble(value);
                        }
                        else if (property.PropertyType == typeof(decimal))
                        {
                            value = Convert.ToDecimal(value);
                        }
                        else if (property.PropertyType == typeof(DateTime))
                        {
                            value = Convert.ToDateTime(value);
                        }

                        property.SetValue(entity, value);
                    }
                }
            }

            await DbEntityManager.UpdateAsync(DbType, Db, entity);
            return Ok(entity);
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await DbEntityManager.RemoveAsync<T>(DbType, Db, nameof(Entity.Id), id);
            return Ok();
        }

    }
}
