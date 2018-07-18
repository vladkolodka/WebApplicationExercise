using System.Data.Entity.Migrations;

namespace WebApplicationExercise.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Orders",
                    c => new
                    {
                        Id = c.Guid(false, true),
                        CreatedDate = c.DateTime(false),
                        Customer = c.String()
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.Products",
                    c => new
                    {
                        Id = c.Guid(false, true),
                        Name = c.String(),
                        Price = c.Double(false),
                        Order_Id = c.Guid()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Order_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Products", new[] {"Order_Id"});
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
        }
    }
}