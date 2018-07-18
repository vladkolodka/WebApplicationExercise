using System.Data.Entity.Migrations;

namespace WebApplicationExercise.Migrations
{
    public partial class Product_NameMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(maxLength: 100));
        }

        public override void Down()
        {
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
    }
}