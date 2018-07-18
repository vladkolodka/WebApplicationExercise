using System.Data.Entity.Migrations;

namespace WebApplicationExercise.Migrations
{
    public partial class Order_IsVisible : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsVisible", c => c.Boolean(false, true));
        }

        public override void Down()
        {
            DropColumn("dbo.Orders", "IsVisible");
        }
    }
}