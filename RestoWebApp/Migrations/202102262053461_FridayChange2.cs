namespace RestoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FridayChange2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Owners", "Restaurant_RestaurantID", "dbo.Restaurants");
            DropIndex("dbo.Owners", new[] { "Restaurant_RestaurantID" });
            DropColumn("dbo.Owners", "Restaurant_RestaurantID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Owners", "Restaurant_RestaurantID", c => c.Int());
            CreateIndex("dbo.Owners", "Restaurant_RestaurantID");
            AddForeignKey("dbo.Owners", "Restaurant_RestaurantID", "dbo.Restaurants", "RestaurantID");
        }
    }
}
