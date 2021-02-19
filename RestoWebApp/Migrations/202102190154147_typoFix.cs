namespace RestoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class typoFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Owners", "Owner_OwnerID", "dbo.Owners");
            DropForeignKey("dbo.Owners", "Restaurant_RestaurantID", "dbo.Restaurants");
            DropIndex("dbo.Owners", new[] { "Owner_OwnerID" });
            DropIndex("dbo.Owners", new[] { "Restaurant_RestaurantID" });
            CreateTable(
                "dbo.OwnerRestaurants",
                c => new
                    {
                        Owner_OwnerID = c.Int(nullable: false),
                        Restaurant_RestaurantID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Owner_OwnerID, t.Restaurant_RestaurantID })
                .ForeignKey("dbo.Owners", t => t.Owner_OwnerID, cascadeDelete: true)
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_RestaurantID, cascadeDelete: true)
                .Index(t => t.Owner_OwnerID)
                .Index(t => t.Restaurant_RestaurantID);
            
            DropColumn("dbo.Owners", "Owner_OwnerID");
            DropColumn("dbo.Owners", "Restaurant_RestaurantID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Owners", "Restaurant_RestaurantID", c => c.Int());
            AddColumn("dbo.Owners", "Owner_OwnerID", c => c.Int());
            DropForeignKey("dbo.OwnerRestaurants", "Restaurant_RestaurantID", "dbo.Restaurants");
            DropForeignKey("dbo.OwnerRestaurants", "Owner_OwnerID", "dbo.Owners");
            DropIndex("dbo.OwnerRestaurants", new[] { "Restaurant_RestaurantID" });
            DropIndex("dbo.OwnerRestaurants", new[] { "Owner_OwnerID" });
            DropTable("dbo.OwnerRestaurants");
            CreateIndex("dbo.Owners", "Restaurant_RestaurantID");
            CreateIndex("dbo.Owners", "Owner_OwnerID");
            AddForeignKey("dbo.Owners", "Restaurant_RestaurantID", "dbo.Restaurants", "RestaurantID");
            AddForeignKey("dbo.Owners", "Owner_OwnerID", "dbo.Owners", "OwnerID");
        }
    }
}
