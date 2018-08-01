namespace WebApplicationExercise.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Relation_Order_Product : DbMigration
    {
        public override void Down()
        {
            this.DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            this.DropIndex("dbo.Products", new[] { "Order_Id" });
            this.AlterColumn("dbo.Products", "Order_Id", c => c.Guid());
            this.CreateIndex("dbo.Products", "Order_Id");
            this.AddForeignKey("dbo.Products", "Order_Id", "dbo.Orders", "Id");
        }

        public override void Up()
        {
            this.DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            this.DropIndex("dbo.Products", new[] { "Order_Id" });
            this.AlterColumn("dbo.Products", "Order_Id", c => c.Guid(false));
            this.CreateIndex("dbo.Products", "Order_Id");
            this.AddForeignKey("dbo.Products", "Order_Id", "dbo.Orders", "Id", true);
        }
    }
}