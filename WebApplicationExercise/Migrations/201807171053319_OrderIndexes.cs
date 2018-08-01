namespace WebApplicationExercise.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class OrderIndexes : DbMigration
    {
        public override void Down()
        {
            this.DropIndex("dbo.Orders", new[] { "Customer" });
            this.DropIndex("dbo.Orders", new[] { "CreatedDate" });
            this.AlterColumn("dbo.Orders", "Customer", c => c.String());
        }

        public override void Up()
        {
            this.AlterColumn("dbo.Orders", "Customer", c => c.String(maxLength: 100));
            this.CreateIndex("dbo.Orders", "CreatedDate");
            this.CreateIndex("dbo.Orders", "Customer");
        }
    }
}