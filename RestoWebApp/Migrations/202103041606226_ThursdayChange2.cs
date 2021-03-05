namespace RestoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThursdayChange2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Restaurants", "RestaurantCategoryID", c => c.Int(nullable: true));
            CreateIndex("dbo.Restaurants", "RestaurantCategoryID");
            AddForeignKey("dbo.Restaurants", "RestaurantCategoryID", "dbo.RestaurantCategories", "RestaurantCategoryID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Restaurants", "RestaurantCategoryID", "dbo.RestaurantCategories");
            DropIndex("dbo.Restaurants", new[] { "RestaurantCategoryID" });
            DropColumn("dbo.Restaurants", "RestaurantCategoryID");
        }
    }
}
