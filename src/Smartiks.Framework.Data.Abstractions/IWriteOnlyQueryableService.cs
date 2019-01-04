using System.Threading;
using System.Threading.Tasks;

namespace Smartiks.Framework.Data.Abstractions
{
    public interface IWriteOnlyQueryableService<TQueryable, TId>
        where TQueryable : class, IId<TId>
    {
        Task<TId> InsertAsync<TModel>(TModel model, CancellationToken token)
            where TModel : class;

        Task UpdateAsync<TModel>(TModel model, CancellationToken token)
            where TModel : class, IId<TId>;

        Task DeleteAsync(TId id, CancellationToken token);
    }
}