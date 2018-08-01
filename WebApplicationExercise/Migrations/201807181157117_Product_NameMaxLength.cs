namespace WebApplicationExercise.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Product_NameMaxLength : DbMigration
    {
        public override void Down()
        {
            this.AlterColumn("dbo.Products", "Name", c => c.String());
        }

        public override void Up()
        {
            this.AlterColumn("dbo.Products", "Name", c => c.String(maxLength: 100));
        }
    }
}