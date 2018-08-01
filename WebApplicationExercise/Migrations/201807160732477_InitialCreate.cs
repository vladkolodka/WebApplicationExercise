namespace WebApplicationExercise.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Down()
        {
            this.DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            this.DropIndex("dbo.Products", new[] { "Order_Id" });
            this.DropTable("dbo.Products");
            this.DropTable("dbo.Orders");
        }

        public override void Up()
        {
            this.CreateTable(
                    "dbo.Orders",
                    c => new { Id = c.Guid(false, true), CreatedDate = c.DateTime(false), Customer = c.String() })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                    "dbo.Products",
                    c => new
 {
                                 Id = c.Guid(false, true),
                                 Name = c.String(),
                                 Price = c.Double(false),
                                 Order_Id = c.Guid()
                             })
                .PrimaryKey(t => t.Id).ForeignKey("dbo.Orders", t => t.Order_Id).Index(t => t.Order_Id);
        }
    }
}