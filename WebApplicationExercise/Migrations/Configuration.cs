using System.Data.Entity.Migrations;
using WebApplicationExercise.Core;

namespace WebApplicationExercise.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MainDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WebApplicationExercise.Core.MainDataContext";
        }

        protected override void Seed(MainDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}