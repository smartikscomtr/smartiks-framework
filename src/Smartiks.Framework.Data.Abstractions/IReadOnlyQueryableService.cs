using System.Threading;
using System.Threading.Tasks;

namespace Smartiks.Framework.Data.Abstractions
{
    public interface IReadOnlyQueryableService<TQueryable, TListQuery, TId>
        where TQueryable : class, IId<TId>
        where TListQuery : Query<TQueryable>
    {
        Task<TModel> GetAsync<TModel>(TId id, CancellationToken token)
            where TModel : class;

        Task<QueryResult<TModel>> GetAsync<TModel>(TListQuery query, CancellationToken token)
            where TModel : class;
    }
}
