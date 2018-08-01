namespace WebApplicationExercise.Core
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using WebApplicationExercise.Models;

    /// <inheritdoc />
    /// <summary>
    ///     Main database EF context
    /// </summary>
    public class MainDataContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public override int SaveChanges()
        {
            this.OnSave();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            this.OnSave();
            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.OnSave();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasMany(o => o.Products).WithRequired(p => p.Order).WillCascadeOnDelete(true);

            modelBuilder.Entity<Order>().HasIndex(o => o.Customer).IsClustered(false);
            modelBuilder.Entity<Order>().HasIndex(o => o.CreatedDate).IsClustered(false);

            base.OnModelCreating(modelBuilder);
        }

        private void OnSave()
        {
            var concurrentEntries = this.ChangeTracker.Entries().Where(
                x => x.Entity is ISequentialIdEntity
                     && (x.State == EntityState.Added || x.State == EntityState.Modified)).Select(e => e.Entity);

            foreach (var entry in concurrentEntries)
            {
                ((ISequentialIdEntity)entry).GenerateId();
            }
        }
    }
}