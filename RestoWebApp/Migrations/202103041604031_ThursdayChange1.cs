namespace RestoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThursdayChange1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RestaurantCategories",
                c => new
                    {
                        RestaurantCategoryID = c.Int(nullable: false, identity: true),
                        RestaurantCategoryDesc = c.String(),
                    })
                .PrimaryKey(t => t.RestaurantCategoryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RestaurantCategories");
        }
    }
}
