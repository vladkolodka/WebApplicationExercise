using System.Data.Entity.Migrations;

namespace WebApplicationExercise.Migrations
{
    public partial class RemoveIdDbGeneration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            DropPrimaryKey("dbo.Orders");
            DropPrimaryKey("dbo.Products");
            AlterColumn("dbo.Orders", "Id", c => c.Guid(false));
            AlterColumn("dbo.Products", "Id", c => c.Guid(false));
            AddPrimaryKey("dbo.Orders", "Id");
            AddPrimaryKey("dbo.Products", "Id");
            AddForeignKey("dbo.Products", "Order_Id", "dbo.Orders", "Id", true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            DropPrimaryKey("dbo.Products");
            DropPrimaryKey("dbo.Orders");
            AlterColumn("dbo.Products", "Id", c => c.Guid(false, true));
            AlterColumn("dbo.Orders", "Id", c => c.Guid(false, true));
            AddPrimaryKey("dbo.Products", "Id");
            AddPrimaryKey("dbo.Orders", "Id");
            AddForeignKey("dbo.Products", "Order_Id", "dbo.Orders", "Id", true);
        }
    }
}