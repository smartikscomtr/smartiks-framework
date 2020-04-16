using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Smartiks.Framework.Data.Abstractions;

namespace Smartiks.Framework.Data.EntityFramework
{
    public class ContextRepository<TContext, TQueryable, TListQuery, TId> : ReadOnlyContextRepository<TContext, TQueryable, TListQuery, TId>, IQueryableService<TQueryable, TListQuery, TId>
        where TContext : DbContext
        where TQueryable : class, IId<TId>
        where TListQuery : Query<TQueryable>
        where TId : struct
    {
        protected DbSet<TQueryable> Set { get; }

        private readonly IMapper _mapper;

        public ContextRepository(TContext context, IMapper mapper) : base(context, mapper)
        {
            Set = context.Set<TQueryable>();

            _mapper = mapper;
        }

        protected override IQueryable<TQueryable> GetQueryable()
        {
            return Set.AsNoTracking();
        }

        public virtual async Task<TId> InsertAsync<TModel>(TModel model, CancellationToken cancellationToken)
            where TModel : class
        {
            var entity = _mapper.Map<TQueryable>(model);

            Set.Add(entity);

            await Context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public virtual async Task UpdateAsync<TModel>(TModel model, CancellationToken cancellationToken)
            where TModel : class, IId<TId>
        {
            var entity = await Set.FirstAsync(e => e.Id.Equals(model.Id), cancellationToken);

            _mapper.Map(model, entity);

            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TId id, CancellationToken cancellationToken)
        {
            var entity = await Set.FirstAsync(e => e.Id.Equals(id), cancellationToken);

            Context.Remove(entity);

            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}