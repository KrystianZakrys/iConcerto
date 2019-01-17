namespace iConcerto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PoprawkaWyświetlaniaEvents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LocationsEvents", "Locations_ID", "dbo.Locations");
            DropForeignKey("dbo.LocationsEvents", "Events_ID", "dbo.Events");
            DropForeignKey("dbo.UserDataEvents", "UserData_ID", "dbo.UserDatas");
            DropForeignKey("dbo.UserDataEvents", "Events_ID", "dbo.Events");
            DropIndex("dbo.LocationsEvents", new[] { "Locations_ID" });
            DropIndex("dbo.LocationsEvents", new[] { "Events_ID" });
            DropIndex("dbo.UserDataEvents", new[] { "UserData_ID" });
            DropIndex("dbo.UserDataEvents", new[] { "Events_ID" });
            AddColumn("dbo.Events", "LocationId", c => c.Int(nullable: false));
            AddColumn("dbo.Events", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Events", "Locations_ID", c => c.Int());
            AddColumn("dbo.Events", "UserData_ID", c => c.Int());
            CreateIndex("dbo.Events", "Locations_ID");
            CreateIndex("dbo.Events", "UserData_ID");
            AddForeignKey("dbo.Events", "Locations_ID", "dbo.Locations", "ID");
            AddForeignKey("dbo.Events", "UserData_ID", "dbo.UserDatas", "ID");
            DropTable("dbo.LocationsEvents");
            DropTable("dbo.UserDataEvents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserDataEvents",
                c => new
                    {
                        UserData_ID = c.Int(nullable: false),
                        Events_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserData_ID, t.Events_ID });
            
            CreateTable(
                "dbo.LocationsEvents",
                c => new
                    {
                        Locations_ID = c.Int(nullable: false),
                        Events_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Locations_ID, t.Events_ID });
            
            DropForeignKey("dbo.Events", "UserData_ID", "dbo.UserDatas");
            DropForeignKey("dbo.Events", "Locations_ID", "dbo.Locations");
            DropIndex("dbo.Events", new[] { "UserData_ID" });
            DropIndex("dbo.Events", new[] { "Locations_ID" });
            DropColumn("dbo.Events", "UserData_ID");
            DropColumn("dbo.Events", "Locations_ID");
            DropColumn("dbo.Events", "UserId");
            DropColumn("dbo.Events", "LocationId");
            CreateIndex("dbo.UserDataEvents", "Events_ID");
            CreateIndex("dbo.UserDataEvents", "UserData_ID");
            CreateIndex("dbo.LocationsEvents", "Events_ID");
            CreateIndex("dbo.LocationsEvents", "Locations_ID");
            AddForeignKey("dbo.UserDataEvents", "Events_ID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.UserDataEvents", "UserData_ID", "dbo.UserDatas", "ID", cascadeDelete: true);
            AddForeignKey("dbo.LocationsEvents", "Events_ID", "dbo.Events", "ID", cascadeDelete: true);
            AddForeignKey("dbo.LocationsEvents", "Locations_ID", "dbo.Locations", "ID", cascadeDelete: true);
        }
    }
}
