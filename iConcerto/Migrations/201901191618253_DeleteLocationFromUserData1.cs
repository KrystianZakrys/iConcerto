namespace iConcerto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteLocationFromUserData1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDatas", "Locations_ID", "dbo.Locations");
            DropIndex("dbo.UserDatas", new[] { "Locations_ID" });
            DropColumn("dbo.UserDatas", "Locations_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserDatas", "Locations_ID", c => c.Int());
            CreateIndex("dbo.UserDatas", "Locations_ID");
            AddForeignKey("dbo.UserDatas", "Locations_ID", "dbo.Locations", "ID");
        }
    }
}
