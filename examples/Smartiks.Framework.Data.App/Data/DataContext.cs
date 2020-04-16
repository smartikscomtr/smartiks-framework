using Microsoft.EntityFrameworkCore;
using Smartiks.Framework.Data.App.Model;

namespace Smartiks.Framework.Data.App.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}