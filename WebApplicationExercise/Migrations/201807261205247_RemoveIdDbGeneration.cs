namespace WebApplicationExercise.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveIdDbGeneration : DbMigration
    {
        public override void Down()
        {
            this.DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            this.DropPrimaryKey("dbo.Products");
            this.DropPrimaryKey("dbo.Orders");
            this.AlterColumn("dbo.Products", "Id", c => c.Guid(false, true));
            this.AlterColumn("dbo.Orders", "Id", c => c.Guid(false, true));
            this.AddPrimaryKey("dbo.Products", "Id");
            this.AddPrimaryKey("dbo.Orders", "Id");
            this.AddForeignKey("dbo.Products", "Order_Id", "dbo.Orders", "Id", true);
        }

        public override void Up()
        {
            this.DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            this.DropPrimaryKey("dbo.Orders");
            this.DropPrimaryKey("dbo.Products");
            this.AlterColumn("dbo.Orders", "Id", c => c.Guid(false));
            this.AlterColumn("dbo.Products", "Id", c => c.Guid(false));
            this.AddPrimaryKey("dbo.Orders", "Id");
            this.AddPrimaryKey("dbo.Products", "Id");
            this.AddForeignKey("dbo.Products", "Order_Id", "dbo.Orders", "Id", true);
        }
    }
}