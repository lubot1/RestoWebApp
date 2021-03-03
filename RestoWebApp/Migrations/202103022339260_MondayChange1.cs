namespace RestoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MondayChange1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OwnerxRestaurants", "Primary");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OwnerxRestaurants", "Primary", c => c.Boolean(nullable: false));
        }
    }
}
