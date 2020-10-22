using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading.Tasks;
using SystemCommonLibrary.Data.DataEntity;
using SystemCommonLibrary.Data.Manager;
using SystemCommonLibrary.Json;

namespace SystemCommonLibrary.AspNetCore.Controller
{
    public class EntityController<T> : ControllerBase where T : Entity
    {
        protected DbType DbType { get; set; }
        protected string Db { get; set; }

        public EntityController(DbType dbType, string db)
        {
            DbType = dbType;
            Db = db;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get([FromRoute]int id)
        {
            var entity = await DbEntityManager.SelectOne<T>(DbType, Db, nameof(Entity.Id), id);
            return Ok(entity);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody]dynamic json)
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
            await DbEntityManager.Insert(DbType, Db, entity);
            return Created($"api/{nameof(T)}/{entity.Id}", entity);
        }

        [HttpPatch]
        public virtual async Task<IActionResult> Patch([FromRoute]int id, [FromBody]dynamic json)
        {
            if (!(json is DynamicJson))
            {
                json = DynamicJson.Parse(json.ToString());
            }
            var entity = await DbEntityManager.SelectOne<T>(DbType, Db, nameof(Entity.Id), id);

            var ps = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in ps)
            {
                if (property.Name != nameof(Entity.Id)
                    && property.Name != nameof(Entity.CreateAt)
                    && property.Name != nameof(Entity.Creator))
                {
                    if (json.TryGetValue(property.Name, out object value))
                    {
                        property.SetValue(entity, value);
                    }
                }
            }

            await DbEntityManager.Update(DbType, Db, entity);
            return Ok(entity);
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromRoute]int id)
        {
            await DbEntityManager.Remove<T>(DbType, Db, nameof(Entity.Id), id);
            return Ok();
        }

    }
}
