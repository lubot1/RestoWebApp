namespace RestoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restaurantTableFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Restaurants", "OwnerID", "dbo.Owners");
            DropIndex("dbo.Restaurants", new[] { "OwnerID" });
            AddColumn("dbo.Restaurants", "RestaurantName", c => c.String());
            AddColumn("dbo.Owners", "Restaurant_RestaurantID", c => c.Int());
            CreateIndex("dbo.Owners", "Restaurant_RestaurantID");
            AddForeignKey("dbo.Owners", "Restaurant_RestaurantID", "dbo.Restaurants", "RestaurantID");
            DropColumn("dbo.Restaurants", "OwnerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Restaurants", "OwnerID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Owners", "Restaurant_RestaurantID", "dbo.Restaurants");
            DropIndex("dbo.Owners", new[] { "Restaurant_RestaurantID" });
            DropColumn("dbo.Owners", "Restaurant_RestaurantID");
            DropColumn("dbo.Restaurants", "RestaurantName");
            CreateIndex("dbo.Restaurants", "OwnerID");
            AddForeignKey("dbo.Restaurants", "OwnerID", "dbo.Owners", "OwnerID", cascadeDelete: true);
        }
    }
}
