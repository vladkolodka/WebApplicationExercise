using System.Data.Entity.Migrations;

namespace WebApplicationExercise.Migrations
{
    public partial class Relation_Order_Product : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Products", new[] {"Order_Id"});
            AlterColumn("dbo.Products", "Order_Id", c => c.Guid(false));
            CreateIndex("dbo.Products", "Order_Id");
            AddForeignKey("dbo.Products", "Order_Id", "dbo.Orders", "Id", true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Products", new[] {"Order_Id"});
            AlterColumn("dbo.Products", "Order_Id", c => c.Guid());
            CreateIndex("dbo.Products", "Order_Id");
            AddForeignKey("dbo.Products", "Order_Id", "dbo.Orders", "Id");
        }
    }
}