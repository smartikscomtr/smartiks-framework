namespace Smartiks.Framework.Data.Abstractions
{
    public interface IId<TId>
    {
        TId Id { get; }
    }
}
