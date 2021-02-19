namespace RestoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MtoMRelationshipFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Owners", "Owner_OwnerID", c => c.Int());
            CreateIndex("dbo.Owners", "Owner_OwnerID");
            AddForeignKey("dbo.Owners", "Owner_OwnerID", "dbo.Owners", "OwnerID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Owners", "Owner_OwnerID", "dbo.Owners");
            DropIndex("dbo.Owners", new[] { "Owner_OwnerID" });
            DropColumn("dbo.Owners", "Owner_OwnerID");
        }
    }
}
