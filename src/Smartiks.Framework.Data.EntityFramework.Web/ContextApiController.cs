using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smartiks.Framework.Data.Abstractions;

namespace Smartiks.Framework.Data.EntityFramework.Web
{
    [ApiController]
    public abstract class ContextApiController<TContext, TQueryable, TListModel, TGetModel, TCreateModel, TUpdateModel, TId> :
        ContextApiController<TContext, TQueryable, Query<TQueryable>, TListModel, TGetModel, TCreateModel, TUpdateModel, TId>
        where TContext : DbContext
        where TQueryable : class, IId<TId>
        where TListModel : class
        where TGetModel : class, IId<TId>
        where TCreateModel : class
        where TUpdateModel : class, IId<TId>
        where TId : struct
    {
        protected ContextApiController(ContextRepository<TContext, TQueryable, Query<TQueryable>, TId> repository) : base(repository)
        {
        }
    }

    [ApiController]
    public abstract class ContextApiController<TContext, TQueryable, TListQuery, TListModel, TGetModel, TCreateModel, TUpdateModel, TId> : ControllerBase
        where TContext : DbContext
        where TQueryable : class, IId<TId>
        where TListQuery : Query<TQueryable>
        where TListModel : class
        where TGetModel : class, IId<TId>
        where TCreateModel : class
        where TUpdateModel : class, IId<TId>
        where TId : struct
    {
        protected ContextRepository<TContext, TQueryable, TListQuery, TId> Repository { get; }

        public ContextApiController(ContextRepository<TContext, TQueryable, TListQuery, TId> repository)
        {
            Repository = repository;
        }

        [HttpGet("")]
        public virtual async Task<QueryResult<TListModel>> Get(TListQuery query)
        {
            return
                await Repository
                    .GetAsync<TListModel>(query, default(CancellationToken));
        }

        [HttpGet("{id}")]
        public virtual async Task<TGetModel> Get(TId id)
        {
            return
                await Repository
                    .GetAsync<TGetModel>(id, default(CancellationToken));
        }

        [HttpPost("")]
        public virtual async Task<IActionResult> Post([FromBody] TCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await Repository.InsertAsync(model, default(CancellationToken));

                return Ok(id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("")]
        public virtual async Task<IActionResult> Put([FromBody] TUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                await Repository.UpdateAsync(model, default(CancellationToken));

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TId id)
        {
            await Repository.DeleteAsync(id, default(CancellationToken));

            return NoContent();
        }
    }
}
