using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Smartiks.Framework.Data.EntityFramework.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartiks.Framework.Data.EntityFramework
{
    public class ContextMigrationService<TContext> : IMigrationService<TContext>
        where TContext : DbContext
    {
        private readonly TContext _context;

        public ContextMigrationService(TContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<string>> GetMigrationsAsync()
        {
            _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));

            return Task.FromResult(_context.Database.GetMigrations());
        }

        public Task<IEnumerable<string>> GetAppliedMigrationsAsync()
        {
            _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));

            return _context.Database.GetAppliedMigrationsAsync();
        }

        public Task<IEnumerable<string>> GetPendingMigrationsAsync()
        {
            _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));

            return _context.Database.GetPendingMigrationsAsync();
        }

        public async Task<bool> NeedMigrationAsync()
        {
            var pendingMigrations = await GetPendingMigrationsAsync();

            return pendingMigrations.Any();
        }

        public async Task MigrateAsync(string targetMigration)
        {
            _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));

            var infrastructure = _context.GetInfrastructure();

            var migrator = infrastructure.GetService<IMigrator>();

            await migrator.MigrateAsync(targetMigration);
        }
    }
}