using Desafio.Dio.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Dio.Repository.Context
{
    public class DesafioContext : DbContext
    {
        public DesafioContext(DbContextOptions<DesafioContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DesafioContext).Assembly);
        }
    }
}