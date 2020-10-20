using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading.Tasks;
using SystemCommonLibrary.Data.DataEntity;
using SystemCommonLibrary.Data.Manager;

namespace SystemCommonLibrary.AspNetCore.Controller
{
    [ApiController]
    public class EntityController<T> : ControllerBase where T : Entity
    {
        private DbType _dbType;
        private string _db;

        public EntityController(DbType dbType, string db)
        {
            _dbType = dbType;
            _db = db;
        }

        [HttpGet("api/" + nameof(T) + "/{id}")]
        public virtual async Task<IActionResult> Get(int id)
        {
            var entity = await DbEntityManager.SelectOne<T>(_dbType, _db, nameof(Entity.Id), id);
            return Ok(entity);
        }

        [HttpPost("api/" + nameof(T))]
        public virtual async Task<IActionResult> Post([FromBody]dynamic json)
        {
            T entity = json.Deserialize<T>();
            await DbEntityManager.Insert(_dbType, _db, entity);
            return Created($"api/{nameof(T)}/{entity.Id}", entity);
        }

        [HttpPatch("api/" + nameof(T) + "/{id}")]
        public virtual async Task<IActionResult> Patch(int id, [FromBody]dynamic json)
        {
            var entity = await DbEntityManager.SelectOne<T>(_dbType, _db, nameof(Entity.Id), id);

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

            await DbEntityManager.Update(_dbType, _db, entity);
            return Ok(entity);
        }

        [HttpDelete("api/" + nameof(T) + "/{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await DbEntityManager.Remove<T>(_dbType, _db, nameof(Entity.Id), id);
            return Ok();
        }

    }
}
