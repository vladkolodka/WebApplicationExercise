using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplicationExercise.Models;

namespace WebApplicationExercise.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     Main database EF context
    /// </summary>
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

        public override int SaveChanges()
        {
            OnSave();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            OnSave();
            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            OnSave();
            return base.SaveChangesAsync(cancellationToken);
        }


        private void OnSave()
        {
            var concurrentEntries = ChangeTracker.Entries().Where(x =>
                    x.Entity is ISequentialIdEntity &&
                    (x.State == EntityState.Added || x.State == EntityState.Modified))
                .Select(e => e.Entity);

            foreach (var entry in concurrentEntries)
            {
                ((ISequentialIdEntity) entry).GenerateId();
            }
        }
    }
}