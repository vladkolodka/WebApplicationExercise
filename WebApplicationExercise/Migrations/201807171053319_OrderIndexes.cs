using System.Data.Entity.Migrations;

namespace WebApplicationExercise.Migrations
{
    public partial class OrderIndexes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "Customer", c => c.String(maxLength: 100));
            CreateIndex("dbo.Orders", "CreatedDate");
            CreateIndex("dbo.Orders", "Customer");
        }

        public override void Down()
        {
            DropIndex("dbo.Orders", new[] {"Customer"});
            DropIndex("dbo.Orders", new[] {"CreatedDate"});
            AlterColumn("dbo.Orders", "Customer", c => c.String());
        }
    }
}