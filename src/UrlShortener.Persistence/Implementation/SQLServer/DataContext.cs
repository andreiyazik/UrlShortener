using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Persistence.Implementation.SQLServer
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Url> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating( builder );
        }
    }
}