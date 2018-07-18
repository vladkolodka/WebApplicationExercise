using System.Data.Entity;
using WebApplicationExercise.Models;

namespace WebApplicationExercise.Core
{
    public class MainDataContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasMany(o => o.Products).WithRequired(p => p.Order).WillCascadeOnDelete(true);

            modelBuilder.Entity<Order>().HasIndex(o => o.Customer).IsClustered(false);
            modelBuilder.Entity<Order>().HasIndex(o => o.CreatedDate).IsClustered(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}