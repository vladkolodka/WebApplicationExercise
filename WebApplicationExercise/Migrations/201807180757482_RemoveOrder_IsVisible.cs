using System.Data.Entity.Migrations;

namespace WebApplicationExercise.Migrations
{
    public partial class RemoveOrder_IsVisible : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "IsVisible");
        }

        public override void Down()
        {
            AddColumn("dbo.Orders", "IsVisible", c => c.Boolean(false));
        }
    }
}