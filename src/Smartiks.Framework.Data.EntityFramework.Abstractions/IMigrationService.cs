using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smartiks.Framework.Data.EntityFramework.Abstractions
{
    public interface IMigrationService<TContext>
        where TContext : DbContext
    {
        Task<IEnumerable<string>> GetMigrationsAsync();
        Task<IEnumerable<string>> GetAppliedMigrationsAsync();
        Task<IEnumerable<string>> GetPendingMigrationsAsync();
        Task<bool> NeedMigrationAsync();
        Task MigrateAsync(string targetMigration = null);
    }
}