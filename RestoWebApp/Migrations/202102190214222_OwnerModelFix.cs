namespace RestoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnerModelFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OwnerRestaurants", "Owner_OwnerID", "dbo.Owners");
            DropForeignKey("dbo.OwnerRestaurants", "Restaurant_RestaurantID", "dbo.Restaurants");
            DropIndex("dbo.OwnerRestaurants", new[] { "Owner_OwnerID" });
            DropIndex("dbo.OwnerRestaurants", new[] { "Restaurant_RestaurantID" });
            AddColumn("dbo.Owners", "Restaurant_RestaurantID", c => c.Int());
            CreateIndex("dbo.Owners", "Restaurant_RestaurantID");
            AddForeignKey("dbo.Owners", "Restaurant_RestaurantID", "dbo.Restaurants", "RestaurantID");
            DropTable("dbo.OwnerRestaurants");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OwnerRestaurants",
                c => new
                    {
                        Owner_OwnerID = c.Int(nullable: false),
                        Restaurant_RestaurantID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Owner_OwnerID, t.Restaurant_RestaurantID });
            
            DropForeignKey("dbo.Owners", "Restaurant_RestaurantID", "dbo.Restaurants");
            DropIndex("dbo.Owners", new[] { "Restaurant_RestaurantID" });
            DropColumn("dbo.Owners", "Restaurant_RestaurantID");
            CreateIndex("dbo.OwnerRestaurants", "Restaurant_RestaurantID");
            CreateIndex("dbo.OwnerRestaurants", "Owner_OwnerID");
            AddForeignKey("dbo.OwnerRestaurants", "Restaurant_RestaurantID", "dbo.Restaurants", "RestaurantID", cascadeDelete: true);
            AddForeignKey("dbo.OwnerRestaurants", "Owner_OwnerID", "dbo.Owners", "OwnerID", cascadeDelete: true);
        }
    }
}
