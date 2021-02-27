namespace RestoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FridayChange3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OwnerxRestaurants",
                c => new
                    {
                        OwnerxRestaurantID = c.Int(nullable: false, identity: true),
                        OwnerID = c.Int(nullable: false),
                        RestaurantID = c.Int(nullable: false),
                        Primary = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OwnerxRestaurantID)
                .ForeignKey("dbo.Owners", t => t.OwnerID, cascadeDelete: true)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantID, cascadeDelete: true)
                .Index(t => t.OwnerID)
                .Index(t => t.RestaurantID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OwnerxRestaurants", "RestaurantID", "dbo.Restaurants");
            DropForeignKey("dbo.OwnerxRestaurants", "OwnerID", "dbo.Owners");
            DropIndex("dbo.OwnerxRestaurants", new[] { "RestaurantID" });
            DropIndex("dbo.OwnerxRestaurants", new[] { "OwnerID" });
            DropTable("dbo.OwnerxRestaurants");
        }
    }
}
