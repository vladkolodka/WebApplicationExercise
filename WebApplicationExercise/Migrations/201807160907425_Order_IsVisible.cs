namespace WebApplicationExercise.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Order_IsVisible : DbMigration
    {
        public override void Down()
        {
            this.DropColumn("dbo.Orders", "IsVisible");
        }

        public override void Up()
        {
            this.AddColumn("dbo.Orders", "IsVisible", c => c.Boolean(false, true));
        }
    }
}