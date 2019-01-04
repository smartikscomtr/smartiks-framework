namespace Smartiks.Framework.Data.Abstractions
{
    public interface IQueryableService<TQueryable, TListQuery, TId> : IReadOnlyQueryableService<TQueryable, TListQuery, TId>, IWriteOnlyQueryableService<TQueryable, TId>
        where TQueryable : class, IId<TId>
        where TListQuery : Query<TQueryable>
    {
    }
}