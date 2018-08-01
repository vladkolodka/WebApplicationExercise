namespace WebApplicationExercise.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveOrder_IsVisible : DbMigration
    {
        public override void Down()
        {
            this.AddColumn("dbo.Orders", "IsVisible", c => c.Boolean(false));
        }

        public override void Up()
        {
            this.DropColumn("dbo.Orders", "IsVisible");
        }
    }
}