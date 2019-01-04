using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Smartiks.Framework.Data.Abstractions;
using Smartiks.Framework.Data.Extensions;

namespace Smartiks.Framework.Data.EntityFramework
{
    public abstract class ReadOnlyContextRepository<TContext, TQueryable, TListQuery, TId> : IReadOnlyQueryableService<TQueryable, TListQuery, TId>
        where TContext : DbContext
        where TQueryable : class, IId<TId>
        where TListQuery : Query<TQueryable>
        where TId : struct
    {
        protected TContext Context { get; }

        protected ReadOnlyContextRepository(TContext context)
        {
            Context = context;
        }

        protected abstract IQueryable<TQueryable> GetQueryable();

        public virtual async Task<TModel> GetAsync<TModel>(TId id, CancellationToken cancellationToken)
            where TModel : class
        {
            var queryable =
                GetQueryable()
                    .Where(q => q.Id.Equals(id));

            return await queryable.ProjectTo<TModel>().FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<QueryResult<TModel>> GetAsync<TModel>(TListQuery query, CancellationToken cancellationToken)
            where TModel : class
        {
            var queryResult = new QueryResult<TModel>();


            var queryable = GetQueryable();

            if (query != null)
            {
                if (query.Criteria != null && query.Criteria.Expression != null)
                {
                    queryable = queryable.Where(query.Criteria.Expression);
                }

                if (query.Orders != null)
                {
                    foreach (var queryOrder in query.Orders)
                    {
                        queryable = queryOrder.ApplyTo(queryable);
                    }
                }

                if (query.Segment != null)
                {
                    queryResult.TotalCount = await queryable.CountAsync(cancellationToken);

                    queryable =
                        queryable
                            .Skip(query.Segment.StartIndex)
                            .Take(query.Segment.Count ?? int.MaxValue);
                }
            }

            queryResult.Items = await queryable.ProjectTo<TModel>().ToListAsync(cancellationToken);

            return queryResult;
        }
    }
}