namespace WebApplicationExercise.Migrations
{
    using System.Data.Entity.Migrations;

    using WebApplicationExercise.Core;

    internal sealed class Configuration : DbMigrationsConfiguration<MainDataContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
            this.ContextKey = "WebApplicationExercise.Core.MainDataContext";
        }

        protected override void Seed(MainDataContext context)
        {
            // This method will be called after migrating to the latest version.

            // You can use the DbSet<T>.AddOrUpdate() helper extension method 
            // to avoid creating duplicate seed data.
        }
    }
}